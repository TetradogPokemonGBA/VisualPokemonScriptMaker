using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public  class CheckObedience:FuncionCheck
    {
        public CheckObedience(string posPokemonEquipo) : base("checkObedience", new string[] { "0x" + posPokemonEquipo }) { }
        public string PosicionPokemonEnEquipo
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
            byte[] word = WordToBytes(PosicionPokemonEnEquipo);
            return new byte[] { 0xCE, word[0], word[1] };
        }
    }
}
