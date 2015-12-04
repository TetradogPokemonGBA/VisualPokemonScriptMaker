using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public abstract class BloqueList:BloqueCompilable
    {
        List<string> list;
        private string prefix;
        private byte lastItem;
        const byte EMPTY = 0x00;

       public BloqueList(string idBloque, string prefix,byte lastItem):base(idBloque)
       {
           // TODO: Complete member initialization
           this.prefix = prefix;
           this.lastItem = lastItem;
           list = new List<string>();
       }
       public BloqueList(string idBloque, string prefix)
           : this(idBloque,prefix,EMPTY)
       {
       }
       protected List<string> List
        {
            get { return list; }
           private set { list = value; }
        }
       public override string Declaracion()
       {
           string declaracionBloqueList = "#org @" + IdBloque + "\n";
           byte[] bytesDeclaracion = DeclaracionToBytes();
           for (int i = 0; i < bytesDeclaracion.Length; i++)
               declaracionBloqueList += prefix + " 0x" + ((Hex)((int)bytesDeclaracion[i]))+"\n";
           return declaracionBloqueList;
       }
       public override byte[] DeclaracionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            string objetoAPoner="";

            bool añadir;
            byte byteHaAñadir=0x00;
            List<byte> bytesDeclaracion=new List<byte> ();
            for (int i = 0; i < List.Count; i++)
            {
                objetoAPoner =DameByteString(List[i]);
                añadir = lastItem == EMPTY;

                byteHaAñadir = Convert.ToByte(" 0x" + objetoAPoner);
                if (!añadir)
                {
                    if (byteHaAñadir != lastItem && i < List.Count - 1)
                        añadir = true;
                }
                if (añadir)
                    bytesDeclaracion.Add(byteHaAñadir);

            }
            if (lastItem != EMPTY &&( byteHaAñadir != lastItem&&List.Count!=0))
                bytesDeclaracion.Add(lastItem);
            return bytesDeclaracion.ToArray();
        }

        protected string DameByteString(string stringByte)
        {
            string[] campos;
            string byteLimpio = "";
            if (stringByte.Contains(' '))
            {
                campos = stringByte.Split(' ');
                byteLimpio = campos[campos.Length - 1];

            }
            else
            {
                byteLimpio = stringByte;

            }
            if (byteLimpio.Contains('x'))
            {
                byteLimpio = byteLimpio.Split('x')[1];
            }
            return byteLimpio;
        }
        protected virtual bool ValidaString(string stringHaValidar)
        { return true; }
        public void Añadir(IEnumerable<string> elementos)
        {
            foreach (string elemento in elementos)
                Añadir(elemento);
        }
        public void Añadir(string elementoLista)
        {
            if (ValidaString(elementoLista))
                List.Add(elementoLista);
            else
                throw new ArgumentException("el elemento"+elementoLista+" no es valido");
        }
        public virtual void AñadirEnd() { }
        public virtual void QuitarEnd() { }
        public void Quitar(IEnumerable<string> elementos)
        {
            foreach (string elemento in elementos)
                Quitar(elemento);
        }
        public void Quitar(string elementoLista)
        {
            List.Remove(elementoLista);
        }
        public void Quitar(int posicionElemento)
        {
            List.RemoveAt(posicionElemento);
        }
        public void Añadir(string elemento, int posicion)
        {
            if (ValidaString(elemento))
                List.Insert(posicion,elemento);
        }






    }
}
