using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public class Release :BloqueFuncion,IWait
    {
       Bloque bloqueLock;
       public Release() : base("release") { }
       public override byte[] LineaEjecucionToBytes()
       {
           return new byte[] { 0x6C };
       }

       #region Miembros de IWait

       public Bloque BloqueWait
       {
           get {
               if (bloqueLock == null)
                   bloqueLock = new Lock();
               
               return bloqueLock; }
       }

       public bool SonPareja(IWait bloquePareja)
       {
           return (bloquePareja as BloqueFuncion).Funcion == "lock";
       }

       #endregion
    }
   public class ReleaseAll : BloqueFuncion,IWait
   {
       Bloque bloqueLockAll;
       public ReleaseAll() : base("releaseAll") { }
       public Bloque BloqueWait
       {
           get
           {
               if (bloqueLockAll == null)
                   bloqueLockAll = new LockAll();

               return bloqueLockAll;
           }
       }
       public bool SonPareja(IWait bloquePareja)
       {
           return (bloquePareja as BloqueFuncion).Funcion == "lockAll";
       }
       public override byte[] LineaEjecucionToBytes()
       {
           return new byte[] { 0x6B };
       }
   }
}
