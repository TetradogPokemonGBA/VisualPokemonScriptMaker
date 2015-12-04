using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public abstract class FuncionCheck:BloqueFuncion,ILastResult
    {
       public FuncionCheck(string funcion, string[] args) : base(funcion, args) { }
       public FuncionCheck(string funcion) : base(funcion) { }
       public string[] PosiblesResults()
       {
           return new string[] { "0x0", "0x1" };
       }
       public override string Explicacion()
       {
           return "Guarda en LASTRESULT el resultado de la comprovacion";
       }
    }
}
