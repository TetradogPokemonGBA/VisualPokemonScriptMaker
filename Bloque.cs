using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.GBA;
using Gabriel.Cat.Extension;
using VisualScripsMaker;

namespace ScriptPokemonMaker
{
    public delegate void IdChangedEventHandler(Bloque bloqueCambiado);
    public abstract class Bloque : IComparable<Bloque>, IComparable
    {
        #region Constantes
        public static readonly byte[] LASTRESULT =WordToBytes("800D");
        public static readonly byte[] LASTTALKED = WordToBytes("800F");
        #endregion
        string idBloque;
        public event IdChangedEventHandler IdCambiado;
        public Bloque(string id)
        {
            if (id.Trim().Contains(' '))
                throw new Exception("Solo puede ser una palabra el id");
            this.idBloque = id;
        }
        public string IdBloque
        {
            get { return idBloque; }
            set { idBloque = value;
            if (IdCambiado != null)
                IdCambiado(this);
            }
        }


        public abstract string LineaEjecucion();

        public virtual string Explicacion()
        {
            return "Pendiente de explicacion";
        }
        public virtual string Ejemplo()
        {
            return "Pendiente de explicacion";
        }
        public int CompareTo(Bloque other)
        {
            if (other != null)
                return idBloque.CompareTo(other.idBloque);
            else
                return -1;
        }
        public int CompareTo(object obj)
        {
            return CompareTo(obj as Bloque);
        }

        public override string ToString()
        {
            return "";
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Bloque);
        }
        public bool Equals(Bloque obj)
        {
            bool iguales = obj!=null;
            if (iguales)
                iguales= LineaEjecucion().Equals(obj.LineaEjecucion());
            return iguales;
        }
        /// <summary>
        /// Convierte el bloque script en bytes listos para poner en un punto del GBA :D
        /// </summary>
        /// <returns></returns>

        public abstract byte[] LineaEjecucionToBytes(RomPokemon rom);//si es necesario compilar antes una parte para poder compilar la siguiente...msgbox @mensajeACompilarAntes 0x6
        




        protected static void CompruebaIdHex(string value)
        {
            CompruebaIdHex(value, true);
        }
        protected static bool CompruebaIdHex(string value,bool excepcion)
        {
            bool correcto = true;
            if (!value.Contains('x') || value.Split('x').Length > 2 || !Hex.ValidaString(value.Split('x')[1]))
                correcto = false;
            if(!correcto&&excepcion)
                throw new Exception("El id es erroneo");
            return correcto;
        }

        protected static bool ValidaNumeroHex(string valorHex, string valorMinHex, string valorMaxHex)
        {
            Hex numHexAcomprobar = valorHex;
            return numHexAcomprobar > ((Hex)valorMaxHex) || numHexAcomprobar < ((Hex)valorMinHex);
        }

        public static Hex InicioBuquedaEspacio { get {
            return BinarioGBA.InicioBusquedaEspacio;
        }
            set {
                BinarioGBA.InicioBusquedaEspacio = value;
            }
        }
        /// <summary>
        /// devuelve los dos bytes del numero
        /// </summary>
        /// <param name="numHex"></param>
        /// <returns></returns>
        public static byte[] WordToBytes(Hex numHex)
        {
            return BitConverter.GetBytes(Convert.ToInt16((int)numHex));
        }
        /// <summary>
        /// devuelve los cuatro bytes del numero
        /// </summary>
        /// <param name="numHex"></param>
        /// <returns></returns>
        public static byte[] PointerToBytes(Hex numHex)
        {
            return BitConverter.GetBytes(Convert.ToInt32((int)numHex));
        }
    }
}
