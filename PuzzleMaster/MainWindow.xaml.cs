using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PuzzleMaster
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        CharGrid gridChar;
        string lastGridText;
        List<string> wordLexicon = new List<string>();
        Random r = new Random();

        public MainWindow()
        {
            InitializeComponent();

            initWordLexicon();
        }

        private void initVars()
        {
            int wordsToSearchCount = Convert.ToInt32(Math.Round(NumericUpDownWordAmount.Value, 0));
            int width = Convert.ToInt32(Math.Round(NumericUpDownWidth.Value, 0));
            int height = Convert.ToInt32(Math.Round(NumericUpDownHeight.Value, 0));
            gridChar = new CharGrid(width, height, wordsToSearchCount);

            gridChar.initCharGrid();

            
            for (int x = 0; x < gridChar.Width; x++)
            {
                for (int y = 0; y < gridChar.Height; y++)
                {
                    gridChar.getCharGrid()[x, y] = '_';
                }
            }
            

            //reset
            gridChar.SearchWords.Clear();
        }

        private char randomChar()
        {
            return (char)r.Next(65, 90);
        }

        private void initWordLexicon()
        {
            string path = @"./SearchWords.txt";
            if(!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("HAUS");
                    sw.WriteLine("FEUERWEHR");
                    sw.WriteLine("BAUM");
                    sw.WriteLine("COMPUTER");
                    sw.WriteLine("HANDLANGER");
                    sw.WriteLine("SUPERMAN");
                }
            }
          
            //read searchWords from file
            string[] searchWords = System.IO.File.ReadAllLines(path);
            wordLexicon = searchWords.ToList<string>();

            //set max
            LabelSearchWordsMax.Content = wordLexicon.Count;
            NumericUpDownWordAmount.Value = wordLexicon.Count;
        }


        /*
            Logic for Random puzzle generation 
             */
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            initVars();

            int tries = 0;

            //only max
            if (gridChar.WordsToSearchCount > wordLexicon.Count)
            {
                MessageBox.Show("Too many SearchWords! Pick less.");
                return;
            }
            
            for(int i = 0; i < gridChar.WordsToSearchCount; i++)
            {
                //set possible directions from settings
                List<Direction> directions = new List<Direction>();
                if (CheckBoxDownwards.IsChecked == true)
                {
                    directions.Add(Direction.Down);
                }
                if (CheckBoxRightwards.IsChecked == true)
                {
                    directions.Add(Direction.Right);
                }
                if (CheckBoxLeftwards.IsChecked == true)
                {
                    directions.Add(Direction.Left);
                }
                if(CheckBoxUpwards.IsChecked == true)
                {
                    directions.Add(Direction.Up);
                }
                if(CheckBoxDiagonalRightDown.IsChecked == true)
                {
                    directions.Add(Direction.Diagonal_Right_Down);
                }
                if (CheckBoxDiagonalRightUp.IsChecked == true)
                {
                    directions.Add(Direction.Diagonal_Right_Up);
                }
                if (CheckBoxDiagonalLeftDown.IsChecked == true)
                {
                    directions.Add(Direction.Diagonal_Left_Down);
                }
                if (CheckBoxDiagonalLeftUp.IsChecked == true)
                {
                    directions.Add(Direction.Diagonal_Left_Up);
                }

                //pick random direction
                Direction randomDir = (Direction)directions[(r.Next(directions.Count))];
                
                //pick random word
                string word = wordLexicon[r.Next(0, wordLexicon.Count)].ToUpper();
                while (gridChar.SearchWords.Contains(word)){
                    word = wordLexicon[r.Next(0, wordLexicon.Count)].ToUpper();
                }
                gridChar.SearchWords.Add(word);

                //write in random x, y
                if(WriteWord(word, randomDir, r.Next(0, gridChar.Width), r.Next(0, gridChar.Height)) < 0 && tries < 1000)
                {
                    //das wort konnte nirgends untergebracht werden
                    i--;
                    gridChar.SearchWords.Remove(word);

                    tries++;
                }
                
            }
            if (tries >= 100)
            {
                MessageBox.Show("Could not generate puzzle! Change your Settings.");
                return;
            }

            //backup last
            lastGridText = gridChar.GridText;

            //set rest of search grid to random chars
            for (int x = 0; x < gridChar.Width; x++)
            {
                for (int y = 0; y < gridChar.Height; y++)
                {
                    if (gridChar.getCharGrid()[x, y] == '_')
                    {
                        gridChar.setChar(x, y, randomChar());
                    }
                }
            }

            RichTextBox1.Document.Blocks.Clear();
            RichTextBox1.Document.Blocks.Add(new Paragraph(new Run(gridChar.GridText)));
        }

        private int WriteWord(string word, Direction dir, int x, int y)
        {
            int count = 0;
            switch (dir)
            {
                case Direction.Right:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x++;
                    }
                    return 1;
                    break;

                case Direction.Left:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x--;
                    }
                    return 1;
                    break;

                case Direction.Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        y++;
                    }
                    return 1;
                    break;

                case Direction.Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        y--;
                    }
                    return 1;
                    break;

                //diagonal
                case Direction.Diagonal_Right_Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x++;
                        y++;
                    }
                    return 1;
                    break;

                case Direction.Diagonal_Right_Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x++;
                        y--;
                    }
                    return 1;
                    break;

                case Direction.Diagonal_Left_Down:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x--;
                        y++;
                    }
                    return 1;
                    break;

                case Direction.Diagonal_Left_Up:
                    while (!IsSpace(word, dir, x, y))
                    {
                        x = r.Next(0, gridChar.Width);
                        y = r.Next(0, gridChar.Height);

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
                        gridChar.getCharGrid()[x, y] = word[i];
                        x--;
                        y--;
                    }
                    return 1;
                    break;

                default:
                    return -1;
            }
        }

        private bool IsSpace(string word, Direction dir, int x, int y)
        {
            //out of bound
            if (x < 0 || y < 0)
            {
                return false;
            }
            else if (x > gridChar.Width || y > gridChar.Height)
            {
                return false;
            }


            switch (dir)
            {
                case Direction.Right:
                    //out of Bound
                    if (x + word.Length > gridChar.Width)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        x++;
                        if (x > gridChar.Width)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.Left:
                    //out of Bound
                    if (x - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
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

                case Direction.Down:
                    //out of Bound
                    if (y + word.Length > gridChar.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        if (y > gridChar.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.Up:
                    //out of Bound
                    if (y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
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
                case Direction.Diagonal_Right_Down:
                    //out of Bound
                    if (x + word.Length > gridChar.Width || y + word.Length > gridChar.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        x++;
                        if (x > gridChar.Width || y > gridChar.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.Diagonal_Right_Up:
                    //out of Bound
                    if (x + word.Length > gridChar.Width || y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y--;
                        x++;
                        if (x > gridChar.Width || y < 0)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.Diagonal_Left_Down:
                    //out of Bound
                    if (x - word.Length < 0|| y + word.Length > gridChar.Height)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
                            {
                                return false;
                            }
                        }

                        y++;
                        x--;
                        if (x < 0 || y > gridChar.Height)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.Diagonal_Left_Up:
                    //out of Bound
                    if (x - word.Length < 0 || y - word.Length < 0)
                    {
                        return false;
                    }

                    //test if fit in other word (Bau in Baum)
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (gridChar.getCharGrid()[x, y] != '_')
                        {
                            if (gridChar.getCharGrid()[x, y] != word[i])
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

        private void CopyTextButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(new TextRange(RichTextBox1.Document.ContentStart, RichTextBox1.Document.ContentEnd).Text);
        }

        private void SpoilerButton_Click(object sender, RoutedEventArgs e)
        {
            if(lastGridText == null)
            {
                MessageBox.Show("Generate puzzle first!");
            }
            else
            {
                RichTextBox1.Document.Blocks.Clear();
                RichTextBox1.Document.Blocks.Add(new Paragraph(new Run(lastGridText)));
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            //todo refresh the loaded lexicon
        }

        private void MenuItemSaveText_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void MenuItemSaveSolution_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }
    }
}

/*
 - wort ist allgemein länger als width oder height
 - wort überschreibt anderes wort -> außer es passt zusammen (z.B Bau passt in Baum)
     */