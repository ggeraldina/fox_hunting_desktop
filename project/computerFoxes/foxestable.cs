using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxesTable
{
     public abstract class FoxesTable
    {
        protected List<CellTable> foxes = new List<CellTable>();
        protected List<List<CellTable>> tableFoxes = new List<List<CellTable>>();
        protected int countDeadFoxes = 0;
        protected int numberFox = 4;
        protected int countDeadFoxesAround = 0;

        public int CountFoxes()
        {            
           return foxes.Count;
        } 
        public List<List<CellTable>> TableFoxes
        {
            get
            {
                return tableFoxes;
            }
        }
        public int CountDeadFoxes
        {
            get
            {
                return countDeadFoxes;
            }
            set
            {
                countDeadFoxes = value;
            }
        }
        public int CountDeadFoxesAround
        {
            get
            {
                return countDeadFoxesAround;
            }
            set
            {
                countDeadFoxesAround = value;
            }
        }
        public int NumberFox
        {
            get
            {
                return numberFox;
            }
            set
            {
                numberFox = value;
            }
        }

        protected void CreateEmptyTable()
        {
            for (int i = 0; i < 9; i++)
            {
                List<CellTable> tableRow = new List<CellTable>();
                int digit = i;
                for (int j = 0; j < 9; j++)
                {
                    int letter = j;
                    CellTable cellTable = new CellTable(digit, letter);
                    tableRow.Add(cellTable);
                }
                TableFoxes.Add(tableRow);
            }
        }
        // Checking adjacent cells to add new foxes
        protected bool BypassNeighboringCells(int rowFox, int columnFox, string flagWhatIsIt)
        {
            int rowBegin = rowFox - 1;
            int rowEnd = rowFox + 1;
            int columnBegin = columnFox - 1;
            int columnEnd = columnFox + 1;
            for (int row = rowBegin; row <= rowEnd; row++)
            {
                for (int column = columnBegin; column <= columnEnd; column++)
                {
                    if (column >= 0 && row >= 0 && column < 9 && row < 9)
                    {
                        switch (flagWhatIsIt)
                        {
                            case "flagCanNotBeLocated":
                                if (this.TableFoxes[row][column].Value.Equals('f'))
                                {
                                    return true;
                                }
                                break;
                            case "shot":
                                TableFoxes[row][column].PossibleShot = false;
                                break;
                        }
                    }
                }
            }
            return false;
        } 
        protected void CountCellValuesFox()
        {
            for (int i = 0; i < numberFox; i++)
            {
                int rowFox = foxes[i].Digit;
                int columnFox = foxes[i].Letter;
                CountCellValues(rowFox, columnFox, "fox");
            }
        }
        protected void CountCellValues(int row, int column, string flagWhatIsIt, int numFox = 0)
        {
            int leftToRightRow = (row - column >= 0) ? row - column : 0;
            int leftToRightColumn = (row - column < 0) ? column - row : 0;
            int rightToLeftRow = ((row + column) - 8 >= 0) ? (row + column) - 8 : 0;
            int rightToLeftColumn = (8 - (row + column) > 0) ? (row + column) : 8;

            for (int i = leftToRightRow, j = leftToRightColumn; i < 9 && j < 9; )
            {
                DoAccordingToFlag(flagWhatIsIt, i, j, numFox);
                i++;
                j++;
            }
            for (int i = rightToLeftRow, j = rightToLeftColumn; i < 9 && j >= 0; )
            {
                DoAccordingToFlag(flagWhatIsIt, i, j, numFox);
                i++;
                j--;
            }
            for (int i = 0; i < 9; i++)
            {
                DoAccordingToFlag(flagWhatIsIt, i, column, numFox);
            }
            for (int j = 0; j < 9; j++)
            {
                DoAccordingToFlag(flagWhatIsIt, row, j, numFox);
            }
        }

        private void DoAccordingToFlag(string flagWhatIsIt, int i, int j, int numFox)
        {
            switch (flagWhatIsIt)
            {
                case "fox":
                    CountCellValuesForOneFox(i, j);
                    break;
                case "shot":
                    CountCellChanceForOneShot(i, j, numFox);
                    break;
                case "shotUpdate":
                    CountFoundFoxesAround(i, j);
                    break;
            }
        }
        private void CountFoundFoxesAround(int row, int column)
        {
            int valueCell = (int)(TableFoxes[row][column].Value);
            bool shotCell = TableFoxes[row][column].Shot;
            // if it dead fox
            if (valueCell == (int)('f') && shotCell)
            {
                CountDeadFoxesAround++;
            }
        }
        private void CountCellValuesForOneFox(int row, int column)
        {
            int valueCell = (int)(TableFoxes[row][column].Value);
            if (valueCell != (int)('f'))
            {
                TableFoxes[row][column].Value++;
            }
        }
        private void CountCellChanceForOneShot(int row, int column, int numFox)
        {            
            if (TableFoxes[row][column].PossibleShot && numFox > 0)
            {
                TableFoxes[row][column].Chance += numFox;
            }
            else
            {
                if (numFox == 0)
                {
                    TableFoxes[row][column].Chance = 0;
                    TableFoxes[row][column].PossibleShot = false;
                }
            }
        }
        protected int[] CreateCoordinates()
        {
            Random rand = new Random();
            int numDigit = rand.Next(0, 9); // Zero to 8
            int numLetter = rand.Next(0, 9); // Zero to 8            
            int[] cell = { numDigit, numLetter };
            return cell;
        }
    }
}
