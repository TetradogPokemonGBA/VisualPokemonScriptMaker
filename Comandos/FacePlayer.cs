using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public class FacePlayer:BloqueFuncion
    {
        public FacePlayer()
            : base("faceplayer")
        { }
        public override byte[] LineaEjecucionToBytes()
        {
            return new byte[] { 0x5A };
        }
    }
}
