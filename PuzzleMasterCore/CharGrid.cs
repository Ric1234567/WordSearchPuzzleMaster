using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMasterCore
{
    public class CharGrid : INotifyPropertyChanged
    {

        private List<string> searchWords = new List<string>();
        private int wordsToSearchCount = 0;
        private int wordsToSearchCountMax = 0;
        private int width = 0;
        private int height = 0;
        private char[,] charGrid;

        public event PropertyChangedEventHandler PropertyChanged;

        public CharGrid(int width, int height, int wordsToSearchCount)
        {
            this.Width = width;
            this.Height = height;
            this.initCharGrid();
            this.WordsToSearchCount = wordsToSearchCount;
        }

        #region Props
        /// <summary>
        /// Selected Words which will be used to create the grid.
        /// </summary>
        public List<string> SearchWords
        {
            get { return searchWords; }
            set
            {
                searchWords = value;
                OnPropertyChanged(nameof(SearchWords));
            }
        }
        public int WordsToSearchCount
        {
            get { return wordsToSearchCount; }
            set
            {
                wordsToSearchCount = value;
                OnPropertyChanged(nameof(WordsToSearchCount));
            }
        }
        public int WordsToSearchCountMax
        {
            get { return wordsToSearchCountMax; }
            set
            {
                wordsToSearchCountMax = value;
                OnPropertyChanged(nameof(WordsToSearchCountMax));
            }
        }
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                initCharGrid();
                OnPropertyChanged(nameof(Width));
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                initCharGrid();
                OnPropertyChanged(nameof(Height));
            }
        }
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
                    s += System.Environment.NewLine;
                }

                //searchWords
                s += "\n\nGesuchte Wörter: " + String.Join(", ", this.SearchWords.ToArray());
                return s;
            }
        }
        #endregion

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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
