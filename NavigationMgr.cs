using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace CC3_GUI
{
    public static class NavigationMgr
    {
        public static CretaTypes.EScreenNames CurrentScreen { get; set; }
        public static CretaTypes.EPopoupNames CurrentPopUp { get; set; }

        public delegate void DelegadoCerrarAplicacion();

        public static void LoadScreen(CretaTypes.EScreenNames eScreen,string sParam="")
        {
            if (CurrentScreen == CretaTypes.EScreenNames.SCREEN_MAIN)
                GlobalState.TimerUpdateMgr.StartReturnToMainTimer();
            if (eScreen == CretaTypes.EScreenNames.SCREEN_MAIN)
                GlobalState.TimerUpdateMgr.StopReturnMainTimer();

            GlobalState.MainWindow.Navigate(eScreen,sParam);
            CurrentScreen = eScreen;
        }

        public static void OpenPopup(CretaTypes.EPopoupNames ePopup, string sParam="")
        {
            GlobalState.MainWindow.OpenPopup(ePopup,sParam);
            CurrentPopUp = ePopup;
        }

        public static MessageBoxResult OpenPopupManual(UserControl WindowContent, string sTitle, object pDataContext, int iWidth=1280, int iHeight=1024, bool bShowStyle=false, bool bBlurBg=true)
        {
#if TEST_PC || PLOTTER //Escalo el contenido para poderlo ver en ordenadores con pantallas más pequeñas
            iWidth = (int)App.Current.MainWindow.Width;
            iHeight = (int)App.Current.MainWindow.Height;
            CC3_Utils.ScaleContentToScreenSize(WindowContent);
#endif
            MessageBoxResult MsgRes = CretaBase.PopUpMgr.OpenCretaPopup(WindowContent, sTitle, pDataContext, iWidth, iHeight, bShowStyle, bBlurBg);

            return MsgRes;
        }

        public static void ClosePopup()
        {
            CretaBase.PopUpMgr.CloseCretaPopup();
            CC3_Utils.RegisterAction((int)CretaTypes.EActionsGUI.ACTION_POPUP_CLOSE, CretaTypes.COM_ACTION_POPUP);
            CurrentPopUp = CretaTypes.EPopoupNames.POPUP_INVALID;
        }

        public static void GoToMainScreen()
        {
            LoadScreen(CretaTypes.EScreenNames.SCREEN_MAIN);
        }

        
        public static void CloseApplication(bool optionAvailable, bool turnoffKernel)
        {
            if (optionAvailable)
            {
                if (GlobalState.CretaMessageBox.ShowWarning("CretaBase:Strings:M_WANT_EXIT", MessageBoxButton.YesNo, false) == MessageBoxResult.Yes)
                    GlobalState.MainWindow.CloseApplication();
            }
            else
            {
                if (turnoffKernel)
                {
                    GlobalState.ComHmiMgr.StringWithoutRequest("KTOFF");
                    new System.Threading.Thread(() => WaitTillKernelClosed()).Start();
                    NavigationMgr.OpenPopup(CretaTypes.EPopoupNames.POPUP_WAITING);
                }
            }
        }

        
        private static void WaitTillKernelClosed()
        {
            while (System.Diagnostics.Process.GetProcessesByName("CC3").Length != 0)
            {
                System.Threading.Thread.Sleep(2000);
            }

            DelegadoCerrarAplicacion miDelegadoCerrarAplicacion = new DelegadoCerrarAplicacion(CerrarAplicacion);
            Application.Current.Dispatcher.BeginInvoke(miDelegadoCerrarAplicacion);
        }

        public static void CerrarAplicacion()
        {
            NavigationMgr.ClosePopup();
            GlobalState.MainWindow.CloseApplication();
        }

    }
}
