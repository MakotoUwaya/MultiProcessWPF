using System;
using System.Windows;

namespace MultiProcessWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Program.MainWindow = this;
        }

        /// <summary>
        /// Adds the specified URI to the text area on the form.
        /// </summary>
        /// <param name="uri"></param>
        public void AddUri(Uri uri)
        {
            try
            {
                if (this.Dispatcher.CheckAccess())
                {
                    textArea.Text += uri.ToString() + Environment.NewLine;
                    this.Activate();
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action<Uri>(AddUri), uri);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed 'AddUri': {e.Message}");
            }

        }
    }
}
