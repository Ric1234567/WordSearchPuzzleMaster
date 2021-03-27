using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace PuzzleMasterCore
{
    class PDFCreator
    {
        //const
        private XUnit MIN_PAGE_MARGIN = XUnit.FromMillimeter(11);
        private XUnit SPACE_BETWEEN_CHARS = XUnit.FromMillimeter(0);//not really needed

        public PDFCreator()
        {
            //empty
        }

        #region props
        //empty
        #endregion

        public void CreatePuzzlePdfFile(string fileName, SearchPuzzle searchPuzzle)
        {
            //pdfsharp
            PdfDocument pdfDoc = new PdfDocument();
            PdfPage page = pdfDoc.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;

            //create graphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            //max printable size of current page size
            XUnit maxPrintablePageWidth = page.Width - 2 * MIN_PAGE_MARGIN;
            XUnit maxPrintablePageHeight = page.Height - 2 * MIN_PAGE_MARGIN;

            //draw searchwordbox
            XUnit searchWordBoxHeight = DrawSearchWordBox(gfx, tf, searchPuzzle, maxPrintablePageWidth);

            //calc the font size to fit the page
            XUnit charSize = CalcPuzzleCharSize(maxPrintablePageWidth, maxPrintablePageHeight - searchWordBoxHeight, searchPuzzle.PuzzleCharGrid);
            //draw the puzzle grid
            DrawCharGrid(gfx, tf, charSize, searchPuzzle.PuzzleCharGrid, searchWordBoxHeight);

            //save to hard drive
            pdfDoc.Save(fileName);//todo abfangen, dass es von anderen prozess verwendet wird
        }
        public void CreateSolutionPdfFile(string fileName, SearchPuzzle searchPuzzle)
        {
            //pdfsharp
            PdfDocument pdfDoc = new PdfDocument();
            PdfPage page = pdfDoc.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;

            //create graphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            //max printable size of current page size
            XUnit maxPrintablePageWidth = page.Width - 2 * MIN_PAGE_MARGIN;
            XUnit maxPrintablePageHeight = page.Height - 2 * MIN_PAGE_MARGIN;

            XUnit charSize = CalcPuzzleCharSize(maxPrintablePageWidth, maxPrintablePageHeight, searchPuzzle.PuzzleSolution);

            DrawCharGrid(gfx, tf, charSize, searchPuzzle.PuzzleSolution, MIN_PAGE_MARGIN);

            //save to hard drive
            pdfDoc.Save(fileName);//todo abfangen, dass es von anderen prozess verwendet wird
        }

        private XUnit DrawSearchWordBox(XGraphics gfx, XTextFormatter tf, SearchPuzzle searchPuzzle, XUnit boxWidth)
        {
            //inits search word box
            const double searchWordsFontSize = 12;
            searchPuzzle.SearchWords.Sort();
            string searchWordsString = string.Join(", ", searchPuzzle.SearchWords);
            XFont searchWordsFont = new XFont("Lucida Console", searchWordsFontSize, XFontStyle.Bold);

            //measure search word string rows to fit page
            XSize size = gfx.MeasureString(searchWordsString, searchWordsFont);
            int searchWordsRows = (int)Math.Ceiling(size.Width / boxWidth);

            //calc height of box
            XUnit boxHeight = XUnit.FromPoint(MIN_PAGE_MARGIN + searchWordsFontSize * searchWordsRows);

            //draw search words and box
            tf.Alignment = XParagraphAlignment.Left;
            XRect rect = new XRect(MIN_PAGE_MARGIN, MIN_PAGE_MARGIN, boxWidth, searchWordsFontSize * searchWordsRows);
            gfx.DrawRectangle(XBrushes.LightGray, rect);//background
            gfx.DrawRectangle(XPens.Black, rect);//border
            tf.DrawString(searchWordsString, searchWordsFont, XBrushes.Black, rect);//searchwords

            return boxHeight;
        }

        private XUnit CalcPuzzleCharSize(XUnit maxPrintablePageWidth, XUnit maxPrintablePageHeight, CharGrid charGrid)
        {
            //height
            XUnit totalCharGridSpaceHeight = maxPrintablePageHeight - SPACE_BETWEEN_CHARS * (charGrid.Height - 1);
            XUnit maxCharHeight = totalCharGridSpaceHeight / charGrid.Height;
            //width
            XUnit totalCharGridSpaceWidth = maxPrintablePageWidth - SPACE_BETWEEN_CHARS * (charGrid.Width - 1);
            XUnit maxCharWidth = totalCharGridSpaceWidth / charGrid.Width;

            //use smaller size to create a square for each char to fit the overall page size
            return Math.Min(maxCharHeight, maxCharWidth);
        }

        private void DrawCharGrid(XGraphics gfx, XTextFormatter tf, XUnit charSquareSize, CharGrid charGrid, XUnit yCoordPuzzle)
        {
            //nice fonts: "Courier New", "Consolas", "Lucida Console"
            XFont charGridFont = new XFont("Lucida Console", charSquareSize.Point, XFontStyle.Bold);

            //draw borders of char grid
            gfx.DrawRectangle(XPens.Black, MIN_PAGE_MARGIN, yCoordPuzzle, charSquareSize * charGrid.Width, charSquareSize * charGrid.Height);//used size of the puzzle

            tf.Alignment = XParagraphAlignment.Center;
            //draw chars of grid
            for (int y = 0; y < charGrid.Height; y++)
            {
                for (int x = 0; x < charGrid.Width; x++)
                {
                    double xCoord = MIN_PAGE_MARGIN + x * (charSquareSize + SPACE_BETWEEN_CHARS);
                    double yCoord = yCoordPuzzle + y * (charSquareSize + SPACE_BETWEEN_CHARS);

                    //draw each char
                    tf.DrawString(charGrid.CharacterGrid[x, y].ToString(), charGridFont, XBrushes.Black,
                        new XRect(xCoord, yCoord,
                        charSquareSize, charSquareSize));
                }
            }
        }

    }
}
