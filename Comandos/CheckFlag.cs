using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public class CheckFlag:FuncionCheck
    {
       public CheckFlag(string flag) : base("checkFlag", new string[] { "0x" + flag }) { }
       public string FlagToCheck
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
           byte[] flag=WordToBytes(FlagToCheck);
           return new byte[] { 0x2B, flag[0], flag[1] };
       }

    }
}
