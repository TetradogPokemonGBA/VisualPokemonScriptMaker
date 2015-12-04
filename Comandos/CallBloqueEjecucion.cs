using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public class CallBloqueEjecucion:Bloque
    {
        BloqueEjecucion bloqueEjecucion;
        public const byte CALL = 0x04;
        string bloqueALlamar;
       public CallBloqueEjecucion(string idCompilado):base("")
       {

       }
       public CallBloqueEjecucion(BloqueEjecucion bloqueEjecucion)
      :base("")
       {
           BloqueEjecucion = bloqueEjecucion;
       }
       /// <summary>
       /// se necesita el 0x o el @ :D
       /// </summary>
       public string IdCompilado
       {
           get { return bloqueALlamar; }
           set { bloqueALlamar = value; }
       }
       /// <summary>
       /// Si no tiene ninguno asociado devuelve null.
       /// </summary>
       public BloqueEjecucion BloqueEjecucion
        {
            get { return bloqueEjecucion; }
            set {
                if (bloqueEjecucion != null)
                    bloqueEjecucion.CambioDireccion -= CambioDireccionBloqueEjecucion;
                bloqueEjecucion = value;
                bloqueEjecucion.CambioDireccion += CambioDireccionBloqueEjecucion;
            if (bloqueEjecucion != null)
            {
              
               IdCompilado = "@"+bloqueEjecucion.IdBloque;
            }
            else
                IdCompilado = "";
            }
        }

       private void CambioDireccionBloqueEjecucion(BloqueCompilable bloque)
       {
           BloqueEjecucion = bloque as BloqueEjecucion;
       }
       public override string Explicacion()
       {
           return "Ejecuta un bloque.";
       }
       public override string Ejemplo()
       {
           return "call @MetodoAEjecutar o call 0xFuncionCompilada";
       }
       public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
       {
           //sustituyo el puntero esos bytes
           byte[] bytesPuntero;
           if (bloqueEjecucion != null)
               bytesPuntero = PointerToBytes(this.BloqueEjecucion.Compilar(rom).Key);
           else
               bytesPuntero = PointerToBytes(IdBloque.Split('x')[1]);

           return new byte[] { CALL, bytesPuntero[0], bytesPuntero[1], bytesPuntero[2], bytesPuntero[3] }; ;
       }

       public override string LineaEjecucion()
       {
           return "call "+this.bloqueALlamar;
       }
    }
}
