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
        public const string WINDOW_NAME = "Search Puzzle Master";

        private SearchPuzzle searchPuzzle = new SearchPuzzle();

        #region props
        public SearchPuzzle SearchPuzzle { get => searchPuzzle; set => searchPuzzle = value; }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Logic for Random puzzle generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPuzzle.GeneratePuzzle();
        }

        private void CopyTextButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.SearchPuzzle.PuzzleString);
        }

        private void SpoilerButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchPuzzle.PuzzleSolution == null || SearchPuzzle.PuzzleSolution.Textstring == null)
            {
                MessageBox.Show("Generate puzzle first!", WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //MessageBox.Show(this.SearchPuzzle.PuzzleSolution.Textstring, WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.None);

                PuzzleSolutionDialog.ShowNotification(WINDOW_NAME
               , this.SearchPuzzle.PuzzleSolution.Textstring);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPuzzle.WordLexicon = SearchPuzzle.RefreshWordLexicon();
        }

        private void MenuItemSaveText_Click(object sender, RoutedEventArgs e)
        {
            //todo save text
            throw new NotImplementedException();
        }

        private void MenuItemSaveSolution_Click(object sender, RoutedEventArgs e)
        {
            //todo save solution
            throw new NotImplementedException();
        }

        private void CreatePDFButton_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "puzzleDoc.pdf";

            SearchPuzzle.CreatePDFFile(filename);

            MessageBox.Show("PDF-file in \"" + filename + "\" created!", WINDOW_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}
