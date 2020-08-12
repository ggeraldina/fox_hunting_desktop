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
using System.Collections;
using System.Windows.Threading;
using System.IO;
using foxesTable;
namespace huntingFoxes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // For user
        private UserFoxesTable tableUserFoxes = new UserFoxesTable();
        // For computer
        private ComputerFoxesTable tableCompFoxes = new ComputerFoxesTable();
        // User table of coordinates of buttons and labels
        private List<CellAndButtonLabel> cellAndButtonLabelTableLeft = new List<CellAndButtonLabel>();
        // Computer table of coordinates of buttons and labels
        private List<CellAndButtonLabel> cellAndButtonLabelTableRight = new List<CellAndButtonLabel>();
        
        public MainWindow()
        {
            InitializeComponent();
            CreateButtonLabelTableLeftRight();
            UpdateLabel("userTable");
            DisableAllButtons("userTable");
        }

        private void UpdateLabel(string flagTable)
        {           
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    char valueCell;
                    switch (flagTable)
                    {
                        case "userTable":
                            valueCell = tableUserFoxes.TableFoxes[i][j].Value;
                            cellAndButtonLabelTableLeft[i * 9 + j].Label.Content = valueCell;
                            cellAndButtonLabelTableLeft[i * 9 + j].Button.Content = "";
                            if (valueCell.Equals('f'))
                            {
                                cellAndButtonLabelTableLeft[i * 9 + j].Label.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/fox.jpg"))); 
                                cellAndButtonLabelTableLeft[i * 9 + j].Label.Content = "";
                            }
                            else
                            {
                                cellAndButtonLabelTableLeft[i * 9 + j].Label.Background = new SolidColorBrush(Colors.Honeydew);
                            }
                            break;
                        case "computerTable":
                            valueCell = tableCompFoxes.TableFoxes[i][j].Value;
                            cellAndButtonLabelTableRight[i * 9 + j].Label.Content = valueCell;
                            if (valueCell.Equals('f'))
                            {
                                cellAndButtonLabelTableRight[i * 9 + j].Label.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/fox.jpg")));
                                cellAndButtonLabelTableRight[i * 9 + j].Label.Content = "";
                            }
                            else
                            {
                                cellAndButtonLabelTableRight[i * 9 + j].Label.Background = new SolidColorBrush(Colors.Honeydew);
                                cellAndButtonLabelTableRight[i * 9 + j].Button.Content = "";
                            }
                            break;
                    }
                }
            }
        }
        private void CommonButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Collapsed;
            int[] cellCoordinates = FindCoordinatesButton(((Button)sender), "left");
            int digit = cellCoordinates[0];
            int letter = cellCoordinates[1];
            if (tableUserFoxes.TableFoxes[digit][letter].Value.Equals('f'))
            {
                tableUserFoxes.CountDeadFoxes++;
                if (tableUserFoxes.CountDeadFoxes == tableUserFoxes.NumberFox)
                {
                    DisableAllButtons("userTable");
                    MessageBox.Show("Вы победили! Поздравляем!");                    
                }
            }
            else
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                EventHandler eh = null;
                bool flag = true;
                eh = (object mySender, EventArgs args) =>
                {
                    if (flag == true)
                    {
                        int[] cell = tableCompFoxes.DoShot();
                        cellAndButtonLabelTableRight[cell[0] * 9 + cell[1]].Button.Visibility = Visibility.Collapsed;
                        if (tableCompFoxes.CountDeadFoxes == tableCompFoxes.NumberFox)
                        {
                            DisableAllButtons("userTable");
                            MessageBox.Show("Вы проиграли!");
                            flag = false;
                        }
                        if (tableCompFoxes.TableFoxes[cell[0]][cell[1]].Value != 'f')
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        timer.Stop();
                    }
                };
                timer.Tick += eh;
                timer.Start();
            }
        }
        private void CommonButtonFox_Click(object sender, RoutedEventArgs e)
        {            
            // Find the coordinates of the cell with this button
            int[] cellCoordinates = FindCoordinatesButton(((Button)sender), "right");
            int digit = cellCoordinates[0];
            int letter = cellCoordinates[1];
            
            // If it is possible to add a this fox, then add
            if (tableCompFoxes.AddFox(digit, letter))
            {
                ((Button)sender).Content = "лиса";
                //((Button)sender).Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/foxBW.jpg"))); 
                UpdateLabel("computerTable");
            }
            else
            {
                char letterCell = (char)(((int)('А')) + letter);
                char digitCell = (char)(((int)('1')) + digit);
                MessageBox.Show("В клетке " + letterCell  + digitCell + " по правилам игры расположить лису уже нельзя");
            }
            if (tableCompFoxes.CountFoxes() == tableCompFoxes.NumberFox)
            {
                DisableAllButtons("computerTable");
                EnableAllButtons("userTable");
            }
        }

        private int[] FindCoordinatesButton(Button button, string flagWhere)
        {
            List<CellAndButtonLabel> cellAndButtonLabelTable = new List<CellAndButtonLabel>();
            switch (flagWhere){
                case "left":
                    cellAndButtonLabelTable = cellAndButtonLabelTableLeft;
                    break;
                case "right":
                    cellAndButtonLabelTable = cellAndButtonLabelTableRight;
                    break;
            }
            IEnumerable<CellAndButtonLabel> cellButton =
                from cell in cellAndButtonLabelTable
                where cell.Button.Equals(button)
                select cell;
            List<CellAndButtonLabel> cellButtonList = cellButton.ToList();
            int digit = cellButtonList[0].Digit;
            int letter = cellButtonList[0].Letter;
            int[] coordinates = { digit, letter };
            return coordinates;
        }
        private void CommonButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (((Button)sender).Content.Equals("X"))
                {
                    ((Button)sender).Content = "";
                }
                else
                {
                    ((Button)sender).Content = "X";
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cellAndButtonLabelTableLeft[i * 9 + j].Button.Visibility = Visibility.Collapsed;
                    cellAndButtonLabelTableRight[i * 9 + j].Button.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void DisableAllButtons(string flagTable)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    switch (flagTable) { 
                        case "userTable":
                            cellAndButtonLabelTableLeft[i * 9 + j].Button.IsEnabled = false;
                            break;
                        case "computerTable":
                            cellAndButtonLabelTableRight[i * 9 + j].Button.IsEnabled = false;
                            break;
                    }
                }
            }
        }
        private void EnableAllButtons(string flagTable)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    switch (flagTable)
                    {
                        case "userTable":
                            cellAndButtonLabelTableLeft[i * 9 + j].Button.IsEnabled = true;
                            break;
                        case "computerTable":
                            cellAndButtonLabelTableRight[i * 9 + j].Button.IsEnabled = true;
                            break;
                    }
                }
            }
        }
        private void VisibleAllAllButtons()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cellAndButtonLabelTableLeft[i * 9 + j].Button.Visibility = Visibility.Visible;
                    cellAndButtonLabelTableRight[i * 9 + j].Button.Visibility = Visibility.Visible;
                }
            }
        }
        private void CreateButtonLabelTableLeftRight()
        {
            Label[][] labelsLeft = { new Label[] { a1, b1, c1, d1, e1, f1, g1, h1, i1 }, 
                                    new Label[] { a2, b2, c2, d2, e2, f2, g2, h2, i2 },
                                    new Label[] { a3, b3, c3, d3, e3, f3, g3, h3, i3 }, 
                                    new Label[] { a4, b4, c4, d4, e4, f4, g4, h4, i4 }, 
                                    new Label[] { a5, b5, c5, d5, e5, f5, g5, h5, i5 }, 
                                    new Label[] { a6, b6, c6, d6, e6, f6, g6, h6, i6 }, 
                                    new Label[] { a7, b7, c7, d7, e7, f7, g7, h7, i7 }, 
                                    new Label[] { a8, b8, c8, d8, e8, f8, g8, h8, i8 }, 
                                    new Label[] { a9, b9, c9, d9, e9, f9, g9, h9, i9 },};
            Button[][] buttonsLeft = { new Button[] { a1Button, b1Button, c1Button, d1Button, e1Button, f1Button, g1Button, h1Button, i1Button }, 
                                    new Button[] { a2Button, b2Button, c2Button, d2Button, e2Button, f2Button, g2Button, h2Button, i2Button },
                                    new Button[] { a3Button, b3Button, c3Button, d3Button, e3Button, f3Button, g3Button, h3Button, i3Button }, 
                                    new Button[] { a4Button, b4Button, c4Button, d4Button, e4Button, f4Button, g4Button, h4Button, i4Button }, 
                                    new Button[] { a5Button, b5Button, c5Button, d5Button, e5Button, f5Button, g5Button, h5Button, i5Button }, 
                                    new Button[] { a6Button, b6Button, c6Button, d6Button, e6Button, f6Button, g6Button, h6Button, i6Button }, 
                                    new Button[] { a7Button, b7Button, c7Button, d7Button, e7Button, f7Button, g7Button, h7Button, i7Button }, 
                                    new Button[] { a8Button, b8Button, c8Button, d8Button, e8Button, f8Button, g8Button, h8Button, i8Button }, 
                                    new Button[] { a9Button, b9Button, c9Button, d9Button, e9Button, f9Button, g9Button, h9Button, i9Button },};
            Label[][] labelsRight = { new Label[] { a1Comp, b1Comp, c1Comp, d1Comp, e1Comp, f1Comp, g1Comp, h1Comp, i1Comp }, 
                                    new Label[] { a2Comp, b2Comp, c2Comp, d2Comp, e2Comp, f2Comp, g2Comp, h2Comp, i2Comp },
                                    new Label[] { a3Comp, b3Comp, c3Comp, d3Comp, e3Comp, f3Comp, g3Comp, h3Comp, i3Comp }, 
                                    new Label[] { a4Comp, b4Comp, c4Comp, d4Comp, e4Comp, f4Comp, g4Comp, h4Comp, i4Comp }, 
                                    new Label[] { a5Comp, b5Comp, c5Comp, d5Comp, e5Comp, f5Comp, g5Comp, h5Comp, i5Comp }, 
                                    new Label[] { a6Comp, b6Comp, c6Comp, d6Comp, e6Comp, f6Comp, g6Comp, h6Comp, i6Comp }, 
                                    new Label[] { a7Comp, b7Comp, c7Comp, d7Comp, e7Comp, f7Comp, g7Comp, h7Comp, i7Comp }, 
                                    new Label[] { a8Comp, b8Comp, c8Comp, d8Comp, e8Comp, f8Comp, g8Comp, h8Comp, i8Comp }, 
                                    new Label[] { a9Comp, b9Comp, c9Comp, d9Comp, e9Comp, f9Comp, g9Comp, h9Comp, i9Comp },};
            Button[][] buttonsRight = { new Button[] { a1ButtonComp, b1ButtonComp, c1ButtonComp, d1ButtonComp, e1ButtonComp, f1ButtonComp, g1ButtonComp, h1ButtonComp, i1ButtonComp }, 
                                    new Button[] { a2ButtonComp, b2ButtonComp, c2ButtonComp, d2ButtonComp, e2ButtonComp, f2ButtonComp, g2ButtonComp, h2ButtonComp, i2ButtonComp },
                                    new Button[] { a3ButtonComp, b3ButtonComp, c3ButtonComp, d3ButtonComp, e3ButtonComp, f3ButtonComp, g3ButtonComp, h3ButtonComp, i3ButtonComp }, 
                                    new Button[] { a4ButtonComp, b4ButtonComp, c4ButtonComp, d4ButtonComp, e4ButtonComp, f4ButtonComp, g4ButtonComp, h4ButtonComp, i4ButtonComp }, 
                                    new Button[] { a5ButtonComp, b5ButtonComp, c5ButtonComp, d5ButtonComp, e5ButtonComp, f5ButtonComp, g5ButtonComp, h5ButtonComp, i5ButtonComp }, 
                                    new Button[] { a6ButtonComp, b6ButtonComp, c6ButtonComp, d6ButtonComp, e6ButtonComp, f6ButtonComp, g6ButtonComp, h6ButtonComp, i6ButtonComp }, 
                                    new Button[] { a7ButtonComp, b7ButtonComp, c7ButtonComp, d7ButtonComp, e7ButtonComp, f7ButtonComp, g7ButtonComp, h7ButtonComp, i7ButtonComp }, 
                                    new Button[] { a8ButtonComp, b8ButtonComp, c8ButtonComp, d8ButtonComp, e8ButtonComp, f8ButtonComp, g8ButtonComp, h8ButtonComp, i8ButtonComp }, 
                                    new Button[] { a9ButtonComp, b9ButtonComp, c9ButtonComp, d9ButtonComp, e9ButtonComp, f9ButtonComp, g9ButtonComp, h9ButtonComp, i9ButtonComp },};

            for (int i = 0; i < 9; i++)
            {
                int digit = i;
                for (int j = 0; j < 9; j++)
                {
                    int letter = j;
                    CellAndButtonLabel cellTableLeft = new CellAndButtonLabel(buttonsLeft[i][j], labelsLeft[i][j], digit, letter);
                    CellAndButtonLabel cellTableRight = new CellAndButtonLabel(buttonsRight[i][j], labelsRight[i][j], digit, letter);
                    cellAndButtonLabelTableLeft.Add(cellTableLeft);
                    cellAndButtonLabelTableRight.Add(cellTableRight);
                }
                
            }
        }

        private void menuItemNewGame_Click(object sender, RoutedEventArgs e)
        {          
            tableUserFoxes = null;
            tableCompFoxes = null;
            UserFoxesTable tableUserFoxesNew = new UserFoxesTable();
            ComputerFoxesTable tableCompFoxesNew = new ComputerFoxesTable();
            tableUserFoxes = tableUserFoxesNew;
            tableCompFoxes = tableCompFoxesNew;
            UpdateLabel("userTable");
            UpdateLabel("computerTable");
            VisibleAllAllButtons();
            DisableAllButtons("userTable");
            EnableAllButtons("computerTable");
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutProgram windowAboutProgram = new AboutProgram();
            windowAboutProgram.ShowDialog();
        }
    }
}
