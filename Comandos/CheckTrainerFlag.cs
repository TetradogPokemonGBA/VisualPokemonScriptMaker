using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public  class CheckTrainerFlag:FuncionCheck
    {
       public CheckTrainerFlag(string idEntrenador) : base("checkTrainerFlag", new string[] { "" }) { EntrenadorAComprobar = idEntrenador; }
       public string EntrenadorAComprobar
       {
           get { return Args[0].Split('x')[1]; }
           set {
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
           byte[] word = WordToBytes(EntrenadorAComprobar);
           return new byte[] { 0x60, word[0], word[1] };
       }
    }
}
