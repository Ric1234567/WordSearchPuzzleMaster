using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMaster
{
    class CharGrid
    {
        
        private List<string> searchWords = new List<string>();
        private int wordsToSearchCount = 0;
        private int width = 0;
        private int height = 0;
        private char[,] charGrid;

        public CharGrid(int width, int height, int wordsToSearchCount)
        {
            this.Width = width;
            this.Height = height;
            this.initCharGrid();
            this.WordsToSearchCount = wordsToSearchCount;
        }

        public List<string> SearchWords { get => searchWords; set => searchWords = value; }
        public int WordsToSearchCount { get => wordsToSearchCount; set => wordsToSearchCount = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public string GridText
        {
            get
            {
                string s = "";

                //grid
                for (int y = 0; y < this.Height; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        s += getCharGrid()[x, y];
                    }

                    //add new line
                    s += '\n';
                }

                //searchWords
                s += "\n\nGesuchte Wörter: " + String.Join(", ", this.SearchWords.ToArray());
                return s;
            }
        }
        public void setChar(int x, int y, char c)
        {
            this.charGrid[x, y] = c;
        }
        public char[,] getCharGrid()
        {
            return charGrid;
        }

        public void initCharGrid()
        {
            charGrid = new char[this.Width, this.Height];
        }

       
    }
}
