using System;
using CretaBase;
using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace CC3_GUI
{
    public class LoadingDataMgr : ViewModelBase
    {
        #region Members
        private Window _window;
        private int _iTimeToClose;
        private Thread _thWindow = null;
        private bool _bStopped;
        #endregion

        public LoadingDataMgr()
        {
            _window = null;            
        }

        /// <summary>
        /// Se genera un popup en primer plano (en segundo plano sigue funcionando todo) 
        /// con una barra de carga que aumenta proporcionalmente al tiempo transcurrido, al finalizar el tiempo se cierra.
        /// </summary>
        /// <param name="iSecs"></param>
        public void StartWithTime(int iSecs)
        {
            Start(iSecs);
        }
        /// <summary>
        /// Se genera un popup en primer plano (la aplicación sigue funcionando atrás) con una barra de carga indeterminada.
        /// No desaparecerá hasta que se invoque la función Stop()
        /// </summary>
        /// <param name="iSecs"></param>
        public void StartIndeterminate()
        {
            Start(-1);
        }
        private void Start(int iSecs)
        {
            if (_thWindow!= null && _thWindow.IsAlive)
            {
                _thWindow = null;
            }
            _iTimeToClose = iSecs;
            _bStopped = false;
            OpenWindowWithControl(iSecs);
        }
        private void OpenWindowWithControl(int iSecs)
        {
            _thWindow = new Thread(() =>
            {
                _window = new Window
                {
                    //Title = sTitle,
                    Width = CretaTypes.SCREEN_WIDTH,
                    Height = CretaTypes.SCREEN_HEIGHT,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    AllowsTransparency = true,
                    //ShowInTaskbar = false,
                    Background = System.Windows.Media.Brushes.Transparent,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    //ShowInTaskbar = false,
                };
                _window.Content = new Managers.LoadingDataControl(iSecs, _window);

                //Nullable<bool> dialogResult = _window.ShowDialog();
                //if (dialogResult != null)
                //    Stop();
                _window.Show();

                _window.Closed += (sender2, e2) => Stop();
                System.Windows.Threading.Dispatcher.Run();
            });
            _thWindow.SetApartmentState(ApartmentState.STA);
            _thWindow.Name = "LoadingDataMgr_th";
            _thWindow.Start();
        }

        public void Stop()
        {
            if (!_bStopped)
            {
                _bStopped = true;
                CloseLoading();
            }
        }
        private void CloseLoading()
        {
            if (_window != null)
            {
                //_window.Content = null;
                _window.Dispatcher.InvokeShutdown();
                //_window.Close();
                _window = null;
            }
            if (_thWindow!=null)
            {
                //_thWindow.Abort();
                _thWindow = null;
            }
        }
    }
}
