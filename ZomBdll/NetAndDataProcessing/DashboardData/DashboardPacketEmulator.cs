﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace System451.Communication.Dashboard.Net
{
    /// <summary>
    /// Emulates the cRIO in sending pre-determined packets to test ZomB. Sends to localhost
    /// </summary>
    public class DashboardPacketEmulator
    {
        byte[][] pkts;
        int i = 0;
        Thread t;
        UdpClient uc;

        /// <summary>
        /// Create a new V-cRIO
        /// </summary>
        public DashboardPacketEmulator()
        {
            pkts = new byte[][] { };
        }

        /// <summary>
        /// Kill the V-cRIO
        /// </summary>
        ~DashboardPacketEmulator()
        {
            Stop();
        }

        /// <summary>
        /// Gets or sets the array of byte[1018] packets to send
        /// </summary>
        public byte[][] Packets
        {
            get
            {
                return pkts;
            }
            set
            {
                if (pkts == null)
                    pkts = new byte[][] { };
                lock (pkts)
                {
                    pkts = value;
                    i = 0;
                }
            }
        }

        delegate void eh();

        /// <summary>
        /// Starts the packet sending to localhost
        /// </summary>
        public void Start()
        {
            if (Packets != null)
            {
                t = new Thread(dowork);
                t.IsBackground = true;
                t.Start();
            }
        }

        void dowork()
        {
            uc = new UdpClient();
            i = 0;
            while (true)
            {
                Thread.Sleep(20);
                lock (pkts)
                {
                    if (i >= Packets.Length)
                        i = 0;
                    uc.Send(Packets[i], 1018, new IPEndPoint(IPAddress.Loopback, 1165));
                    i++;
                }
            }
        }

        /// <summary>
        /// Stop packet sending
        /// </summary>
        public void Stop()
        {
            try
            {
                t.Abort();
            }
            catch { }
        }
    }
}