using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace huntingFoxes
{
    class CellAndButtonLabel
    {
        public Button Button
        {
            get;
            set;
        }
        public Label Label
        {
            get;
            set;
        }
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
        public CellAndButtonLabel(Button button, Label label, 
            int digit, int letter)
        {
            Button = button;
            Label = label;
            Digit = digit;
            Letter = letter;
        }
    }
}
