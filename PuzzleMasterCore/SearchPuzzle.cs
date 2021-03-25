using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace PuzzleMasterCore
{
    public class SearchPuzzle : INotifyPropertyChanged
    {
        const string SEARCH_WORDS_PATH = @"./SearchWords.txt";

        private int puzzleWidth = 20;
        private int puzzleHeight = 30;
        private CharGrid puzzleCharGrid;
        private CharGrid solutionCharGrid;

        private List<string> wordLexicon;
        private List<string> searchWords = new List<string>();//needed
        private int wordsToSearchCount = 0;

        DirectionSettings directionSettings = new DirectionSettings();

        private Random random = new Random();//todo optimiern mit CharGrid

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchPuzzle()
        {
            LoadWordLexicon();
            this.PuzzleCharGrid = new CharGrid(this.PuzzleWidth, this.PuzzleHeight);
        }

        #region props
        public CharGrid PuzzleCharGrid
        {
            get { return puzzleCharGrid; }
            set
            {
                puzzleCharGrid = value;
                OnPropertyChanged(nameof(PuzzleCharGrid));
            }
        }
        public CharGrid PuzzleSolution { get => solutionCharGrid; set => solutionCharGrid = value; }
        public int WordsToSearchCount
        {
            get { return wordsToSearchCount; }
            set
            {
                wordsToSearchCount = value;
                OnPropertyChanged(nameof(WordsToSearchCount));
            }
        }
        public int WordsToSearchCountMax { get => WordLexicon.Count; }

        /// <summary>
        /// Selected Words which will be used to create the grid.
        /// </summary>
        public List<string> SearchWords { get => searchWords; set => searchWords = value; }
        public List<string> WordLexicon
        {
            get { return wordLexicon; }
            set
            {
                wordLexicon = value;

                OnPropertyChanged(nameof(WordLexicon));
                OnPropertyChanged(nameof(WordsToSearchCountMax));
            }
        }
        public int PuzzleWidth
        {
            get { return puzzleWidth; }
            set
            {
                puzzleWidth = value;
                OnPropertyChanged(nameof(PuzzleWidth));
            }
        }
        public int PuzzleHeight
        {
            get { return puzzleHeight; }
            set
            {
                puzzleHeight = value;
                OnPropertyChanged(nameof(PuzzleHeight));
            }
        }
        public DirectionSettings DirectionSettings
        {
            get { return directionSettings; }
        }

        /// <summary>
        /// Returns the output of the puzzleGeneration as string including the words to search.
        /// </summary>
        public string PuzzleString
        {
            get
            {
                this.SearchWords.Sort();
                return this.PuzzleCharGrid.Textstring + System.Environment.NewLine + string.Join(", ", this.SearchWords);
            }
        }
        #endregion

        /// <summary>
        /// Creates and/or reads a list of words from a txt-file in the same folder as the exe
        /// </summary>
        public void LoadWordLexicon()
        {
            if (!File.Exists(SEARCH_WORDS_PATH))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(SEARCH_WORDS_PATH))
                {
                    //default words:
                    sw.WriteLine("HAUS");
                    sw.WriteLine("FEUERWEHR");
                    sw.WriteLine("BAUM");
                    sw.WriteLine("COMPUTER");
                    sw.WriteLine("HANDLANGER");
                    sw.WriteLine("SUPERMAN");
                    sw.WriteLine("LAGER");
                    sw.WriteLine("LAGERREGAL");
                    sw.WriteLine("REGAL");
                    sw.WriteLine("BAUMHAUS");
                }
            }

            //read searchWords from file and add to the lexicon of the puzzle
            this.WordLexicon = new List<string>(ReadSearchWordsFromFile(SEARCH_WORDS_PATH));

            //default words to search
            this.WordsToSearchCount = this.WordsToSearchCountMax;
        }

        /// <summary>
        /// Reads words from a file line by line in a List
        /// </summary>
        /// <param name="path">Path to the txt-file including the words line by line</param>
        /// <returns>List with words</returns>
        private List<string> ReadSearchWordsFromFile(string path)
        {
            List<string> words = System.IO.File.ReadAllLines(path).ToList<string>();

            //remove null or empty strings
            words = words.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return words;
        }

        /// <summary>
        /// Refreshes the WordLexicon by reading the file
        /// </summary>
        /// <returns>WordLexicon which contains possible words for the puzzle</returns>
        private List<string> RefreshWordLexicon()
        {
            LoadWordLexicon();
            return this.ReadSearchWordsFromFile(SEARCH_WORDS_PATH);
        }

        /// <summary>
        /// Create a PDF-File from the puzzle Text
        /// </summary>
        /// <param name="fileName"></param>
        public void CreatePDFFile(string fileName)
        {
            //pdfsharp
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;

            //create graphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //inits search word box
            double searchWordsFontSize = 12;
            this.SearchWords.Sort();
            string searchWordsString = string.Join(", ", this.SearchWords);
            XFont searchWordsFont = new XFont("Lucida Console", searchWordsFontSize, XFontStyle.Bold);

            XUnit MIN_PAGE_MARGIN = XUnit.FromMillimeter(11);
            XUnit SPACE_BETWEEN_CHARS = XUnit.FromMillimeter(0);//not needed

            //max printable size of current page size
            XUnit maxPrintablePageWidth = page.Width - 2 * MIN_PAGE_MARGIN;
            XUnit maxPrintablePageHeight = page.Height - 2 * MIN_PAGE_MARGIN;

            //measure search word string rows to fit page
            XSize size = gfx.MeasureString(searchWordsString, searchWordsFont);
            int searchWordsRows = (int)Math.Ceiling(size.Width / maxPrintablePageWidth);

            //to place puzzle after searchwords box
            XUnit puzzleCoordY = XUnit.FromPoint(MIN_PAGE_MARGIN + searchWordsFontSize * searchWordsRows);

            ///char grid
            //height
            XUnit totalCharGridSpaceHeight = maxPrintablePageHeight - SPACE_BETWEEN_CHARS * (this.PuzzleHeight - 1) - puzzleCoordY;
            XUnit maxCharHeight = totalCharGridSpaceHeight / this.PuzzleHeight;
            //width
            XUnit totalCharGridSpaceWidth = maxPrintablePageWidth - SPACE_BETWEEN_CHARS * (this.PuzzleWidth - 1);
            XUnit maxCharWidth = totalCharGridSpaceWidth / this.PuzzleWidth;

            //use smaller size to create a square for each char to fit the overall page size
            XUnit charSquareSize = Math.Min(maxCharHeight, maxCharWidth);
            double fontSize = charSquareSize.Point;

            
            //nice fonts: "Courier New", "Consolas", "Lucida Console"
            XFont charGridFont = new XFont("Lucida Console", fontSize, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            //draw search words and box
            tf.Alignment = XParagraphAlignment.Left;
            XRect rect = new XRect(MIN_PAGE_MARGIN, MIN_PAGE_MARGIN, maxPrintablePageWidth, searchWordsFontSize * searchWordsRows);
            gfx.DrawRectangle(XBrushes.LightGray, rect);
            gfx.DrawRectangle(XPens.Black, rect);
            tf.DrawString(searchWordsString, searchWordsFont, XBrushes.Black, rect);


            //draw borders of char grid
            gfx.DrawRectangle(XPens.Black, MIN_PAGE_MARGIN, puzzleCoordY, charSquareSize * this.PuzzleWidth, charSquareSize * this.PuzzleHeight);//used size of the puzzle

            tf.Alignment = XParagraphAlignment.Center;
            //draw chars of grid
            for (int y = 0; y < this.PuzzleHeight; y++)
            {
                for (int x = 0; x < this.PuzzleWidth; x++)
                {
                    double xCoord = MIN_PAGE_MARGIN + x * (charSquareSize + SPACE_BETWEEN_CHARS);
                    double yCoord = puzzleCoordY + y * (charSquareSize + SPACE_BETWEEN_CHARS);

                    tf.DrawString(this.PuzzleCharGrid.CharacterGrid[x, y].ToString(), charGridFont, XBrushes.Black,
                        new XRect(xCoord, yCoord,
                        maxCharWidth, maxCharHeight));
                }
            }

            //save to hard drive
            doc.Save(fileName);//todo abfangen, dass es von anderen prozess verwendet wird
        }

        /// <summary>
        /// Picks a random direction
        /// </summary>
        /// <returns></returns>
        private Directions PickRandomDirection()
        {
            return this.DirectionSettings.SelectedDirections[random.Next(this.DirectionSettings.SelectedDirections.Count)];
        }

        /// <summary>
        /// Generates a random puzzle
        /// </summary>
        /// <returns></returns>
        public void GeneratePuzzle()
        {
            //Resets
            this.PuzzleCharGrid = new CharGrid(this.PuzzleWidth, this.PuzzleHeight);
            this.PuzzleCharGrid.ResetGrid();
            this.SearchWords.Clear();

            int tries = 0;

            //only max
            if (WordsToSearchCount > WordLexicon.Count)
            {
                MessageBox.Show("Too many SearchWords! Pick less.", MainWindow.WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //copy words for this puzzle
            List<string> tempLexicon = new List<string>(WordLexicon);

            for (int i = 0; i < WordsToSearchCount; i++)
            {
                //pick random direction
                Directions randomDir = this.PickRandomDirection();

                if (tempLexicon.Count > 0)
                {
                    //pick random word
                    string word = tempLexicon[random.Next(0, tempLexicon.Count)];

                    //remove word in both cases
                    tempLexicon.Remove(word);
                    if (word.Length > this.PuzzleWidth || word.Length > this.PuzzleHeight)
                    {
                        MessageBox.Show("\"" + word + "\" is too long for the size of this puzzle! It will not be used.", MainWindow.WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        //remove to prevent the same words multiple times
                        word = word.ToUpper();
                        SearchWords.Add(word);

                        //write in random x, y
                        if (!PuzzleCharGrid.InsertWordAtRandomPosition(word, randomDir) && tries < 1000)
                        {
                            //das wort konnte nirgends untergebracht werden
                            i--;
                            SearchWords.Remove(word);

                            tries++;
                        }
                    }
                }
            }
            if (tries >= 100)
            {
                MessageBox.Show("Could not generate puzzle! Change your Settings.", MainWindow.WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //save puzzle solution
            this.PuzzleSolution = new CharGrid(this.PuzzleWidth, this.PuzzleHeight);
            this.PuzzleSolution.CharacterGrid = (char[,])this.PuzzleCharGrid.CharacterGrid.Clone();

            //set rest of search grid to random chars
            this.PuzzleCharGrid.SetEmptyToRandom();

            OnPropertyChanged(nameof(PuzzleString));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
