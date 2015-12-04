using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker.Comandos
{
   public class TrainerBattle:BloqueFuncion
    {
       public enum TipoBatallaEntrenador { 
       Normal=0,
       Dobles=4
       }
        BloqueTexto inicioCombate;

       BloqueTexto finCombate;


       public TrainerBattle(TipoBatallaEntrenador kindOfBattle, string trainerNumber, BloqueTexto inicioCombate, BloqueTexto finCombate)
           : base("trainerBattle", new string[] { "", "", "0x0", "", "" })
       { 
            KindOfBattle=kindOfBattle;
            BattleToStart = trainerNumber;
            InicioCombate = inicioCombate;
            FinCombate = finCombate;
            inicioCombate.IdCambiado += InicioCambiado;
            finCombate.IdCambiado += FinCambiado;
       }

       private void InicioCambiado(Bloque bloqueCambiado)
       {
           InicioCombate = bloqueCambiado as BloqueTexto;
       }
       private void FinCambiado(Bloque bloqueCambiado)
       {
           FinCombate = bloqueCambiado as BloqueTexto;
       }
       /// <summary>
       /// trainer number
       /// </summary>
       public string BattleToStart
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

       public TipoBatallaEntrenador KindOfBattle
       {
           get { return (TipoBatallaEntrenador)Convert.ToInt32(Args[1].Split('x')[1]); }
           set
           {
              Args[1] = "0x" + ((int)value);
           }
       }
       public BloqueTexto InicioCombate
       {
           get { return inicioCombate; }
           set {
               if (inicioCombate != null)
               {
                   inicioCombate.CambioDireccion -= CambioDireccionInicioCombate;

               } 

               inicioCombate = value;
               inicioCombate.CambioDireccion += CambioDireccionInicioCombate;
               Args[3] = "0x"+inicioCombate.DireccionDeclaracion;
                }
       }

       private void CambioDireccionInicioCombate(BloqueCompilable bloque)
       {
           InicioCombate = bloque as BloqueTexto;
       }
       private void CambioDireccionFinCombate(BloqueCompilable bloque)
       {
          FinCombate = bloque as BloqueTexto;
       }
       public BloqueTexto FinCombate
       {
           get { return finCombate; }
           set {
               if (finCombate != null)
               {
                   finCombate.CambioDireccion -= CambioDireccionFinCombate;

               } 

               finCombate = value; 
               finCombate.CambioDireccion += CambioDireccionFinCombate;
               Args[4] = "0x" + finCombate.DireccionDeclaracion;
}
       }
       public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
       {
           byte[] wordBattleStart = WordToBytes(BattleToStart);
           byte wordReserved =  0x0;
           byte[] challengeText = PointerToBytes(InicioCombate.Compilar(rom).Key);
           byte[] defeatText = PointerToBytes(FinCombate.Compilar(rom).Key);
           return new byte[] { 0x5C, (byte)KindOfBattle, wordBattleStart[0], wordBattleStart[1], wordReserved,wordReserved,/*mirar si esta bien*/ challengeText[0], challengeText[1], challengeText[2], challengeText[3], defeatText[0], defeatText[1], defeatText[2], defeatText[3] };

       }

       public override byte[] LineaEjecucionToBytes()
       {
           throw new NotImplementedException();
       }
    }
}
