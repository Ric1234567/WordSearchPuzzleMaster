using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PuzzleMasterCore
{
    public class CharGrid : INotifyPropertyChanged
    {
        private int width = 0;
        private int height = 0;
        private char[,] charGrid;
        private Random r = new Random();

        public event PropertyChangedEventHandler PropertyChanged;

        public CharGrid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.InitCharGrid();
        }

        #region Props
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                InitCharGrid();
                OnPropertyChanged(nameof(Width));
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                InitCharGrid();
                OnPropertyChanged(nameof(Height));
            }
        }

        /// <summary>
        /// Grid as string
        /// </summary>
        public string Textstring
        {
            get
            {
                string s = "";

                //iterate grid array
                for (int y = 0; y < this.Height; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        s += this.CharacterGrid[x, y];
                    }

                    //add new line
                    s += System.Environment.NewLine;
                }

                return s;
            }
        }

        public char[,] CharacterGrid
        {
            get { return charGrid; }
            set
            {
                charGrid = value;
                OnPropertyChanged(nameof(CharacterGrid));
            }
        }
        #endregion

        /// <summary>
        /// Sets all "_" in the grid to random chars
        /// </summary>
        public void SetEmptyToRandom()
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (this.CharacterGrid[x, y] == '_')
                    {
                        this.CharacterGrid[x, y] = this.RandomChar();
                    }
                }
            }
            OnPropertyChanged(nameof(Textstring));
        }

        public void InitCharGrid()
        {
            this.CharacterGrid = new char[this.Width, this.Height];
        }

        /// <summary>
        /// Resets the grid with "_" in every cell
        /// </summary>
        public void ResetGrid()
        {
            //reset the grid
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.CharacterGrid[x, y] = '_';
                }
            }
        }

        /// <summary>
        /// Writes a word in the grid
        /// </summary>
        /// <param name="word"></param>
        /// <param name="dir"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int WriteWord(string word, Directions dir, int x, int y)
        {
            int count = 0;
            switch (dir)
            {
                case Directions.Right:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                    }
                    return 1;
                    break;

                case Directions.Left:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                    }
                    return 1;
                    break;

                case Directions.Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        y++;
                    }
                    return 1;
                    break;

                case Directions.Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        y--;
                    }
                    return 1;
                    break;

                //diagonal
                case Directions.Diagonal_Right_Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                        y++;
                    }
                    return 1;
                    break;

                case Directions.Diagonal_Right_Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                        y--;
                    }
                    return 1;
                    break;

                case Directions.Diagonal_Left_Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                        y++;
                    }
                    return 1;
                    break;

                case Directions.Diagonal_Left_Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, this.Width);
                        y = r.Next(0, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return -1;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                        y--;
                    }
                    return 1;
                    break;

                default:
                    return -1;
            }
        }

        private bool IsSpace(string word, Directions dir, int x, int y)
        {
            //out of bound
            if (x < 0 || y < 0)
            {
                return false;
            }
            else if (x > this.Width || y > this.Height)
            {
                return false;
            }


            switch (dir)
            {
                case Directions.Right:
                    //out of Bound
                    if (x + word.Length > this.Width)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        x++;
                        if (x > this.Width)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Left:
                    //out of Bound
                    if (x - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        x--;
                        if (x < 0)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Down:
                    //out of Bound
                    if (y + word.Length > this.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        if (y > this.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Up:
                    //out of Bound
                    if (y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y--;
                        if (y < 0)
                        {
                            return false;
                        }
                    }
                    break;

                //diagonal
                case Directions.Diagonal_Right_Down:
                    //out of Bound
                    if (x + word.Length > this.Width || y + word.Length > this.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        x++;
                        if (x > this.Width || y > this.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Diagonal_Right_Up:
                    //out of Bound
                    if (x + word.Length > this.Width || y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y--;
                        x++;
                        if (x > this.Width || y < 0)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Diagonal_Left_Down:
                    //out of Bound
                    if (x - word.Length < 0 || y + word.Length > this.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        x--;
                        if (x < 0 || y > this.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Diagonal_Left_Up:
                    //out of Bound
                    if (x - word.Length < 0 || y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (this.CharacterGrid[x, y] != '_')
                        {
                            if (this.CharacterGrid[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y--;
                        x--;
                        if (x < 0 || y < 0)
                        {
                            return false;
                        }
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        public char RandomChar()
        {
            return (char)r.Next(65, 90);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
