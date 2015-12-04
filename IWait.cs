using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    /*
       warp 0xB 0x2 0xFF 0x2 0x2
       waitstate //sirve para mas de uno...crear otra interficie?? IWaitState?? y sera IWait???   
     */
  public  interface IWait
    {
      /// <summary>
      /// puede devolver null si no se puede generar el bloque opuesto completamente
      /// </summary>
        Bloque BloqueWait { get; }
        bool SonPareja(IWait bloquePareja);
    }
}
