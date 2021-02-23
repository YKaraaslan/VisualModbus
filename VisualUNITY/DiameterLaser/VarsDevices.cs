using Modbus.Device;
using System.IO.Ports;

namespace VisualUNITY.DiameterLast
{
    class VarsDevices
    {
        public static int deviceWorking, deviceProjectReady;
        private static SerialPort port;
        private static IModbusSerialMaster master;
        public static string device1DbName, device2DbName, device3DbName, device4DbName, device5DbName;
        public static string device6DbName, device7DbName, device8DbName, device9DbName, device10DbName;
        public static string device11DbName, device12DbName, device13DbName, device14DbName;

        public static string device1Name, device2Name, device3Name, device4Name, device5Name;
        public static string device6Name, device7Name, device8Name, device9Name, device10Name;
        public static string device11Name, device12Name, device13Name, device14Name;

        public static bool device1Connected, device2Connected, device3Connected, device4Connected, device5Connected;
        public static bool device6Connected, device7Connected, device8Connected, device9Connected, device10Connected;
        public static bool device11Connected, device12Connected, device13Connected, device14Connected;

        public static int DeviceWorking { get => deviceWorking; set => deviceWorking = value; }
        public static int DeviceProjectReady { get => deviceProjectReady; set => deviceProjectReady = value; }
    
        public static string Device1DbName { get => device1DbName; set => device1DbName = value; }
        public static string Device2DbName { get => device2DbName; set => device2DbName = value; }
        public static string Device3DbName { get => device3DbName; set => device3DbName = value; }
        public static string Device4DbName { get => device4DbName; set => device4DbName = value; }
        public static string Device5DbName { get => device5DbName; set => device5DbName = value; }
        public static string Device6DbName { get => device6DbName; set => device6DbName = value; }
        public static string Device7DbName { get => device7DbName; set => device7DbName = value; }
        public static string Device8DbName { get => device8DbName; set => device8DbName = value; }
        public static string Device9DbName { get => device9DbName; set => device9DbName = value; }
        public static string Device10DbName { get => device10DbName; set => device10DbName = value; }
        public static string Device11DbName { get => device11DbName; set => device11DbName = value; }
        public static string Device12DbName { get => device12DbName; set => device12DbName = value; }
        public static string Device13DbName { get => device13DbName; set => device13DbName = value; }
        public static string Device14DbName { get => device14DbName; set => device14DbName = value; }

        public static string Device1Name { get => device1Name; set => device1Name = value; }
        public static string Device2Name { get => device2Name; set => device2Name = value; }
        public static string Device3Name { get => device3Name; set => device3Name = value; }
        public static string Device4Name { get => device4Name; set => device4Name = value; }
        public static string Device5Name { get => device5Name; set => device5Name = value; }
        public static string Device6Name { get => device6Name; set => device6Name = value; }
        public static string Device7Name { get => device7Name; set => device7Name = value; }
        public static string Device8Name { get => device8Name; set => device8Name = value; }
        public static string Device9Name { get => device9Name; set => device9Name = value; }
        public static string Device10Name { get => device10Name; set => device10Name = value; }
        public static string Device11Name { get => device11Name; set => device11Name = value; }
        public static string Device12Name { get => device12Name; set => device12Name = value; }
        public static string Device13Name { get => device13Name; set => device13Name = value; }
        public static string Device14Name { get => device14Name; set => device14Name = value; }


        public static bool Device1Connected { get => device1Connected; set => device1Connected = value; }
        public static bool Device2Connected { get => device2Connected; set => device2Connected = value; }
        public static bool Device3Connected { get => device3Connected; set => device3Connected = value; }
        public static bool Device4Connected { get => device4Connected; set => device4Connected = value; }
        public static bool Device5Connected { get => device5Connected; set => device5Connected = value; }
        public static bool Device6Connected { get => device6Connected; set => device6Connected = value; }
        public static bool Device7Connected { get => device7Connected; set => device7Connected = value; }
        public static bool Device8Connected { get => device8Connected; set => device8Connected = value; }
        public static bool Device9Connected { get => device9Connected; set => device9Connected = value; }
        public static bool Device10Connected { get => device10Connected; set => device10Connected = value; }
        public static bool Device11Connected { get => device11Connected; set => device11Connected = value; }
        public static bool Device12Connected { get => device12Connected; set => device12Connected = value; }
        public static bool Device13Connected { get => device13Connected; set => device13Connected = value; }
        public static bool Device14Connected { get => device14Connected; set => device14Connected = value; }
        public static SerialPort Port { get => port; set => port = value; }
        public static IModbusSerialMaster Master { get => master; set => master = value; }
    }
}
