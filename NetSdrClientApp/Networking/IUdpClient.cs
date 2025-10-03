﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NetSdrClientApp.Networking;

namespace NetSdrClientApp.Networking
{﻿
    public interface IUdpClient
    {
        event EventHandler<byte[]>? MessageReceived;
    
        Task StartListeningAsync();
    
        void StopListening();
        void Exit();
    }
}
