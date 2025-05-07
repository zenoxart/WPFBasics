using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WPFBasics.Common;
using WPFBasics.Common.Services;

namespace WPFBasics.Common.Connection
{
    /// <summary>
    /// Stellt Methoden für den Versand und Empfang von UDP-Nachrichten bereit.
    /// </summary>
    public class UDPService
    {
        public LogService Logger { get; init; }
        public UdpClient UdpClient { get; private set; }

        /// <summary>
        /// Initialisiert den UDP-Client auf dem angegebenen Port.
        /// </summary>
        public void Initialize(int port)
        {
            try
            {
                UdpClient = new UdpClient(port);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "UDPService.Initialize");
                Logger?.LogException(ex, "UDPService.Initialize");
            }
        }

        /// <summary>
        /// Sendet eine Nachricht an die angegebene Adresse und Port.
        /// </summary>
        public async Task SendAsync(string message, string hostname, int port)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await UdpClient.SendAsync(data, data.Length, hostname, port);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "UDPService.SendAsync");
                Logger?.LogException(ex, "UDPService.SendAsync");
            }
        }

        /// <summary>
        /// Empfängt eine Nachricht asynchron.
        /// </summary>
        public async Task<string> ReceiveAsync()
        {
            try
            {
                var result = await UdpClient.ReceiveAsync();
                return Encoding.UTF8.GetString(result.Buffer);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "UDPService.ReceiveAsync");
                Logger?.LogException(ex, "UDPService.ReceiveAsync");
                return null;
            }
        }
    }
}
