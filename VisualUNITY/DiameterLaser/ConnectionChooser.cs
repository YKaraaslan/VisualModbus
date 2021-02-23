using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VisualUNITY.DiameterLast
{
    class ConnectionChooser
    {
        public static int parityChooser(string parityChosen)
        {
            switch (parityChosen)
            {
                case "Even":
                    return 0;

                case "Mark":
                    return 1;

                case "None":
                    return 2;

                case "Odd":
                    return 3;

                case "Space":
                    return 4;

                default:
                    return 2;
            }
        }

        public static int stopBitChooser(string stopBitChosen)
        {
            switch (stopBitChosen)
            {
                case "None":
                    return 0;

                case "1":
                    return 1;

                case "1.5":
                    return 2;

                case "2":
                    return 3;

                default:
                    return 1;
            }
        }
    }
}
