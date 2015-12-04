﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker.Comandos
{
    public class ClearFlag : BloqueFuncion
    {
        public ClearFlag(string flag)
            : base("clearFlag", new string[] { "" })
        {
            Flag = flag;
        }

        public string Flag
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
            byte[] word = WordToBytes(Flag);
            return new byte[] { 0x2A, word[0], word[1] };
        }
    }
}
