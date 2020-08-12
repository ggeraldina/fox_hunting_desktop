using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace foxesTable 
{
    public class UserFoxesTable : FoxesTable
    {
        // FoxesTable field:
        //protected List<CellTable> foxes = new List<CellTable>();
        //protected List<List<CellTable>> tableFoxes = new List<List<CellTable>>();
        //protected static int numberFox = 5;
        //protected int countDeadFoxes = 0;
        //
        //public List<List<CellTable>> TableFoxes { get; }
        //public int CountDeadFoxes { get; set; }

        public UserFoxesTable()
        {
            tableFoxes = new List<List<CellTable>>();
            CreateEmptyTable();
            this.CountDeadFoxes = 0;
            AddFoxes();
            CountCellValuesFox();
        }

        public UserFoxesTable(int numFox)
        {
            tableFoxes = new List<List<CellTable>>();
            CreateEmptyTable();
            this.CountDeadFoxes = 0;
            this.NumberFox = numFox;
            AddFoxes();
            CountCellValuesFox();
        }

        private void AddFoxes()
        {
            for (int i = 0; i < numberFox; )
            {
                int[] foxCoordinates = CreateCoordinates();
                CellTable fox = new CellTable(foxCoordinates[0], foxCoordinates[1], 'f');
                int rowFox = fox.Digit;
                int columnFox = fox.Letter;
                bool flagCanNotBeLocated = BypassNeighboringCells(rowFox, columnFox, "flagCanNotBeLocated");
                if (flagCanNotBeLocated)
                {
                   continue;
                }   
                foxes.Add(fox);
                char valueFox = fox.Value;
                this.TableFoxes[rowFox][columnFox].Value = valueFox;
                i++;
            }
        }



        

        
    }

   
}
