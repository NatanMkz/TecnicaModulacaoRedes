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

namespace TecnicaModulacaoRedes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string OrigemMensagem = "";

        public MainWindow()
        {
            InitializeComponent();
            main();
        }

        private void main()
        {
            try
            {
                string input = "Hello, world!";
                //Console.WriteLine(StringToBinary(input));
                //int binaryInput = Convert.ToInt32("1001000", 2);
                //Console.WriteLine(BinaryToString(binaryInput));

                BinaryFrequencyShiftKeying bfsk = new BinaryFrequencyShiftKeying(10000, 0, 1300, 1700);
                var modulacao = bfsk.Modulate(input);
                string demodulate = bfsk.Demodulate(modulacao);

                txtOrigem.Text = "Mensagem Original: " + input;
                txtDemodulated.Text = "Mensagem de demodulação: " + demodulate.ToString();
                OrigemMensagem = demodulate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
