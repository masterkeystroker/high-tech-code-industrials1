using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public clsTest classTest { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            classTest = new clsTest();

            classTest.TestColorA = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            //TestColor22.Color = Color.FromArgb(255,0,255,0);
            classTest.StrTestA = "- test -";
        }

        private SolidColorBrush _testColor;
        public SolidColorBrush TestColor22 
        {
            get { return _testColor; }
            set
            {
                _testColor = value;
            }
        }
        public Color TestColor2 { get; set; }
        private string _strTest;
        public string StrTest
        {
            get 
            { 
                return _strTest;
            }
            set
            {
                _strTest = value;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class clsTest : ViewModelBase
    {
        private SolidColorBrush _testColor;
        public SolidColorBrush TestColorA
        {
            get 
            { 
                return _testColor; 
            }
            set
            {
                _testColor = value;
                NotifyPropertyChanged("TestColor22");
            }
        }
        public Color TestColor2 { get; set; }
        private string _strTest;
        public string StrTestA
        {
            get
            {
                return _strTest;
            }
            set
            {
                _strTest = value;
                NotifyPropertyChanged("StrTest");
            }
        }
    }
}
