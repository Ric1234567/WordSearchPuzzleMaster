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
        private Random random = new Random();

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
        /// Inserts a word at a random position in the grid
        /// </summary>
        /// <param name="word"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool InsertWordAtRandomPosition(string word, Directions dir)
        {
            int count = 0;
            int x;
            int y;

            switch (dir)
            {
                case Directions.Right:
                    //pick random position for this direction
                    x = random.Next(this.Width - word.Length + 1);
                    y = random.Next(this.Height);

                    //check for space (or blocked by other words etc.)
                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(this.Width - word.Length + 1);
                        y = random.Next(this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                    }
                    return true;

                case Directions.Left:
                    //pick random position for this direction
                    x = random.Next(word.Length - 1, this.Width);
                    y = random.Next(this.Height);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(word.Length - 1, this.Width);
                        y = random.Next(this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                    }
                    return true;

                case Directions.Down:
                    //pick random position for this direction
                    x = random.Next(this.Width);
                    y = random.Next(this.Height - word.Length + 1);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(this.Width);
                        y = random.Next(this.Height - word.Length + 1);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        y++;
                    }
                    return true;

                case Directions.Up:
                    //pick random position for this direction
                    x = random.Next(this.Width);
                    y = random.Next(word.Length - 1, this.Height);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(this.Width);
                        y = random.Next(word.Length - 1, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        y--;
                    }
                    return true;

                //diagonal
                case Directions.Diagonal_Right_Down:
                    //pick random position for this direction
                    x = random.Next(this.Width - word.Length + 1);
                    y = random.Next(this.Height - word.Length + 1);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(this.Width - word.Length + 1);
                        y = random.Next(this.Height - word.Length + 1);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                        y++;
                    }
                    return true;

                case Directions.Diagonal_Right_Up:
                    //pick random position for this direction
                    x = random.Next(this.Width - word.Length);
                    y = random.Next(word.Length - 1, this.Height);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(this.Width - word.Length);
                        y = random.Next(word.Length - 1, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x++;
                        y--;
                    }
                    return true;

                case Directions.Diagonal_Left_Down:
                    //pick random position for this direction
                    x = random.Next(word.Length - 1, this.Width);
                    y = random.Next(this.Height - word.Length);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(word.Length - 1, this.Width);
                        y = random.Next(this.Height - word.Length);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                        y++;
                    }
                    return true;

                case Directions.Diagonal_Left_Up:
                    //pick random position for this direction
                    x = random.Next(word.Length - 1, this.Width);
                    y = random.Next(word.Length - 1, this.Height);

                    while (!IsSpace(word, dir, x, y))
                    {
                        //pick new random position
                        x = random.Next(word.Length - 1, this.Width);
                        y = random.Next(word.Length - 1, this.Height);

                        //stopping condition
                        count++;
                        if (count > 100)
                        {
                            return false;
                        }
                    }

                    //set in the word
                    for (int i = 0; i < word.Length; i++)
                    {
                        this.CharacterGrid[x, y] = word[i];
                        x--;
                        y--;
                    }
                    return true;

                default:
                    return false;
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
                    if (x - word.Length + 1 < 0)
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
                        if (x + 1 < 0)
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
                    if (y - word.Length + 1 < 0)
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
                        if (y + 1 < 0)
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
                    if (x + word.Length > this.Width || y - word.Length + 1 < 0)
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
                        if (x > this.Width || y + 1 < 0)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Diagonal_Left_Down:
                    //out of Bound
                    if (x + 1 - word.Length < 0 || y + word.Length > this.Height)
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
                        if (x + 1 < 0 || y > this.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Directions.Diagonal_Left_Up:
                    //out of Bound
                    if (x - word.Length + 1 < 0 || y - word.Length + 1 < 0)
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
                        if (x + 1 < 0 || y + 1 < 0)
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
            return (char)random.Next(65, 90);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
