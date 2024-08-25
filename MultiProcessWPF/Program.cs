using System;

namespace MultiProcessWPF
{

    static class Program
    {
        /// <summary>
        /// Gets the main window in the application.
        /// </summary>
        internal static MainWindow MainWindow { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var app = new App();

            Uri uri = null;
            if (args.Length > 0)
            {
                // a URI was passed and needs to be handled
                try
                {
                    uri = new Uri(args[0].Trim());
                }
                catch (UriFormatException)
                {
                    Console.WriteLine("Invalid URI.");
                }
            }

            IUriHandler handler = UriHandler.GetHandler();
            if (handler != null)
            {
                // the singular instance of the application is already running
                if (uri != null)
                {
                    handler.HandleUri(uri);
                }
            }
            else
            {
                // this must become the singular instance of the application
                UriHandler.Register();

                app.InitializeComponent();
                app.Run();
            }
        }
    }
}