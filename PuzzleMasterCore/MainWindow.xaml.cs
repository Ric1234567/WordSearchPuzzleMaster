using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PuzzleMasterCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CharGrid gridChar = new CharGrid(20, 20, 4);
        string lastGridText;
        List<string> wordLexicon = new List<string>();
        Random r = new Random();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = gridChar;

            //initVars();
            initWordLexicon();
        }

        /// <summary>
        /// Reset the grid
        /// </summary>
        private void resetGrid()
        {
            //reset the grid
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

        /// <summary>
        /// Creates and/or reads a list of words from a txt-file in the same folder as the exe
        /// </summary>
        private void initWordLexicon()//todo auslagerung in eigene Puzzle Klasse
        {
            string path = @"./SearchWords.txt";
            if (!File.Exists(path))
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
            gridChar.WordsToSearchCountMax = wordLexicon.Count;
            gridChar.WordsToSearchCount = wordLexicon.Count;
        }

        /// <summary>
        /// Logic for Random puzzle generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            resetGrid();

            int tries = 0;

            //only max
            if (gridChar.WordsToSearchCount > wordLexicon.Count)
            {
                MessageBox.Show("Too many SearchWords! Pick less.");
                return;
            }

            for (int i = 0; i < gridChar.WordsToSearchCount; i++)
            {
                //set possible directions from settings
                List<Directions> directions = new List<Directions>();
                if (CheckBoxDownwards.IsChecked == true)
                {
                    directions.Add(Directions.Down);
                }
                if (CheckBoxRightwards.IsChecked == true)
                {
                    directions.Add(Directions.Right);
                }
                if (CheckBoxLeftwards.IsChecked == true)
                {
                    directions.Add(Directions.Left);
                }
                if (CheckBoxUpwards.IsChecked == true)
                {
                    directions.Add(Directions.Up);
                }
                if (CheckBoxDiagonalRightDown.IsChecked == true)
                {
                    directions.Add(Directions.Diagonal_Right_Down);
                }
                if (CheckBoxDiagonalRightUp.IsChecked == true)
                {
                    directions.Add(Directions.Diagonal_Right_Up);
                }
                if (CheckBoxDiagonalLeftDown.IsChecked == true)
                {
                    directions.Add(Directions.Diagonal_Left_Down);
                }
                if (CheckBoxDiagonalLeftUp.IsChecked == true)
                {
                    directions.Add(Directions.Diagonal_Left_Up);
                }

                //pick random direction
                Directions randomDir = (Directions)directions[(r.Next(directions.Count))];

                //pick random word
                string word = wordLexicon[r.Next(0, wordLexicon.Count)].ToUpper();
                while (gridChar.SearchWords.Contains(word))
                {
                    word = wordLexicon[r.Next(0, wordLexicon.Count)].ToUpper();
                }
                gridChar.SearchWords.Add(word);

                //write in random x, y
                if (WriteWord(word, randomDir, r.Next(0, gridChar.Width), r.Next(0, gridChar.Height)) < 0 && tries < 1000)
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

        private int WriteWord(string word, Directions dir, int x, int y)
        {
            int count = 0;
            switch (dir)
            {
                case Directions.Right:
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

                case Directions.Left:
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

                case Directions.Down:
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

                case Directions.Up:
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
                case Directions.Diagonal_Right_Down:
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

                case Directions.Diagonal_Right_Up:
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

                case Directions.Diagonal_Left_Down:
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

                case Directions.Diagonal_Left_Up:
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

        private bool IsSpace(string word, Directions dir, int x, int y)
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
                case Directions.Right:
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

                case Directions.Left:
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

                case Directions.Down:
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

                case Directions.Up:
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
                case Directions.Diagonal_Right_Down:
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

                case Directions.Diagonal_Right_Up:
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

                case Directions.Diagonal_Left_Down:
                    //out of Bound
                    if (x - word.Length < 0 || y + word.Length > gridChar.Height)
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

                case Directions.Diagonal_Left_Up:
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
            if (lastGridText == null)
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
            throw new NotImplementedException();
        }

        private void MenuItemSaveText_Click(object sender, RoutedEventArgs e)
        {
            //todo
            throw new NotImplementedException();
        }

        private void MenuItemSaveSolution_Click(object sender, RoutedEventArgs e)
        {
            //todo
            throw new NotImplementedException();
        }

        private void CreatePDFButton_Click(object sender, RoutedEventArgs e)
        {
            //pdfsharp
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;

            //create graphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //gute schriftarten: "Courier New", "Consolas", "Lucida Console"
            XFont font = new XFont("Lucida Console", 18, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            //get puzzle text from text Richtextbox
            TextRange textRange = new TextRange(RichTextBox1.Document.ContentStart, RichTextBox1.Document.ContentEnd);
            string puzzleText = textRange.Text;

            int margin = 30;
            //draw text in pdf
            tf.DrawString(puzzleText, font, XBrushes.Black,
                new XRect(margin, margin, page.Width - margin, page.Height - margin),
                XStringFormats.TopLeft);
            //todo Anpassung der Schriftgröße an die eingegebenen CharGrid größe des erstellten Dokuments

            string filename = "puzzleDoc.pdf";

            //save to hard drive
            doc.Save(filename);


            MessageBox.Show("PDF-file " + filename + " created!");
        }
    }

}
