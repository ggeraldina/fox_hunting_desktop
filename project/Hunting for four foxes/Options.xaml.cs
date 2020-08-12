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
using System.Windows.Forms;
using System.IO;
using foxesTable;
namespace huntingFoxes
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private Grid gridBase;
        private MainWindow mainWindow;
        public Options()
        {
            InitializeComponent();
        }
        public Options(MainWindow mainWindow)
        {
            InitializeComponent();
            this.gridBase = mainWindow.gridBase;
            this.mainWindow = mainWindow;
            UpdateComboBoxTypeGame();
            UpdateComboBoxSelectQuantity();
        }
        private void buttonSelectBackground_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog.ShowDialog();
            string selectedFile = openFileDialog.FileName;
            mainWindow.CurentBackgroundPath = selectedFile;
            if (!selectedFile.Equals(""))
            {
                gridBase.Background = new ImageBrush(new BitmapImage(new Uri(selectedFile)));
            }
        }

        private void buttonOriginalBackground_Click(object sender, RoutedEventArgs e)
        {
            GradientStopCollection gradientStopCollection = new GradientStopCollection();
            gradientStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFE24927"), 0));
            gradientStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFF6B245"), 1));
            gridBase.Background = new LinearGradientBrush(gradientStopCollection, 90.0);
            mainWindow.CurentBackgroundPath = "";
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            string typeGame = comboBoxTypeGame.SelectedItem.ToString();
            string numFoxes = comboBoxSelectQuantity.SelectedItem.ToString();
            bool flagNewGame = true;
            if(typeGame.Equals(mainWindow.CurrentTypeGame) && numFoxes.Equals(mainWindow.CurrentNumFoxes.ToString()))
            {
                flagNewGame = false;
            }
            List<string> option = new List<string>();
            option.Add(typeGame);
            option.Add(numFoxes);
            option.Add(mainWindow.CurentBackgroundPath);
            File.WriteAllLines((Directory.GetCurrentDirectory() + "/option.txt"), option);
            if (flagNewGame)
            {
                mainWindow.CreateNewGame();
            }
            this.DialogResult = true;
        }
        private void UpdateComboBoxTypeGame()
        {
            string[] queryResults = { "На минимальное число ходов", 
                                        "Против компьютера (сложный уровень)", 
                                        "Против компьютера (средний уровень)", 
                                        "Против компьютера (простой уровень)"};
            int currentIndex = 0;
            int count = 0;
            foreach (var item in queryResults)
            {
                comboBoxTypeGame.Items.Add(item);
               if (mainWindow.CurrentTypeGame.Equals(item))
                {
                    currentIndex = count;
                }
                count++;
            }
            comboBoxTypeGame.SelectedIndex = currentIndex;
        }
        private void UpdateComboBoxSelectQuantity(){
            int[] queryResults = {3, 4, 5, 6, 7, 8};
            int currentIndex = 0;
            foreach (var item in queryResults)
            {
                comboBoxSelectQuantity.ItemsSource += item.ToString();
                if (mainWindow.CurrentNumFoxes == item)
                {
                    currentIndex = item - 3;
                }
            }
            comboBoxSelectQuantity.SelectedIndex = currentIndex;
        }

        private void SaveTypeGame_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.CurrentTypeGame = comboBoxTypeGame.SelectedItem.ToString();
            mainWindow.labelTypeGame.Content = "Тип игры: " + mainWindow.CurrentTypeGame;
        }
        private void SaveSelectQuantity_Click(object sender, RoutedEventArgs e)
        {
            int numFoxes = Convert.ToInt32(comboBoxSelectQuantity.SelectedItem.ToString());
            mainWindow.CurrentNumFoxes = numFoxes;
            //mainWindow.labelQuantity.Content = "Число лис на поле = " + numFoxes.ToString();
            //UserFoxesTable tableUserFoxesNew = new UserFoxesTable(numFoxes);
            //ComputerFoxesTable tableCompFoxesNew = new ComputerFoxesTable(numFoxes);
            //mainWindow.TableUserFoxes = tableUserFoxesNew;
            //mainWindow.TableCompFoxes = tableCompFoxesNew;
            //mainWindow.UpdateForNewGame();
           
        }
    }
}
