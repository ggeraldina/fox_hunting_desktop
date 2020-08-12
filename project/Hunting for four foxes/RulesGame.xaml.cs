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
using System.Windows.Shapes;
using System.IO;

namespace huntingFoxes
{
    /// <summary>
    /// Логика взаимодействия для RulesGame.xaml
    /// </summary>
    public partial class RulesGame : Window
    {
        public RulesGame()
        {
            InitializeComponent();
            this.textBlockRulesGame.Text = "";
            try
            {
                string fileWithRulesGame = (new Uri(Directory.GetCurrentDirectory() + "/rules of the game.txt")).OriginalString;
                Encoding enc = Encoding.GetEncoding(1251);
                string[] linesRulesGame = File.ReadAllLines(fileWithRulesGame, enc);
                foreach (string lrg in linesRulesGame)
                {
                    this.textBlockRulesGame.Text += lrg;
                    this.textBlockRulesGame.Text += "\n";
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
