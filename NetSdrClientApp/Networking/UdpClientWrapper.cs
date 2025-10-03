using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetSdrClientApp.Networking
{
    public class UdpClientWrapper : IUdpClient
    {
        private UdpClient? _udpClient;
        private CancellationTokenSource? _cts;
        private bool _isListening;

        public event EventHandler<byte[]>? MessageReceived;
        public bool IsListening => _isListening;

        public async Task StartListeningAsync()
        {
            if (_isListening) 
                return;

            try
            {
                _cts = new CancellationTokenSource();
                _udpClient = new UdpClient(50000); // Використовуйте потрібний порт
                _isListening = true;

                while (!_cts.Token.IsCancellationRequested)
                {
                    var result = await _udpClient.ReceiveAsync(_cts.Token);
                    MessageReceived?.Invoke(this, result.Buffer);
                }
            }
            catch (OperationCanceledException)
            {
                // Очікуване скасування - ігноруємо
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UDP listener: {ex.Message}");
            }
            finally
            {
                _isListening = false;
            }
        }

        public void StopListening()
        {
            _cts?.Cancel();
            _isListening = false;
        }

        public void Exit()
        {
            StopListening();
            _udpClient?.Close();
            _udpClient?.Dispose();
            _cts?.Dispose();
        }
    }
}
