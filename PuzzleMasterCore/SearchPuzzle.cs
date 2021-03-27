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
