using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptPokemonMaker
{
    public class ChosePath : Bloque
    {
        string[] resultados;
       KeyValuePair< CallBloqueEjecucion,bool>[] idsBloques;
        public ChosePath(ILastResult objResultados)
            : base("")
        {
            CambiaResultados(objResultados);

        }
        public KeyValuePair<CallBloqueEjecucion, bool>[] IdsBloques
        {
            get { return idsBloques; }
            set
            {
                if (value == null)
                    value = new KeyValuePair<CallBloqueEjecucion, bool>[] { };
                for (int i = 0; i < value.Length && i < IdsBloques.Length; i++)
                    idsBloques[i] = value[i];
                for (int i = value.Length; i < IdsBloques.Length; i++)
                    idsBloques[i] = new KeyValuePair< CallBloqueEjecucion,bool>(null,false);
            }
        }
        public string[] Resultados
        {
            get { return resultados; }
            private set { resultados = value; }
        }
        public void CambiaResultados(ILastResult objResultados)
        {
            resultados = objResultados.PosiblesResults();
            idsBloques = new KeyValuePair<CallBloqueEjecucion, bool>[resultados.Length];//si lo cambia lo reseteo??
        }
        /// <summary>
        /// Asinga a una respuesta existente un bloque de ejecucion
        /// </summary>
        /// <param name="resultado">resultado al que se le asignara el bloque si no existe no se omitira y no se modificara nada</param>
        /// <param name="idBloque">bloque para ser ejecutado si se cumple la comparacion LastResult y el Resultado si es null se quita</param>
        /// <param name="acabaEnEnd">si es true se usara el bloque de ejecucion como bloqueEnd sino como bloqueReturn</param>
        public void PonIdAResultado(string resultado, CallBloqueEjecucion idBloque,bool acabaEnEnd)
        {
            int pos = -1;
            for (int i = 0; i < resultados.Length && pos == -1; i++)
                if (resultados[i] == resultado)
                    pos = i;
            if (pos != -1)
            {
                if (idBloque != null)
                    idsBloques[pos] = new KeyValuePair<CallBloqueEjecucion, bool>(idBloque, acabaEnEnd);
                else
                    idsBloques[pos] = default(KeyValuePair<CallBloqueEjecucion, bool>);
            }
        }
        public void QuitarIdAResultado(string resultado)
        {
            PonIdAResultado(resultado, null,false);
        }
        public override string LineaEjecucion()
        {
            string lineaEjecucion = "";
            for (int i = 0; i < resultados.Length; i++)
            {
                if (idsBloques[i].Key!= null)
                {
                    lineaEjecucion += "compare LASTRESULT " + resultados[i]+"\n";
                    if(idsBloques[i].Value)
                        lineaEjecucion += "if 0x1 " + idsBloques[i].Key.BloqueEjecucion.LineaEjecucionEnd() + "\n";
                    else
                        lineaEjecucion += "if 0x1 " + idsBloques[i].Key.BloqueEjecucion.LineaEjecucionReturn() + "\n";
                }
            }
            return lineaEjecucion;

        }
        /// <summary>
        /// Mira si el hilo principal es accesible por algun camino
        /// </summary>
        /// <returns></returns>
        public bool PuedeContinuar()
        {
            bool puedeContinuar = false;
            for (int i = 0; i < this.idsBloques.Length && !puedeContinuar; i++)
                if (idsBloques[i].Key != null && !idsBloques[i].Value)//si no es un bloque end
                    puedeContinuar = true;//puede continuar
            return puedeContinuar;
        }



        public override byte[] LineaEjecucionToBytes(Gabriel.Cat.GBA.RomPokemon rom)
        {

            List<byte> bytesLineaEjecucion = new List<byte>();
            //lo mismo que lo de XSE pero en vez de letras bytes xD
            byte[]  word,callMasPunteroDondeTieneQueIr;

            //comparacion 0x21 LASTRESULT word
            //if call//0x07 0x1 Call+PunteroDondeTieneQueIr
            for (int i = 0; i < resultados.Length; i++)
            {
                if (idsBloques[i].Key != null)
                {
                    word = WordToBytes(resultados[i]);
                    if (idsBloques[i].Value)

                        callMasPunteroDondeTieneQueIr = idsBloques[i].Key.BloqueEjecucion.LineaEjecucionEndToBytes(rom);
                    else
                        callMasPunteroDondeTieneQueIr =idsBloques[i].Key.BloqueEjecucion.LineaEjecucionReturnToBytes(rom);

                    bytesLineaEjecucion.Add(0x21);//compare
                    bytesLineaEjecucion.AddRange(LASTRESULT);//lastResult
                    bytesLineaEjecucion.AddRange(word);//el resultado
                    bytesLineaEjecucion.Add(0x07);//if
                    bytesLineaEjecucion.Add(0x1);//true
                    bytesLineaEjecucion.AddRange(callMasPunteroDondeTieneQueIr);//a donde tiene que ir
                }
            }
            
            return bytesLineaEjecucion.ToArray();
        }


    }
}
