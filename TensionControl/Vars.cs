using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensionControl
{
    class Vars
    {
        public static ModbusClient client;

        public static ModbusClient Client { get => client; set => client = value; }
    }
}
