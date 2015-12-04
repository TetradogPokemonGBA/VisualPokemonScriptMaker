using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public class SetFlag : BloqueFuncion
    {
        public SetFlag(string flag)
            : base("setFlag", new string[] { "" })
        {
            Flag = flag;
        }

        public string Flag
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
            }
        }
        public override byte[] LineaEjecucionToBytes()
        {
            byte[] word = WordToBytes(Flag);
            return new byte[] { 0x29, word[0], word[1] };
        }
    }
}
