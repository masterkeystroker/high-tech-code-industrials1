using System;
using CretaBase;
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
using System.Windows.Threading;

namespace CC3_GUI.Managers
{
    /// <summary>
    /// Lógica de interacción para LoadingDataControl.xaml
    /// </summary>
    public partial class LoadingDataControl : UserControl
    {
        private int _iTimeToClose;
        private Window _win;
        private DispatcherTimer _dispatcherTimer;
        private DateTime _datStart;
        private bool _bIndeterminate;

        public LoadingDataControl(int iTimeToClose, Window win)
        {
            InitializeComponent();

            _iTimeToClose = iTimeToClose;
            _bIndeterminate = (iTimeToClose == -1);
            LoadingBar.IsIndeterminate = _bIndeterminate;
            
            _win = win;
            
            if (!_bIndeterminate)
            {
                _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(Update);
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                _datStart = DateTime.Now;
                _dispatcherTimer.Start();
            }
        }

        public void Close()
        {
            _dispatcherTimer.Stop();
            _iTimeToClose = 0;
            _win.Close();
        }
        
        public void Update(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = DateTime.Now.Subtract(_datStart);
            LoadingBar.Value = CretaUtils.CretaClamp(elapsedTime.Seconds * 100 / _iTimeToClose, 0, 100);
            if (elapsedTime.Seconds >= _iTimeToClose)
            {
                Close();
            }
        }
    }
}
