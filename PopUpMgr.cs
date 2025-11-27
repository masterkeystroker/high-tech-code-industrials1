using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace CC3_GUI
{
    public class PopUpMgr
    {
        private Window _window;
        /// <summary>
        /// Open a new PopoUp with a UserControl as content
        /// </summary>
        /// <param name="WindowContent">The user control to open</param>
        /// <param name="sTitle">The Window Title</param>
        /// <param name="iWidth">Width Size</param>
        /// <param name="iHeight">Height Size</param>
        public void OpenCretaPopup(UserControl WindowContent, string sTitle = "", object pDataContext = null, int iWidth = CretaTypes.SCREEN_WIDTH, int iHeight = CretaTypes.SCREEN_HEIGHT)
        {
            if (_window != null)
                CloseCretaPopup();

            _window = new Window
            {
                Title = sTitle,
                Content = WindowContent,
                Width = iWidth,
                Height = iHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                //ShowInTaskbar = false,
                Background = System.Windows.Media.Brushes.Transparent,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //ShowInTaskbar = false,
            };

            var blur = new BlurEffect();
            blur.Radius = 5;
            App.Current.MainWindow.Effect = blur;
            App.Current.MainWindow.Opacity = 0.75;

            if (pDataContext != null)
            {
                _window.DataContext = pDataContext;
            }
            else
            {
                _window.DataContext = App.Current.MainWindow.DataContext;
            }

            GlobalState.MainWindow.PauseAllAnimations(true);

            Nullable<bool> dialogResult = _window.ShowDialog();
            if(dialogResult!=null)
                ClearPopUp();
        }

        /// <summary>
        /// Close and Returns the window to normal state (clearing the effects)
        /// </summary>
        public void CloseCretaPopup(UserControl pContent=null)
        {
            if (_window != null)
            {
                //Closes the window that contents the userControl
                Window parentwin = Window.GetWindow(_window);
                if (parentwin != null)
                {
                    parentwin.Close();
                    GlobalState.MainWindow.PauseAllAnimations(false);
                }
            }
        }

        private void ClearPopUp()
        {
            //Clears the effect applied in main window
            Window MainW = App.Current.MainWindow;
            MainW.Effect = null;
            App.Current.MainWindow.Opacity = 1.0;

            _window = null;
        }
    }
}
