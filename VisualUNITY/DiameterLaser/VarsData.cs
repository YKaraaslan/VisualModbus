using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualUNITY.DiameterLast
{
    class VarsData
    {
        private static double diameter, diameter2, diameterSet, diameterDifference, plusTolerance, plusTolerance2; 
        private static double minusTolerance, minusTolerance2, xAxis, yAxis, length, display3;
        private static double nc, even, parp, pari;

        public static double Diameter { get => diameter; set => diameter = value; }
        public static double Diameter2 { get => diameter2; set => diameter2 = value; }
        public static double DiameterSet { get => diameterSet; set => diameterSet = value; }
        public static double DiameterDifference { get => diameterDifference; set => diameterDifference = value; }
        public static double PlusTolerance { get => plusTolerance; set => plusTolerance = value; }
        public static double PlusTolerance2 { get => plusTolerance2; set => plusTolerance2 = value; }
        public static double MinusTolerance { get => minusTolerance; set => minusTolerance = value; }
        public static double MinusTolerance2 { get => minusTolerance2; set => minusTolerance2 = value; }
        public static double XAxis { get => xAxis; set => xAxis = value; }
        public static double YAxis { get => yAxis; set => yAxis = value; }
        public static double Length { get => length; set => length = value; }
        public static double Display3 { get => display3; set => display3 = value; }
        public static double Nc { get => nc; set => nc = value; }
        public static double Even { get => even; set => even = value; }
        public static double Parp { get => parp; set => parp = value; }
        public static double Pari { get => pari; set => pari = value; }
    }
}
