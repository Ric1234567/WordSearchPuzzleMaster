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
        SearchPuzzle searchPuzzle = new SearchPuzzle();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = searchPuzzle;
        }

        /// <summary>
        /// Logic for Random puzzle generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            searchPuzzle.GeneratePuzzle();

            //set to text box//todo binding textbox
            RichTextBox1.Document.Blocks.Clear();
            RichTextBox1.Document.Blocks.Add(new Paragraph(new Run(searchPuzzle.PuzzleCharGrid.Textstring)));
        }

        private void CopyTextButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(new TextRange(RichTextBox1.Document.ContentStart, RichTextBox1.Document.ContentEnd).Text);
        }

        private void SpoilerButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchPuzzle.PuzzleSolution.Textstring == null)
            {
                MessageBox.Show("Generate puzzle first!");
            }
            else
            {
                RichTextBox1.Document.Blocks.Clear();
                RichTextBox1.Document.Blocks.Add(new Paragraph(new Run(searchPuzzle.PuzzleSolution.Textstring)));
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            searchPuzzle.WordLexicon = searchPuzzle.RefreshWordLexicon();
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
            const string filename = "puzzleDoc.pdf";

            searchPuzzle.CreatePDFFile(filename);

            MessageBox.Show("PDF-file " + filename + " created!");
        }
    }

}
