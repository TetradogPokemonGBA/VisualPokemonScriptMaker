using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
  public  class Nop:BloqueFuncion
    {
      public Nop() : base("nop") { }
      public override byte[] LineaEjecucionToBytes()
      {
          return new byte[] { 0x00 };
      }
    }
}
