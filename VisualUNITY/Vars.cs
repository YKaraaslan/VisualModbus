using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualUNITY
{
    class Vars
    {
        public static string selectedUserControl;
        private static bool justSignedOut, isAdmin;

        public static bool JustSignedOut { get => justSignedOut; set => justSignedOut = value; }
        public static bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public static string SelectedUserControl { get => selectedUserControl; set => selectedUserControl = value; }
    }
}
