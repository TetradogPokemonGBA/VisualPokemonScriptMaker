using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
  public  class SetObedience:BloqueFuncion
    {
      public SetObedience(string pokemonParty)
          : base("setObedience", new string[] { "" })
      {
          PokemonParty = pokemonParty; 
      }

      public string PokemonParty
      {
          get { return Args[0].Split('x')[1]; }
          set
          {
              try
              {
                  int num = -1;
                  if (value.Contains('x'))
                  {
                      num = Convert.ToInt32(value.Split('x')[1]);
                      if (num < 1 && num > 6)
                          throw new Exception("El numero se sale del rango del equipo pokemon 1-6");
                      Bloque.CompruebaIdHex(value);

                      Args[0] = value;
                  }
                  else
                  {
                      num = Convert.ToInt32(value);
                      if (num < 1 && num > 6)
                          throw new Exception("El numero se sale del rango del equipo pokemon 1-6");
                      Args[0] = "0x" + value;
                  }
              }
              catch {
                  throw new Exception("El valor no es un numero hexagesimal");
              }
          }
      }
      public override byte[] LineaEjecucionToBytes()
      {
          byte[] word = WordToBytes(PokemonParty);
          return new byte[] { 0xCD, word[0], word[1] };
      }
    }
}
