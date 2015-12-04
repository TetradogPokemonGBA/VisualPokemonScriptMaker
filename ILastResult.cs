using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public  interface ILastResult
    {
        string[] PosiblesResults();//resultados que se escriben en el script
        // string[] TraduccionResultados();//pone lo que significa cada resultado asi se podra mostrar SI en vez de 0x1
    }
}
