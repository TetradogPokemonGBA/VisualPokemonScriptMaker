using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.Extension;
using Gabriel.Cat;
using VisualScripsMaker;
namespace ScriptPokemonMaker
{

    public class BloqueEjecucion : BloqueCompilable
    {
        List<Bloque> bloquesScript;
        public const byte END = 0x02;
        public const byte RETURN = 0x03;
        public BloqueEjecucion(string id)
            : base(id)
        {
            bloquesScript = new List<Bloque>();
        }
        public List<Bloque> BloquesScript
        {
            get { return bloquesScript; }
        }
        public string IdBloqueReturn
        {
            get { return IdBloque; }
        }
        public string IdBloqueEnd
        {
            get { return IdBloque + "End"; }
        }
        public override string ToString()
        {
            return LineaEjecucion();
        }
        private string CuerpoBloque()
        {
            string bloqueSinAcabar = "";
            for (int i = 0; i < bloquesScript.Count; i++)
                bloqueSinAcabar += bloquesScript[i].LineaEjecucion() + "\n";
            return bloqueSinAcabar;
        }
        public string BloqueEnd()
        {
            return "#Org @" + IdBloqueEnd + "\n" + LineaEjecucion() + "\nend";//llamo al bloque return y luego cuando acaba hago end asi tengo las dos versiones de la funcion y no duplico codigo
        }
        public string BloqueReturn()
        {
            return "#Org @" + IdBloqueReturn + "\n" + CuerpoBloque() + "return\n";
        }
        public override string Declaracion()
        {
            return BloqueReturn();
        }
        public override string LineaEjecucion()
        {
            return "call @" + IdBloque;
        }
        public string LineaEjecucionEnd()
        {
            return "call @" + IdBloqueEnd;
        }
        public string LineaEjecucionReturn()
        {
            return "call @" + IdBloqueReturn;
        }
        //control bien formado chosepath //mira si se cierran o si se abren las etiquetas que necesitan su pareja lock-release
        //la mejor practica es abrir  y cerrar en el mismo bloque asi no sa queda nada colgando...pero si es tiene que hacer 
        //un bloque end como camino alternativo no hay mas remedio que cerrar en el lo que se tiene abierto en el hilo principal
        public Bloque[] CierraBloque(BloqueEjecucion bloqueFinalContenido)
        {
            return BloquesSinPareja(bloqueFinalContenido, true);
        }
        public Bloque[] AbreBloque(BloqueEjecucion bloqueInicioContenido)
        {
            return BloquesSinPareja(bloqueInicioContenido, true);
        }
        public Bloque[] SinPareja()
        {
            return SinPareja(null);
        }
        public Bloque[] SinPareja(BloqueEjecucion bloqueContenido)
        {
            return BloquesSinPareja(bloqueContenido, false);
        }
        private Bloque[] BloquesSinPareja(BloqueEjecucion bloqueContenido, bool esBloqueEnd)
        {
            List<Bloque> parejasPorMirar = new List<Bloque>();
            List<Bloque> sinPareja = new List<Bloque>();
            IWait laPareja = null;
            IWait laOtraPareja = null;
            bool trobat = false;
            parejasPorMirar.AddRange(bloquesScript.Filtra((bloque) =>
            {
                bool seQueda = false;
                if (bloque != null)
                {
                    //cojo los que estan por encima del bloque porque son los que se tienen que cerrar con ese bloque
                    if (esBloqueEnd)
                    {
                        if (!trobat)
                            trobat = bloque == bloqueContenido;
                        if (esBloqueEnd)
                        {
                            if (!trobat)//si el bloque tiene que cerrar entonces hasta que no lo encuentra no empieza a cogerlos
                                seQueda = bloque is IWait;

                        }
                        else
                        {
                            if (trobat)//si el bloque tiene que abrir entonces hasta que no lo encuentra no para de cogerlos
                                seQueda = bloque is IWait;//los que estan por arriba (i/o ese mismo) no me interesan
                        }
                    }
                    else seQueda = bloque is IWait;//no filtro porque me interesan todos los IWait
                }
                return seQueda;

            }));
            if (bloqueContenido != null)//si hay un bloque que añada mas IWait
                parejasPorMirar.AddRange(bloqueContenido.bloquesScript.Filtra((bloque) => { return bloque is IWait; }));
            for (int i = 0; i < parejasPorMirar.Count; i++)
            {
                laPareja = parejasPorMirar[i] as IWait;
                laOtraPareja = null;
                parejasPorMirar.WhileEach((posiblePareja) =>
                {
                    laOtraPareja = posiblePareja as IWait;
                    if (!laPareja.SonPareja(laOtraPareja))
                        laOtraPareja = null;
                    return laOtraPareja == null;
                });
                if (laOtraPareja == null)
                    sinPareja.Add(parejasPorMirar[i]);
            }
            return sinPareja.ToTaula();
        }

        public override byte[] DeclaracionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            return DeclaracionReturnToBytes(rom, null);//bloque return
        }

        public byte[] DeclaracionEndToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {

            List<byte> bytes = IDeclaracionToBytes(rom, null);
            bytes.Add(END);
            return bytes.ToArray();
        }

        public byte[] DeclaracionReturnToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            List<byte> bytes = IDeclaracionToBytes(rom, null);
            bytes.Add(RETURN);
            return bytes.ToArray();
        }
        public  byte[] DeclaracionToBytes(Gabriel.Cat.GBA.RomPokemon rom, Llista<BloqueCompilado> bloquesCompilados)
        {
            return DeclaracionReturnToBytes(rom,bloquesCompilados);//bloque return
        }

        private List<byte> IDeclaracionToBytes(Gabriel.Cat.GBA.RomPokemon rom,Llista<BloqueCompilado> bloquesCompilados)
        {
            //hay que repensarlo...si quito los bloques puede   que no se pueda compilar otros bloques....
            List<Byte> bytes = new List<byte>();
            BloqueCompilado bloque = bloquesCompilados != null ? bloquesCompilados.Busca(IdBloque) : default(BloqueCompilado);
            if (!bloque.Equals(default(BloqueCompilado)))//lo quito para evitar la recursividad infinita
                bloquesCompilados.Elimina(bloque);
            for (int i = 0; i < this.bloquesScript.Count; i++)
                if (!(bloquesScript[i] is BloqueCompilable) || bloquesCompilados == null)
                    bytes.AddRange(bloquesScript[i].LineaEjecucionToBytes(rom));
                else
                {
                    //si el bloque compilable es de ejecucion puede que necesite para compilar los bloquesCompilados...evitar recursividad infinita...
                    //tambien mirar la linea de ejecucion to bytes!!
                    bytes.AddRange(bloquesCompilados.Busca(bloquesScript[i].IdBloque).LineaEjecucion);//pongo la linea compilada
                }
            if (!bloque.Equals(default(BloqueCompilado)))//lo pongo para...
                bloquesCompilados.Afegir(bloque);
            return bytes;
        }
        public byte[] DeclaracionEndToBytes(Gabriel.Cat.GBA.RomPokemon rom, Llista<BloqueCompilado> bloquesCompilados)
        {

            List<byte> bytes = IDeclaracionToBytes(rom,bloquesCompilados);
            bytes.Add(END);
            return bytes.ToArray();
        }

        public byte[] DeclaracionReturnToBytes(Gabriel.Cat.GBA.RomPokemon rom, Llista<BloqueCompilado> bloquesCompilados)
        {
            List<byte> bytes = IDeclaracionToBytes(rom,bloquesCompilados);
            bytes.Add(RETURN);
            return bytes.ToArray();
        }
        public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            return LineaEjecucionToBytes(rom,null);//bloque return
        }
        public byte[] LineaEjecucionEndToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            return LineaEjecucionEndToBytes(rom, null);
        }

        public byte[] LineaEjecucionReturnToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            return LineaEjecucionReturnToBytes(rom, null);
        }
        public  byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom, Llista<BloqueCompilado> bloquesCompilados)
        {
            return LineaEjecucionReturnToBytes(rom,bloquesCompilados);//bloque return
        } 
        public byte[] LineaEjecucionEndToBytes(Gabriel.Cat.GBA.RomPokemon rom,Llista<BloqueCompilado> bloquesCompilados)
        {
            List<byte> bytes = new List<byte>(CallBloqueEjecucion.CALL);
            bytes.AddRange(PointerToBytes(Gabriel.Cat.GBA.BinarioGBA.Edita(rom.ArchivoGbaPokemon,DeclaracionEndToBytes(rom)).Key));
            return bytes.ToArray();
        }

        public byte[] LineaEjecucionReturnToBytes(Gabriel.Cat.GBA.RomPokemon rom, Llista<BloqueCompilado> bloquesCompilados)
        {
            List<byte> bytes = new List<byte>(CallBloqueEjecucion.CALL);
            bytes.AddRange(PointerToBytes(Gabriel.Cat.GBA.BinarioGBA.Edita(rom.ArchivoGbaPokemon, DeclaracionReturnToBytes(rom)).Key));
            return bytes.ToArray();
        }

    }
}
