using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public class BloqueMovimiento : BloqueList, IWait
    {
        string numPersonaje;
        WaitMovement bloqueWait;
        public BloqueMovimiento(string numPersonaje, string idBloque)
            : base(idBloque, "#raw ", 0xFE)
        {
            NumPersonaje = numPersonaje;
        }
        #region Miembros de IWait

        public Bloque BloqueWait
        {
            get
            {
                if (bloqueWait == null)
                    bloqueWait = new WaitMovement(numPersonaje);
                return bloqueWait;

            }
        }
        public bool SonPareja(IWait bloquePareja)
        {
            return (bloquePareja as BloqueFuncion).LineaEjecucion() == "waitmovement 0x" + numPersonaje;
        }

        #endregion
        public string NumPersonaje
        {
            get { return numPersonaje; }
            set
            {
                WaitMovement wait = BloqueWait as WaitMovement;
                wait.NumPersonaje = value;
                numPersonaje = wait.NumPersonaje;


            }
        }
        protected override bool ValidaString(string stringHaValidar)
        {
            string byteString;
            bool valido = List.Count == 0;
            if (!valido)
            {
                valido = "FE" != DameByteString(List[List.Count - 1]).ToUpper();

                if (valido)
                {
                    byteString = DameByteString(stringHaValidar).ToUpper();
                    valido = ((Hex)byteString) < ((Hex)"FF");//si es mas grande no es valido
                }
            }
            return valido;
        }
        public override string LineaEjecucion()
        {
            return "applymovement 0x" + numPersonaje + " @" + IdBloque;
        }
        public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            //hay que mirar la ROM!!! no es lo mismo RubyZafiroEsmeralda que RojoFuegoVerdeHoja...ups es verdad no lo controlo desde aqui jeje...
            byte[] pjWord = WordToBytes(NumPersonaje);
            byte[] ptrMovs = PointerToBytes(Compilar(rom).Key);
            byte[] linea = new byte[7];
            linea[0] = 0x4F;
            for (int i = 0; i < pjWord.Length; i++)
                linea[i + 1] = pjWord[i];
            for (int i = 0; i < ptrMovs.Length; i++)
                linea[i + 3] = ptrMovs[i];
            return linea;
        }

        public override void AñadirEnd()
        {
            if (DameByteString(List[List.Count - 1]) != "FE")
                List.Add("FE");
        }

        public override void QuitarEnd()
        {
            if (DameByteString(List[List.Count - 1]) == "FE")
                List.RemoveAt(List.Count-1);
        }
    }
    public class WaitMovement : BloqueFuncion, IWait
    {
        public WaitMovement(string numPersonaje)
            : base("waitMovement", new string[] { "" })
        {
            NumPersonaje = numPersonaje;
        }

        public string NumPersonaje
        {
            get { return Args[0].Split('x')[1]; }
            set
            {
                string numPersonaje;
                if (value.Substring(0, 2) == "0x")
                {

                    Bloque.CompruebaIdHex(value);

                    numPersonaje = value;
                }
                else
                {
                    numPersonaje = "0x" + value;
                    Bloque.CompruebaIdHex(numPersonaje);
                }
                if (!ValidaNumeroHex(numPersonaje.Split('x')[1], "FF", "0"))//poner el maximo...y contar con el player
                    throw new IndexOutOfRangeException("EL numero " + numPersonaje.Split('x')[1] + " del persoanje no es valido!");
                Args[0] = numPersonaje;
            }

        }
        public override byte[] LineaEjecucionToBytes()
        {
            byte[] word = WordToBytes(NumPersonaje);
            return new byte[] { 0x51, word[0], word[1] };
        }

        #region Miembros de IWait

        public Bloque BloqueWait
        {
            get { return null; }//como me falta los movimientos no lo puedo crear...
        }

        public bool SonPareja(IWait bloquePareja)
        {
            bool sonPareja = bloquePareja is BloqueMovimiento;
            if (sonPareja)
                sonPareja = (bloquePareja as BloqueMovimiento).NumPersonaje == NumPersonaje;
            return sonPareja;
        }

        #endregion
    }
}
