using Gabriel.Cat.GBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.Extension;
using VisualScripsMaker;
using Gabriel.Cat;
namespace ScriptPokemonMaker
{
    public delegate void CompilarEventHanlder(BloqueCompilable bloque);
    public abstract class BloqueCompilable : Bloque
    {
        #region Atributos
        private RomPokemon romActual;


        int direccionDeclaracion;
        protected byte[] bytesCompilados;
        protected byte[] pointerBytes;//es donde guardo la direccion del objeto compilado
        #endregion
        #region eventos
        public event CompilarEventHanlder CambioDireccion;
        #endregion
        #region Constructor
        public BloqueCompilable(string idBloque) : base(idBloque) { }
        #endregion
        #region Propiedades
        public byte[] BytesCompilados
        {
            get { return bytesCompilados; }
            protected set { bytesCompilados = value; }
        } 
        public RomPokemon RomActual
        {
            get { return romActual; }
            set { romActual = value; }
        }
        public int DireccionDeclaracion
        {
            get
            {
                if (direccionDeclaracion == -1)
                    Compilar();
                return direccionDeclaracion;
            }
            protected set { direccionDeclaracion = value; }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Quita de la rom el bloque compilado
        /// </summary>
        public void LimpiaRom()
        {
            if (direccionDeclaracion != -1 && romActual != null)
            {
                //lo quito
                LimpiaRom(romActual, direccionDeclaracion, bytesCompilados.Length);
                direccionDeclaracion = -1;
                bytesCompilados = new byte[] { };
            }
        }
        /// <summary>
        /// Quita de la rom el bloque compilado
        /// </summary>
        public void LimpiaRom(RomPokemon rom, int direccionDeclaracionCompilada, int longitudBloque)
        {
            //quita de la rom los bytes puestos...la rom??? 
            if (direccionDeclaracionCompilada != -1 && rom != null && longitudBloque > 0)
            {
                rom.ArchivoGbaPokemon = BinarioGBA.SustituyeBytes(rom.ArchivoGbaPokemon, direccionDeclaracionCompilada, longitudBloque);

            }
            else
                throw new ArgumentException("los argumentos son incorrectos");
        }
        /// <summary>
        /// Compila los byes de la declaracion a la rom y sustituye los bytes de la RomActual 
        /// </summary>
        /// <returns></returns>
        public int Compilar()
        {
            //punto de memoria :D jaja
            //pongo
            LimpiaRom();
            KeyValuePair<int, byte[]> romEditada = Compilar(romActual);
            bytesCompilados = romEditada.Value;
            direccionDeclaracion = romEditada.Key;
            return direccionDeclaracion;
        }
        /// <summary>
        /// Compila los byes de la declaracion a la rom y sustituye los bytes de la RomActual 
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, byte[]> Compilar(RomPokemon rom)
        {

            KeyValuePair<int, byte[]> romEditada = BinarioGBA.Edita(rom.ArchivoGbaPokemon, DeclaracionToBytes(rom));
            rom.ArchivoGbaPokemon = romEditada.Value;
            return romEditada;
        }
        public byte[] DeclaracionToBytes()
        {
            return DeclaracionToBytes(romActual);
        }
        #endregion
        #region abstractMetodos
        public abstract string Declaracion();

        public abstract byte[] DeclaracionToBytes(RomPokemon rom);

        #endregion

        public static string Declaracion(IEnumerable<BloqueCompilable> bloques)
        {
            string declaracion = "#dynamic 0x" + InicioBuquedaEspacio + "\n";
            foreach (BloqueCompilable bloque in bloques)
                declaracion += bloque.Declaracion() + "\n";
            return declaracion;

        }
        /// <summary>
        /// Compila los bloques en la rom
        /// </summary>
        /// <param name="bloques"></param>
        /// <param name="romAPonerLosScripts"></param>
        /// <returns>lista de puntero(en decimal)/bloqueAlQuePertenece </returns>
        public static BloqueCompilado[] Compilar(IEnumerable<BloqueCompilable> bloques, RomPokemon romAPonerLosScripts)
        {
            Llista<BloqueCompilado> bloquesCompilados = new Llista<BloqueCompilado>();
            Llista<BloqueEjecucion> bloquesEjecucion = new Llista<BloqueEjecucion>(bloques.Casting<BloqueEjecucion>(false));
            foreach (BloqueCompilable bloque in bloques)
                    bloquesCompilados.Afegir(new BloqueCompilado(romAPonerLosScripts, bloque));
            for (int i = 0; i < bloquesEjecucion.Count; i++)
                bloquesEjecucion[i].DeclaracionToBytes(romAPonerLosScripts, bloquesCompilados);//compilo todas las declaraciones :)
            for (int i = 0,direccion=-1; i < bloquesCompilados.Count; i++)
                 direccion=bloquesCompilados[i].DireccionBloque;//asi compilo los que no lo estan :)
            return bloquesCompilados.ToArray();

        }
    }
}
