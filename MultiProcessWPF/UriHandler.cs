using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace MultiProcessWPF
{

    /// <summary>
    /// Example implementation of a URI handler.
    /// </summary>
    public class UriHandler : MarshalByRefObject, IUriHandler {

        const string IPC_CHANNEL_NAME = "SingleInstanceWithUriScheme";

        /// <summary>
        /// Registers the URI handler in the singular instance of the application.
        /// </summary>
        /// <returns></returns>
        public static bool Register() {
            try {
                IpcServerChannel channel = new IpcServerChannel(IPC_CHANNEL_NAME);
                ChannelServices.RegisterChannel(channel, true);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(UriHandler), "UriHandler", WellKnownObjectMode.SingleCall);

                return true;
            }
            catch {
                Console.WriteLine("Couldn't register IPC channel.");
                Console.WriteLine();
            }

            return false;
        }

        /// <summary>
        /// Returns the URI handler from the singular instance of the application, or null if there is no other instance.
        /// </summary>
        /// <returns></returns>
        public static IUriHandler GetHandler() {
            try {
                IpcClientChannel channel = new IpcClientChannel();
                ChannelServices.RegisterChannel(channel, true);
                string address = $"ipc://{IPC_CHANNEL_NAME}/UriHandler";
                IUriHandler handler = (IUriHandler)RemotingServices.Connect(typeof(IUriHandler), address);

                // need to test whether connection was established
                TextWriter.Null.WriteLine(handler.ToString());

                return handler;
            }
            catch {
                Console.WriteLine("Couldn't get remote UriHandler object.");
                Console.WriteLine();
            }

            return null;
        }

        /// <summary>
        /// Handles the URI.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool HandleUri(Uri uri) {
            // this is only a demonstration; a real implementation would:
            // - validate the URI
            // - perform a particular action depending on the URI

            Program.MainWindow.AddUri(uri);
            return true;
        }
    }
}
