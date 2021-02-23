using ColorPickerWPF;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VisualUNITY.DiameterLast
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        SnackbarMessageQueue myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        Assembly assembly;
        ResourceManager rm;
        CultureInfo cultureInfo;

        public Settings()
        {
            InitializeComponent();
            assembly = Assembly.Load("VisualUNITY");
            rm = new ResourceManager("VisualUNITY.Languages.language", assembly);
            cultureInfo = new CultureInfo(Properties.Settings.Default.language);
            Snackbar.MessageQueue = myMessageQueue;
            prepare();
        }

        private void prepare()
        {
            diameterCheck.IsChecked = Properties.Settings.Default.diameterCheck;
            diameterSetCheck.IsChecked = Properties.Settings.Default.diameterSetCheck;
            diameterDifferenceCheck.IsChecked = Properties.Settings.Default.diameterDifferenceCheck;
            plusToleranceCheck.IsChecked = Properties.Settings.Default.plusToleranceCheck;
            minusToleranceCheck.IsChecked = Properties.Settings.Default.minusToleranceCheck;
            display3Check.IsChecked = Properties.Settings.Default.display3Check;
            ncCheck.IsChecked = Properties.Settings.Default.ncCheck;
            evenCheck.IsChecked = Properties.Settings.Default.evenCheck;
            parpCheck.IsChecked = Properties.Settings.Default.parpCheck;
            pariCheck.IsChecked = Properties.Settings.Default.pariCheck;
            diameter2.IsChecked = Properties.Settings.Default.diameter2;
            plusTolerance2.IsChecked = Properties.Settings.Default.plusTolerance2;
            minusTolerance2.IsChecked = Properties.Settings.Default.minusTolerance2;
            xAxis.IsChecked = Properties.Settings.Default.xAxis;
            yAxis.IsChecked = Properties.Settings.Default.yAxis;

            if (Properties.Settings.Default.language == "tr-TR")
                turkish.IsChecked = true;

            if (Properties.Settings.Default.language == "en-EN")
                english.IsChecked = true;

            if (Properties.Settings.Default.language == "ar-AR")
                arabic.IsChecked = true;

            upDownDevice.Value = Properties.Settings.Default.deviceAmount;
            upDownOperator.Value = Properties.Settings.Default.operatorAmount;
        }

        private void Flipper_OnIsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (Properties.Settings.Default.deviceAmount != (int)upDownDevice.Value)
            {
                if (MessageBox.Show(rm.GetString("deviceWarning"), rm.GetString("system"), MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    save();
                    return;
                }
                return;
            }
            save();
        }

        private void save()
        {
            Properties.Settings.Default.diameterCheck = (bool)diameterCheck.IsChecked;
            Properties.Settings.Default.diameterSetCheck = (bool)diameterSetCheck.IsChecked;
            Properties.Settings.Default.diameterDifferenceCheck = (bool)diameterDifferenceCheck.IsChecked;
            Properties.Settings.Default.plusToleranceCheck = (bool)plusToleranceCheck.IsChecked;
            Properties.Settings.Default.minusToleranceCheck = (bool)minusToleranceCheck.IsChecked;
            Properties.Settings.Default.display3Check = (bool)display3Check.IsChecked;
            Properties.Settings.Default.ncCheck = (bool)ncCheck.IsChecked;
            Properties.Settings.Default.evenCheck = (bool)evenCheck.IsChecked;
            Properties.Settings.Default.parpCheck = (bool)parpCheck.IsChecked;
            Properties.Settings.Default.pariCheck = (bool)pariCheck.IsChecked;
            Properties.Settings.Default.diameter2 = (bool)diameter2.IsChecked;
            Properties.Settings.Default.plusTolerance2 = (bool)plusTolerance2.IsChecked;
            Properties.Settings.Default.minusTolerance2 = (bool)minusTolerance2.IsChecked;
            Properties.Settings.Default.xAxis = (bool)xAxis.IsChecked;
            Properties.Settings.Default.yAxis = (bool)yAxis.IsChecked;

            if (turkish.IsChecked == true)
            {
                cultureInfo = new CultureInfo("tr-TR");
                Properties.Settings.Default.language = "tr-TR";
                Properties.Settings.Default.Save();
            }
            else if (english.IsChecked == true)
            {
                cultureInfo = new CultureInfo("en-EN");
                Properties.Settings.Default.language = "en-EN";
                Properties.Settings.Default.Save();
            }
            else if (arabic.IsChecked == true)
            {
                cultureInfo = new CultureInfo("ar-AR");
                Properties.Settings.Default.language = "ar-AR";
                Properties.Settings.Default.Save();
            }

            Properties.Settings.Default.deviceAmount = (int)upDownDevice.Value;
            Properties.Settings.Default.operatorAmount = (int)upDownOperator.Value;

            myMessageQueue.Enqueue(rm.GetString("settingsSaved", cultureInfo), rm.GetString("ok"),
                () => HandleOKMethod());
        }

        private void HandleOKMethod()
        {
            this.Close();
        }

        private void turkish_Checked(object sender, RoutedEventArgs e)
        {
            english.IsChecked = false;
            arabic.IsChecked = false;
        }

        private void english_Checked(object sender, RoutedEventArgs e)
        {
            turkish.IsChecked = false;
            arabic.IsChecked = false;
        }

        private void arabic_Checked(object sender, RoutedEventArgs e)
        {
            english.IsChecked = false;
            turkish.IsChecked = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
