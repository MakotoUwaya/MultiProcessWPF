using System;
using System.IO;
using Microsoft.Win32;

namespace Installer
{

    /// <summary>
    /// Entry point for the installer.
    /// </summary>
    class Program
    {

        const string URI_SCHEME = "mukwty-wpf";
        const string URI_KEY = "URL:mukwty Protocol";

        static void RegisterUriScheme(string appPath)
        {
            // HKEY_CLASSES_ROOT\es-one-desktop-test
            using (var hkcrClass = Registry.ClassesRoot.CreateSubKey(URI_SCHEME))
            {
                hkcrClass.SetValue(null, URI_KEY);
                hkcrClass.SetValue("URL Protocol", string.Empty, RegistryValueKind.String);

                // use the application's icon as the URI scheme icon
                using (var defaultIcon = hkcrClass.CreateSubKey("DefaultIcon"))
                {
                    var iconValue = string.Format("\"{0}\",0", appPath);
                    defaultIcon.SetValue(null, iconValue);
                }

                // open the application and pass the URI to the command-line
                using (var shell = hkcrClass.CreateSubKey("shell"))
                {
                    using (var open = shell.CreateSubKey("open"))
                    {
                        using (var command = open.CreateSubKey("command"))
                        {
                            var cmdValue = string.Format("\"{0}\" \"%1\"", appPath);
                            command.SetValue(null, cmdValue);
                        }
                    }
                }
            }
        }

        static void UnregisterUriScheme()
        {
            Registry.ClassesRoot.DeleteSubKeyTree(URI_SCHEME);
        }

        static void Main(string[] args)
        {
            if ((args.Length > 0) && (args[0].Equals("/u") || args[0].Equals("-u")))
            {
                // uninstall
                Console.Write("Attempting to unregister URI scheme...");

                try
                {
                    UnregisterUriScheme();
                    Console.WriteLine(" Success.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Failed!");
                    Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
                }
            }
            else
            {
                // install
                var appPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "MultiProcessWPF.exe");

                Console.Write("Attempting to register URI scheme...");

                try
                {
                    if (!File.Exists(appPath))
                    {
                        throw new InvalidOperationException(string.Format("Application not found at: {0}", appPath));
                    }

                    RegisterUriScheme(appPath);
                    Console.WriteLine(" Success.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Failed!");
                    Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
