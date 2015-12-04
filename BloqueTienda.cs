using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.GBA;
namespace ScriptPokemonMaker
{
    public class BloqueTienda : BloqueList
    {
        public const byte WORD = 0x0;
        public const byte POKEMART = 0x86;
        public BloqueTienda(string idBloque)
            : base(idBloque, "#raw word ")
        {
        }
        public override string LineaEjecucion()
        {
            return "pokemart @" + IdBloque;
        }


        public override byte[] LineaEjecucionToBytes(RomPokemon rom)
        {
            //ahora editar sera desde la clase...si editan se quita el puntero...y se obtine al compilarlo :D
            return new byte[] { POKEMART, pointerBytes[0], pointerBytes[1], pointerBytes[2], pointerBytes[3] };
        }
        public override byte[] DeclaracionToBytes(RomPokemon rom)
        {
            byte[] bytesHaTratar= base.DeclaracionToBytes(rom);
            byte[] bytesTratados = new byte[bytesHaTratar.Length * 2];
            for (int i = 0,j=1; i < bytesTratados.Length; i+=2,j+=2)
            {
                bytesTratados[i] = bytesHaTratar[i];
                bytesTratados[j] = WORD;
            }
            return bytesTratados;
        }

    }
}
