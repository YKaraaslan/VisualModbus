using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for NotificationItems.xaml
    /// </summary>
    public partial class NotificationItems : UserControl
    {
        public NotificationItems()
        {
            InitializeComponent();
        }


        #region notificationRegion
        private string _notificationContent;
        private string _time;

        public string NotificationText
        {
            get { return _notificationContent; }
            set { _notificationContent = value; notificationContent.Content = value; }
        }

        public string TimeText
        {
            get { return _time; }
            set { _time = value; time.Content = value; }
        }

        #endregion
    }
}
