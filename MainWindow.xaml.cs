using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScriptPokemonMaker
{
    /// <summary>
    /// Es un programa que permite desarroyar scrips para pokemon gba(de momento) y exporta para XSE (de momento)
    /// La idea es hacerlo visual y facil de entender :D
    /// poner ejemplos de uso y modo ayuda
    /// poner simulador en un futuro para no tener que " encender la gba"
    /// permite administrar los flags (asi es mas facil)
    /// permite crear bloques de ejecucion o acciones(igual pero en vez de end tiene un return)[se pueden ejecutar desde otros metodos][mirar si el de ejecucion se puede hacer o acaba con el end que tiene...]
    /// permite crear bloques de texto [y poder ver como quedara]
    /// permite crear bloques de movimiento
    /// se guarda todo en un xml y se puede exportar para XSE
    /// todo tiene su icono representativo,su configurador y en el futuro su simulador
    /// en el futuro abra un simulador de todo el script que ejecutara las acciones...mas adelante...
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
             
        }
    }
}
