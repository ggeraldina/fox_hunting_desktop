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
            buttonSave_Click(sender, e);
            this.DialogResult = true;
        }

        private void UpdateComboBoxSelectQuantity(){
            QuantityFoxDataContext quantityFoxDataContext = new QuantityFoxDataContext();
            var queryResults = 
                from q in quantityFoxDataContext.QuantityFoxes
                select q;
            int currentIndex = 0;
            foreach (var item in queryResults)
            {
                comboBoxSelectQuantity.ItemsSource += item.Quantity.ToString();
                if (mainWindow.CurrentNumFoxes == item.Quantity)
                {
                    currentIndex = item.Key - 1;
                }
            }
            comboBoxSelectQuantity.SelectedIndex = currentIndex;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            int numFoxes = Convert.ToInt32(comboBoxSelectQuantity.SelectedItem.ToString());
            mainWindow.CurrentNumFoxes = numFoxes;
            mainWindow.labelQuantity.Content = "Число лис на поле = " + numFoxes.ToString();
            UserFoxesTable tableUserFoxesNew = new UserFoxesTable(numFoxes);
            ComputerFoxesTable tableCompFoxesNew = new ComputerFoxesTable(numFoxes);
            mainWindow.TableUserFoxes = tableUserFoxesNew;
            mainWindow.TableCompFoxes = tableCompFoxesNew;
            mainWindow.UpdateForNewGame();
        }
    }
}
