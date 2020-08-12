using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foxesTable
{
    public class CellTable
    {
        
        public int Digit // 0-8
        {
            get;
            set;
        }
        public int Letter // 0-8
        {
            get;
            set;
        }
        public char Value // f - fox; number foxes: 0; 1; 2; 3; 4
        {
            get;
            set;
        } 
        public bool Shot // выстрел
        {
            get;
            set;
        }
        public bool PossibleShot 
        {
            get;
            set;
        }
        public int Chance
        {
            get;
            set;
        }
        public CellTable(int digit1, int letter1)
        {
            Digit = digit1;
            Letter = letter1;
            Value = '0';
            Shot = false;
            PossibleShot = true;
            Chance = 0;
        }
        public CellTable(int digit1, int letter1, char value1)
        {
            Digit = digit1;
            Letter = letter1;
            Value = value1;
            Shot = false;
            PossibleShot = true;
            Chance = 0;
        }
    }
}
