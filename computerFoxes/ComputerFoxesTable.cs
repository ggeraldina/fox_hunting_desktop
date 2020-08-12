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

        public ComputerFoxesTable()
        {
            CreateEmptyTable();
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
        public int[] DoShot()
        {

            int maxChance = FindMaximumChance();
            List<CellTable> cellsMaxChance = CreateListWithMaximumChance(maxChance);
            
            Random rand = new Random();
            int numCell = rand.Next(0, cellsMaxChance.Count);
            int row = cellsMaxChance[numCell].Digit;
            int column = cellsMaxChance[numCell].Letter;
            
            tableFoxes[row][column].Shot = true;
            tableFoxes[row][column].PossibleShot = false;

            DoAccordingToThisShot(row, column);
            
            int[] cell = { row, column };
            return cell;
        }

        private void DoAccordingToThisShot(int row, int column)
        {
            int valueCell = (int)(TableFoxes[row][column].Value);
            int valueCellDigit = valueCell - (int)('0');
            if (valueCell != (int)('f') && valueCell != (int)('0'))
            {                
                CountCellValues(row, column, "shot", valueCellDigit);
            }
            CountDeadFoxesAround = 0;
            CountCellValues(row, column, "shotUpdate");
            int differenceValues = valueCellDigit - CountDeadFoxesAround;
            if (differenceValues == 0)
            {
                CountCellValues(row, column, "shot", 0);
            }
            if (valueCell == (int)('f') )
            {
                CountDeadFoxes++;
                BypassNeighboringCells(row, column, "shot");
                UpdatePossibleShots();
            }
        }
        private void UpdatePossibleShots()
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
                        CountCellValues(cell.Digit, cell.Letter, "shotUpdate");
                        int differenceValues = valueCellDigit - CountDeadFoxesAround; 
                        if (differenceValues == 0)
                        {
                            CountCellValues(cell.Digit, cell.Letter, "shot", 0);
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
        
    }
}
