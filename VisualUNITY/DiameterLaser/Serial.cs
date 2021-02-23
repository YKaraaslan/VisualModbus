using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualUNITY.DiameterLast
{
    class Serial
    {
        private static SerialPort port;

        public static SerialPort Port { get => port; set => port = value; }
    }
}
