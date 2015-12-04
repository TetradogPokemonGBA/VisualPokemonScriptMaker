using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker.Comandos
{
    public class SetTrainerFlag : BloqueFuncion
    {
        public SetTrainerFlag(string trainerFlag)
            : base("setTrainerFlag", new string[] { "" })
        {
            TrainerFlag = trainerFlag;
        }

        public string TrainerFlag
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
        public override byte[] LineaEjecucionToBytes()
        {
            byte[] word = WordToBytes(TrainerFlag);
            return new byte[] { 0x62, word[0], word[1] };
        }
    }
}
