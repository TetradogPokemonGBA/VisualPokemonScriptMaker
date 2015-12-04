using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gabriel.Cat.GBA;
using Gabriel.Cat;
using ScriptPokemonMaker;
namespace VisualScripsMaker
{
  public  class BloqueCompilado:IClauUnicaPerObjecte,IComparable
    {
      //por mirar...
      RomPokemon rom;
      BloqueCompilable bloque;
      int direccionBloque;
      byte[] declaracion;
      byte[] lineaEjecucion;
    

      public BloqueCompilado(RomPokemon rom, BloqueCompilable bloque)
      {
          this.rom = rom;
          this.bloque = bloque;
          this.direccionBloque = -1;
          lineaEjecucion = null;
          declaracion = null;
      } 
      public int DireccionBloque
      {
          get {
              if (direccionBloque == -1)
                  Compilar();
              return direccionBloque; }
      }
      public byte[] Declaracion
      {
          get
          {
              if (declaracion == null)
                  CompilaDeclaracion();
              return declaracion;
          }
          set { declaracion = value; }

      }

      public byte[] LineaEjecucion
      {
          get
          {
              if (lineaEjecucion == null)
                  CompilaLineaEjecucion();
              return lineaEjecucion;
          }
          set { lineaEjecucion = value; }

      }

      public string IdBloque
      { get { return bloque.IdBloque; } }
      public RomPokemon Rom
      {
          get { return rom; }
      }
      public BloqueCompilable Bloque
      { get { return bloque; } }

      private void Compilar()
      {
          direccionBloque = bloque.Compilar(rom).Key;
      }
      private void CompilaLineaEjecucion()
      {
          lineaEjecucion = bloque.LineaEjecucionToBytes(rom);
      }
      private void CompilaDeclaracion()
      {
          declaracion = bloque.DeclaracionToBytes(Rom);
      }
      
      #region Miembros de IClauUnicaPerObjecte

      public IComparable Clau()
      {
          return IdBloque;
      }

      #endregion

      #region Miembros de IComparable

      public int CompareTo(object obj)
      {
          int compareTo=-1;
          try
          {
              BloqueCompilado other = (BloqueCompilado)obj;
              compareTo= IdBloque.CompareTo(other.IdBloque);
          }
          catch { }
          return compareTo;
      }

      #endregion
    }
}
