using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public class Lock:BloqueFuncion,IWait
    {
       Release release;
      public Lock() : base("lock") { }

      #region Miembros de IWait

      public Bloque BloqueWait
      {
          get { if (release == null)release = new Release(); return release; }
      }

      #endregion
      public override string Explicacion()
      {
          return "Para el movimiento del PLAYER, se requiere de un Release para desbloquearlo";
      }
      public override byte[] LineaEjecucionToBytes()
      {
          return new byte[] { 0x6A };
      }

      #region Miembros de IWait


      public bool SonPareja(IWait bloquePareja)
      {
          return (bloquePareja as BloqueFuncion).Funcion == "release";
      }

      #endregion
    }
   public class LockAll : BloqueFuncion,IWait
   {
       ReleaseAll releaseAll;
       public LockAll() : base("lockAll") { }

       #region Miembros de IWait

       public Bloque BloqueWait
       {
           get { if (releaseAll == null)releaseAll = new ReleaseAll(); return releaseAll; }
       }
       public bool SonPareja(IWait bloquePareja)
       {
           return (bloquePareja as BloqueFuncion).Funcion == "releaseAll";
       }

       #endregion
       public override string Explicacion()
       {
           return "Para el movimiento de todos los personajes de la pantalla, se requiere de un ReleaseAll para desbloquearlos";
       }
       public override byte[] LineaEjecucionToBytes()
       {
           return new byte[] { 0x69 };
       }
   }
}
