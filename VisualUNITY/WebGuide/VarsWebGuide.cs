using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualUNITY.WebGuide
{
    class VarsWebGuide
    {
        public static ModbusClient client;

        public static ModbusClient Client { get => client; set => client = value; }

        public string sender { get; set; }
        public string senderMail { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public string date { get; set; }
    }
}
