using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public abstract class BloqueFuncion:Bloque
    {
        string funcion;
        string[] args;

        public BloqueFuncion(string funcion, string[] args)
            : base("")
        {
            this.funcion = funcion;
            this.args = args;
        }
        public BloqueFuncion(string funcion) : this(funcion, new string[] { }) { }
        public string Funcion
        {
            get { return funcion; }
            set {
                if (value == null) throw new  NullReferenceException("Tiene que haber una funcion");
                funcion = value; }
        }
        public string[] Args
        {
            get { return args; }
            set {
                if (value == null)
                    value = new string[] { };
                args = value; 
                }
        }
        public override string LineaEjecucion()
        {
            string lineaEjecucion = funcion;
            for (int i = 0; i < args.Length; i++)
                if(args[i]!=null)
                   lineaEjecucion += " " + args[i];
            return lineaEjecucion;
        }
        public abstract byte[] LineaEjecucionToBytes();
        public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {
            return LineaEjecucionToBytes();
        }

    }
}
