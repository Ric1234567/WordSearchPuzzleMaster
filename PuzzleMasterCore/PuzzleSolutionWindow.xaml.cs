using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PuzzleMasterCore
{
    public partial class PuzzleSolutionDialog : Window, INotifyPropertyChanged
    {
        private string _message;

        public event PropertyChangedEventHandler PropertyChanged;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        public static void ShowNotification(string title, string message)
        {
            PuzzleSolutionDialog dialog = new PuzzleSolutionDialog();
            dialog.Title = title;
            dialog.Message = message;
            dialog.ShowDialog();
        }
        public PuzzleSolutionDialog()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += (s, e) =>
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            };
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyPropertyChanged(); }
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
