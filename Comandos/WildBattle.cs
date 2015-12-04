using Gabriel.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
   public class WildBattle:BloqueFuncion
    {
       public const byte WILDBATTLE = 0x00;
       public WildBattle(string idPokemon, string nivel, string idObjetoEquipado)
           : base("wildbattle", new string[] { "","","" })
       {
           IdPokemon = idPokemon;
           Nivel = nivel;
           ItemPokemon = idObjetoEquipado;
       }
       public WildBattle(string idPokemon, string nivel)
           : this(idPokemon, nivel, "0")
       { }
       public string IdPokemon
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
       /// <summary>
       /// Esta en Hexadecimal
       /// </summary>
       public string Nivel
       {
           get { return Args[1].Split('x')[1]; }
           set
           {
               if (value.Contains('x'))
               {

                   Bloque.CompruebaIdHex(value);

                   Args[1] = value;
               }
               else
                   Args[1] = "0x" + value;
           }
       }
       public string ItemPokemon
       {
           get { return Args[2].Split('x')[1]; }
           set
           {
               if (value.Contains('x'))
               {

                   Bloque.CompruebaIdHex(value);

                   Args[2] = value;
               }
               else
                   Args[2] = "0x" + value;
           }
       }
       public override byte[] LineaEjecucionToBytes()
       {
           byte[] bEspeciePokemon=WordToBytes(IdPokemon), bItem=WordToBytes(ItemPokemon);
           byte bNivel=Convert.ToByte(Nivel);
           return new byte[] { WILDBATTLE, bEspeciePokemon[0], bEspeciePokemon[1], bNivel, bItem[0], bItem[1] };
       }
    }
}
