using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxesTable
{
    public class ComputerFoxesTable : FoxesTable
    {
        // FoxesTable field:
        //protected List<CellTable> foxes = new List<CellTable>();
        //protected List<List<CellTable>> tableFoxes = new List<List<CellTable>>();
        //protected static int numberFox = 5;
        //protected int countDeadFoxes = 0;
        //
        //public List<List<CellTable>> TableFoxes { get; }
        //public int CountDeadFoxes { get; set; }
        //public int CountDeadFoxesAround { get; set; }
        //public int NumberFox { get; set; }

        public ComputerFoxesTable()
        {
            CreateEmptyTable();
        }
        public ComputerFoxesTable(int numFox)
        {
            CreateEmptyTable();
            this.NumberFox = numFox;
        }
        public bool AddFox(int rowFox, int columnFox)
        {
            bool flagCanNotBeLocated = BypassNeighboringCells(rowFox, columnFox, "flagCanNotBeLocated");
            if (flagCanNotBeLocated)
            {
                return false;
            }
            CellTable fox = new CellTable(rowFox, columnFox, 'f');
            foxes.Add(fox);
            char valueFox = fox.Value;
            this.TableFoxes[rowFox][columnFox].Value = valueFox;
            CountCellValues(rowFox, columnFox, "fox");
            return true;
        }
        public int[] DoShot(string currentTypeGame)
        {
            List<CellTable> cellsChance = new List<CellTable>();            
            Random randShot = new Random();
            int randNumber = randShot.Next(1, 100);
            int chanseSmartShot = 1;
            bool currentSmartShot = false;
            switch(currentTypeGame){
                case "Против компьютера (сложный уровень)":
                    chanseSmartShot = 9;
                    //  probability smart shot= 1 - 1/chanseSmartShot 
                    if (randNumber % chanseSmartShot != 0)
                    {
                        currentSmartShot = true;
                    }
                    break;
                case "Против компьютера (средний уровень)":
                    chanseSmartShot = 5;
                    //  probability smart shot= 1 - 1/chanseSmartShot 
                    if (randNumber % chanseSmartShot != 0)
                    {
                        currentSmartShot = true;
                    }
                    break;
                case "Против компьютера (простой уровень)":
                    chanseSmartShot = 9;
                    //  probability smart shot= 1/chanseSmartShot 
                    if (randNumber % chanseSmartShot == 0)
                    {
                        currentSmartShot = true;
                    }
                    break;
            }
            //  probability smart shot= 1 - 1/chanseSmartShot 
            
            

            if (currentSmartShot)
            {
                // smart shot
                int maxChance = FindMaximumChance();
                cellsChance = CreateListWithMaximumChance(maxChance);
            }
            else
            {
                cellsChance = CreateListWithAnyChance();
            }

            Random randCell = new Random();
            int numCell = randCell.Next(0, cellsChance.Count);
            int row = cellsChance[numCell].Digit;
            int column = cellsChance[numCell].Letter;
            
            tableFoxes[row][column].Shot = true;
            tableFoxes[row][column].PossibleShot = false;

            DoAccordingToThisShot(row, column);
            
            int[] cell = { row, column };
            return cell;
        }

        private void DoAccordingToThisShot(int row, int column)
        {
            //f, 0, ..., 8
            int valueCell = (int)(TableFoxes[row][column].Value);
            //integer 0, ..., 8 or ((int)'f'-'0')
            int valueCellDigit = valueCell - (int)('0');
            // add chanse for cells
            if (valueCell != (int)('f') && valueCell != (int)('0'))
            {                
                CountCellValues(row, column, "shot", valueCellDigit);
            }
            CountDeadFoxesAround = 0;
            // update CountDeadFoxesAround
            CountCellValues(row, column, "shotUpdate");
            if (valueCell != (int)('f') && NumberFox - CountDeadFoxesAround == valueCellDigit)
            {
                // add a big chance 
                CountCellValues(row, column, "shot", 100);
            }
            int differenceValues = valueCellDigit - CountDeadFoxesAround;
            if (differenceValues == 0)
            {
                // no chance
                CountCellValues(row, column, "shot", 0);
            }            
            if (valueCell == (int)('f') )
            {
                CountDeadFoxes++;
                BypassNeighboringCells(row, column, "shot");
                UpdatePossibleShotsBigChance();
                UpdatePossibleShotsNoChance();
            }
        }
        private void UpdatePossibleShotsNoChance()
        {
            foreach (List<CellTable> rowTable in tableFoxes)
            {
                foreach (CellTable cell in rowTable)
                {
                    if (cell.Value != (int)('f') && cell.Shot)
                    {
                        int valueCell = cell.Value;
                        int valueCellDigit = valueCell - (int)('0');
                        CountDeadFoxesAround = 0;
                        // update CountDeadFoxesAround
                        CountCellValues(cell.Digit, cell.Letter, "shotUpdate");
                        int differenceValues = valueCellDigit - CountDeadFoxesAround; 
                        if (differenceValues == 0)
                        {
                            // no chance
                            CountCellValues(cell.Digit, cell.Letter, "shot", 0);
                        }
                    }
                }
            }
        }
        private void UpdatePossibleShotsBigChance()
        {
            foreach (List<CellTable> rowTable in tableFoxes)
            {
                foreach (CellTable cell in rowTable)
                {
                    if (cell.Value != (int)('f') && cell.Shot)
                    {
                        int valueCell = cell.Value;
                        int valueCellDigit = valueCell - (int)('0');
                        CountDeadFoxesAround = 0;
                        // update CountDeadFoxesAround
                        CountCellValues(cell.Digit, cell.Letter, "shotUpdate");
                        if (valueCell != (int)('f') && NumberFox - CountDeadFoxesAround == valueCellDigit)
                        {
                            // add a big chance 
                            CountCellValues(cell.Digit, cell.Letter, "shot", 100);
                        }                        
                    }
                }
            }
        }
        private int FindMaximumChance()
        {
            int maxChance = 0;
            foreach (List<CellTable> rowTable in tableFoxes)
            {
                foreach (CellTable cell in rowTable)
                {
                    if(cell.Chance > maxChance && cell.PossibleShot)
                    {
                        maxChance = cell.Chance;
                    }
                }
            }
            return maxChance;
        }
        private List<CellTable> CreateListWithMaximumChance(int maxChance)
        {
            List<CellTable> cellsMaxChance = new List<CellTable>();
            foreach (List<CellTable> rowTable in tableFoxes)
            {
                foreach (CellTable cell in rowTable)
                {
                    if (cell.Chance == maxChance && cell.PossibleShot)
                    {
                        cellsMaxChance.Add(cell);
                    }
                }
            }
            return cellsMaxChance;
        }
        private List<CellTable> CreateListWithAnyChance()
        {
            List<CellTable> cellsAnyChance = new List<CellTable>();
            foreach (List<CellTable> rowTable in tableFoxes)
            {
                foreach (CellTable cell in rowTable)
                {
                    if (!cell.Shot)
                    {
                        cellsAnyChance.Add(cell);
                    }
                }
            }
            return cellsAnyChance;
        }
        
    }
}
