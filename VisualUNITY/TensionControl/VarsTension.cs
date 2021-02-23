using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualUNITY.TensionControl
{
    class VarsTension
    {
        public static ModbusClient client;
        public static string selectedUserControl;

        public static ModbusClient Client { get => client; set => client = value; }
        public static string SelectedUserControl { get => selectedUserControl; set => selectedUserControl = value; }
    }
}
