using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public class CheckGender:FuncionCheck
    {
        public CheckGender()
            : base("checkGender")
        { }
        public override string Ejemplo()
        {
            return "checkGender";
        }
        public override byte[] LineaEjecucionToBytes()
        {
            return new byte[] { 0xA0 };
        }
    }
}
