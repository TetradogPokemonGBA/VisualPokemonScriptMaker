using Gabriel.Cat;
using Gabriel.Cat.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.Extension;
namespace ScriptPokemonMaker
{
    public class BloqueTexto : BloqueCompilable,ILastResult
    {
        public enum MsgBoxType
        {
            Obtain=0,
            Find=1,//se usa para el texto de un objeto del suelo se suele encontrar en giveitem como tercer argumento
            Face=2,
            Sign = 3,//se usa para los SignPost(los scrips ROJOS)...se suelen usar para libros,descripciones de carteles,etc...
            KeepOpen=4,
            YesNo=5,
            Normal=6,
            Pokenav=10,//A..  //solo vale para esmeralda...creo...

        }

        string textoBloque;
        MsgBoxType tipoMsgBox;
        public const byte ULTIMOBYTE = 0xFF;
        public static readonly byte[] MSGBOX = {0xf,0x0, 0x9f};
        public static readonly LlistaOrdenada<string, byte> stringToByte;
        static BloqueTexto()
        {
            stringToByte = new LlistaOrdenada<string, byte>();
            System.IO.StringReader sr = new System.IO.StringReader(VisualScripsMaker.Resource.HexEqualChar);
            string[] campos;
            try
            {
                while (true)//al no tener un EndOfStream...pues hasta que pete
                {
                    campos = sr.ReadLine().Split('=');
                    stringToByte.Afegir(campos[1], Convert.ToByte("0x" + campos[0]));
                }
            }
            catch { }
            sr.Close();

        }
        public BloqueTexto(string id)
            : base(id)
        {
            textoBloque = "";
        }
        public string TextoBloque
        {
            get { return textoBloque; }
            set { textoBloque = value; }
        } 
        public MsgBoxType TipoMsgBox
        {
            get { return tipoMsgBox; }
            set { tipoMsgBox = value; }
        }
        public override string ToString()
        {
            return "#org @" + IdBloque + "\n= " + TextoBloque;
        }

        public override string LineaEjecucion()
        {
            string lineaEjecucion;
            if (tipoMsgBox != MsgBoxType.Pokenav)
                lineaEjecucion = "msgBox @" + IdBloque + " 0x" + ((int)tipoMsgBox);
            else
                lineaEjecucion = "msgBox @" + IdBloque + " 0xA";
            return lineaEjecucion;
        }

        public string[] PosiblesResults()
        {
            string[] posiblesResults = { };
            if(TipoMsgBox==MsgBoxType.YesNo)
            posiblesResults= new string[] { "0x0", "0x1" };
            return posiblesResults;
        }

        public override string Declaracion()
        {
            string declaracion = "Org @" + IdBloque+"\n";
            declaracion += "= " + TextoBloque.Trim();//quito los espacios del principio y del fin????
            return declaracion;
        }

        public override byte[] LineaEjecucionToBytes(RomPokemon rom)
        {

            List<byte> bytes = new List<byte>();
            bytes.AddRange(MSGBOX);
            bytes.AddRange(PointerToBytes(BinarioGBA.Edita(rom.ArchivoGbaPokemon,DeclaracionToBytes()).Key));
            bytes.Add(Convert.ToByte(((Hex)((int)TipoMsgBox)).ToString()));
            return bytes.ToArray();
        }

        public override byte[] DeclaracionToBytes(RomPokemon rom)
        {
            //por mirar ...al menos tiene que funcionar en el FIRE RED...
            List<byte> bytes = new List<byte>();
            bool formandoString = false;
            string stringFormandose = "";
            for (int i = 0; i < TextoBloque.Length; i++)
            {
                if (!formandoString && TextoBloque[i] != '[')
                    bytes.Add(stringToByte[TextoBloque[i] + ""]);
                else
                {
                    formandoString = true;
                    stringFormandose += TextoBloque[i];
                    if (TextoBloque[i] == ']')
                    {
                        if (stringFormandose.ToLower() != "[player]" && stringFormandose.ToLower() != "[rival]")
                           bytes.Add(stringToByte[stringFormandose]);
                        //else pongo lo que toque...

                        formandoString = false;
                        stringFormandose = "";
                    }
                }
            }
            if (bytes[bytes.Count - 1] != ULTIMOBYTE)
                bytes.Add(ULTIMOBYTE);
            return bytes.ToArray();
        }
    }
}
