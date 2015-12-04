using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker.Comandos
{
    public class Cry : BloqueFuncion, IWait
    {
        public enum Efecto
        {
            Nada = 0,//poner todos los efectos...
        }
        WaitCry waitCry;
        public Cry(string idPokemon)
            : this(idPokemon, Efecto.Nada)
        { }
        public Cry(string idPokemon, Efecto efecto)
            : base("cry", new string[] { idPokemon, "" })
        {
            EfectoSonido = efecto;
        }
        public string Pokemon
        {
            get { return Args[0].Split('x')[1]; }
            set
            {
                if (value.Contains('x'))
                {

                    Bloque.CompruebaIdHex(value);

                    Args[0] = value;
                }
                else
                    Args[0] = "0x" + value;
                //mirar si esta dentro del rango 1-385?
            }
        }
        public Efecto EfectoSonido
        {
            get { return (Efecto)Convert.ToInt32(Args[1].Split('x')[1]); }
            set
            {
                Args[1] = "0x" + ((int)value);
            }
        }

        public override byte[] LineaEjecucionToBytes()
        {
            //por probar que Word =Short en bytes
            byte[] especiePokemon=WordToBytes(Pokemon);
            byte[] efecto = WordToBytes((int)EfectoSonido);
            return new byte[] { 0xA1,especiePokemon[0],especiePokemon[1],efecto[0],efecto[1] };
        } 

        #region Miembros de IWait

        public Bloque BloqueWait
        {
            get { 
                if (waitCry == null)
                      waitCry = new WaitCry();
                   return waitCry;
                }
        }

        #endregion
        public override string Ejemplo()
        {
            return "cry 0xNumHexPokemon. ej. cry 0xFF -> sonido celebi";
        }
        public override string Explicacion()
        {
            return "Se escucha el sonido del pokemon en cuestion";
        }

        #region Miembros de IWait


        public bool SonPareja(IWait bloquePareja)
        {
            return (bloquePareja as BloqueFuncion).Funcion == "waitcry";
        }


        #endregion


    }
    public class WaitCry : BloqueFuncion,IWait
    {
        public WaitCry() : base("waitCry") { }
        public override string Explicacion()
        {
            return " espera a escuchar el sonido del pokemon";
        }
        public override byte[] LineaEjecucionToBytes()
        {
            return new byte[] { 0xC5 };
        }
        #region Miembros de IWait

        public Bloque BloqueWait
        {
            get { return null; }
        }

        public bool SonPareja(IWait bloquePareja)
        {
            return (bloquePareja as BloqueFuncion).Funcion == "cry";
        }

        #endregion
    }
}
