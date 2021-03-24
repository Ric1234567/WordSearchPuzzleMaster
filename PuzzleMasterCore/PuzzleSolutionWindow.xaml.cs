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
