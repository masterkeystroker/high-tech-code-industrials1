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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace CC3_Mgr
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window _window = null;
        public MainWindow()
        {
            this.Hide();
            this.ShowInTaskbar = false;
            string[] args = Environment.GetCommandLineArgs();
            String Arguments = "";
            for (int i = 1; i < args.Length; i++ )
            {
                Arguments = Arguments + " " + args[i];
            }

            _window = new Window
            {
                Title = "CC3 OS",
                Content = new LoadingScreen(),
                Width = 630,
                Height = 225,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                //ShowInTaskbar = false,
                Background = System.Windows.Media.Brushes.Transparent,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
            };
            _window.Show();
            _window.Topmost = true;

            InitializeComponent();

            StartPrograms(Arguments);            
        }

        private const int SLEEP_TIME_SECONDS = 5;
#if DEBUG
        private const string CC3_KERNEL_PATH = "C:\\Desarrollo\\Kernel\\Implementacion\\CC3\\CC3\\bin\\Release\\CC3.exe";
        private const string CC3_GUI_PATH = "C:\\Desarrollo\\CC3\\CC3_GUI\\CC3_GUI\\bin\\Debug\\CC3_GUI.exe";
#else
        private const string CC3_KERNEL_PATH = "..\\Kernel\\CC3.exe";
        private const string CC3_GUI_PATH = "..\\GUI\\CC3_GUI.exe";
#endif
        Process CC3_Kernel = null;
        Process CC3_GUI = null;
        
        private void StartPrograms(String Arguments)
        {
            bool bKernelRunning = false;
            bool bGUIRunning = false;
            bool bFound = false;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SLEEP_TIME_SECONDS));
            Process[] processArr = Process.GetProcesses();
            Process KernelProc = null;
            Process GUIProc = null;
            for (int i = 0; i < processArr.Length && !bFound; ++i)
            {
                string sName = processArr[i].ProcessName;

                if (sName == "CC3")
                {
                    bKernelRunning = true;
                    KernelProc = processArr[i];
                }
                if (sName == "CC3_GUI")
                {
                    bGUIRunning = true;
                    GUIProc = processArr[i];
                }
                bFound = bGUIRunning && bKernelRunning;
            }
            if (!System.IO.File.Exists(CC3_KERNEL_PATH) || !System.IO.File.Exists(CC3_GUI_PATH))
            {
                //MessageBox.Show("Error Loading");
                string sMsg = "";
                if (!System.IO.File.Exists(CC3_KERNEL_PATH))
                    sMsg += string.Format("Program '" + CC3_KERNEL_PATH + "' not found.\n");
                if (!System.IO.File.Exists(CC3_GUI_PATH))
                    sMsg += string.Format("Program '" + CC3_GUI_PATH + "' not found.\n");

                if(sMsg!="")
                    MessageBox.Show(sMsg);
            }
            try
            {
                bool bOk = false;
                //Si el kernel está abierto pero no comunica puede ser por dos cosas, porque aún está cerrándose o porque se ha quedado 'pajarito'
                //  en cualquiera de los dos casos debería mostrarse un mensaje explicándolo y preguntar si quiere volver a intentarlo o matar el proceso anterior
                if (bKernelRunning)
                {
                    bool bCanFollow = ConnectionOk();
                    if (!bCanFollow)
                    {
                        //MessageBoxResult res = MessageBox.Show("El kernel aún está abierto. ¿Quiere forzar el cierre?", "Test", MessageBoxButton.YesNo);
                        //if (res == MessageBoxResult.Yes)
                        {
                            if (!KernelProc.HasExited)
                            {
                                KernelProc.Kill();
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SLEEP_TIME_SECONDS));
                            }
                            bKernelRunning = false;
                        }
                    }
                }
                //Opens kernel
                if (!bKernelRunning && System.IO.File.Exists(CC3_KERNEL_PATH)) //Comprobar, además de si está el proceso si se puede comunicar
                {
                    CC3_Kernel = new Process();
                    CC3_Kernel.StartInfo.FileName = CC3_KERNEL_PATH;
                    CC3_Kernel.StartInfo.Arguments = Arguments;
                    bOk = CC3_Kernel.Start();
                    this.Topmost = true;
                    //Si abre intento conectar para saber si puedo abrir el interfaz (si puedo conectar es que ha terminado de cargar)
                    if (bOk)
                    {
                        const int TRY_TO_CONNECT_TIMEOUT = 5;//Minutos
                        bool bConnected = false;
                        DateTime datStart = DateTime.Now;
                        TimeSpan elapsedTime = DateTime.Now.Subtract(datStart);
                        while (!bConnected && elapsedTime.TotalMinutes < TRY_TO_CONNECT_TIMEOUT)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SLEEP_TIME_SECONDS));
                            elapsedTime = DateTime.Now.Subtract(datStart);

                            bConnected = ConnectionOk();
                        }
                    }

                    CC3_Kernel.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    
                    //Si el kernel estaba cerrado y el interfaz no, entonces fuerzo el cierre del interfaz para abrirlo más adelante
                    if (bGUIRunning)
                    {
                        if (!GUIProc.HasExited)
                        {
                            GUIProc.Kill();
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SLEEP_TIME_SECONDS));
                        }
                        bGUIRunning = false;
                    }
                }

                if (!bGUIRunning && System.IO.File.Exists(CC3_GUI_PATH))
                {
                    //Opens GUI
                    CC3_GUI = new Process();
                    CC3_GUI.StartInfo.FileName = CC3_GUI_PATH;

                    bOk = CC3_GUI.Start();
                    this.Topmost = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir los programas");
#if DEBUG
                string sMsg = ex.Message;
                string sCallStack = ex.StackTrace;
                WriteLogEvent(sMsg + " - " + sCallStack);
#endif
                this.Close();
            }
            _window.Close();
            this.Close();
            CC3_Kernel = null;
            CC3_GUI = null;
        }
        //Comprueba la conexión al kernel
        private bool ConnectionOk()
        {
            Creta.libcomhmi.clsClientTCP comHmi = new Creta.libcomhmi.clsClientTCP("localhost", 32000);
            return comHmi.Conectado;
        }

        /// <summary>
        /// Escribe mensajes de log en el archivo indicado, si no se especifica archivo se guarda en la ruta especificada en la propiedad LogsFilePath
        /// </summary>
        /// <param name="sMessage">Mensaje a mostrar</param>
        /// <param name="sFile">Ruta del archivo de salida, si no se especifica se escribirá en un con la fecha actual en la ruta especificada</param>
        public void WriteLogEvent(string sMessage, string sFile = "")
        {
            DateTime dt = DateTime.Now;
            if (sFile == "")
            {
                const string GENERAL_STRING_PATH = "\\Cretaprint\\CC3\\";
                string sProgDataFolder = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");

                //GUI Logs Directory
                sFile = sProgDataFolder + GENERAL_STRING_PATH + "LogsGui\\LogMgr_" + dt.Year + "_" + dt.Month + "_" + dt.Day + ".txt";
            }
            IFormatProvider culture = new System.Globalization.CultureInfo("es-ES", true);
            sMessage = dt.ToString(culture) + ";" + sMessage;
            sMessage = sMessage.Replace(System.Environment.NewLine, " ");
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(sFile, true))
                {
                    file.WriteLine(sMessage);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Windows.MessageBox.Show(string.Format("Error creating Log entry at file {0}", sFile));
#endif
                Console.WriteLine("Excepción al crear una entrada en el log: {0}", ex.Message);
            }
        }

        #region Manage Processes
        /*
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
        //Pillar la ventana activa
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        //Para ver los parámetros: http://msdn.microsoft.com/en-us/library/windows/desktop/ms646360%28v=vs.85%29.aspx
        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;
        private const UInt32 SC_MINIMIZE = 0xF020;
        private const UInt32 SC_MOVE = 0xF010;
        private const UInt32 SC_SIZE = 0xF000;
        private const UInt32 SC_NEXTWINDOW = 0xF040;
        private const UInt32 SC_PREVWINDOW = 0xF050;
        //Nuevos flags
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        //
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private string OnScreenKeyboadApplication = "osk.exe";
        private bool _isMaximised;
        private void ManageProcesses()
        {
            // Get the name of the On screen keyboard
            string processName = System.IO.Path.GetFileNameWithoutExtension(OnScreenKeyboadApplication);

            // Check whether the application is not running 
            var query = from process in Process.GetProcesses()
                        where process.ProcessName == processName
                        select process;

            var keyboardProcess = query.FirstOrDefault();

            // launch it if it doesn't exist
            if (keyboardProcess == null)
            {
                IntPtr ptr = new IntPtr(); ;
                bool sucessfullyDisabledWow64Redirect = false;

                // Disable x64 directory virtualization if we're on x64,
                // otherwise keyboard launch will fail.
                if (System.Environment.Is64BitOperatingSystem)
                {
                    sucessfullyDisabledWow64Redirect = Wow64DisableWow64FsRedirection(ref ptr);
                }

                // osk.exe is in windows/system folder. So we can directky call it without path
                using (Process osk = new Process())
                {
                    osk.StartInfo.FileName = OnScreenKeyboadApplication;
                    osk.Start();

                    _isMaximised = true;
                    //osk.WaitForInputIdle(2000);
                }

                // Re-enable directory virtualisation if it was disabled.
                if (System.Environment.Is64BitOperatingSystem)
                    if (sucessfullyDisabledWow64Redirect)
                        Wow64RevertWow64FsRedirection(ptr);

                Process process = Process.GetCurrentProcess();
                IntPtr iMainWindow = process.MainWindowHandle;
                SetForegroundWindow(iMainWindow);
            }
            else
            {
                var windowHandle = keyboardProcess.MainWindowHandle;
                if (!_isMaximised)
                {                   
                    //IntPtr focused = GetForegroundWindow();
                    //string sTitle = GetActiveWindowTitle();
                    //test1.Text = sTitle;
                    // Bring keyboard to the front if it's already running
                    SendMessage(windowHandle, WM_SYSCOMMAND, new IntPtr(SC_RESTORE), new IntPtr(0));
                    _isMaximised = true;
                                        
                    //sTitle = GetActiveWindowTitle();
                    //test2.Text = sTitle;
                    
                    Process process = Process.GetCurrentProcess();
                    IntPtr iMainWindow = process.MainWindowHandle;
                    SetForegroundWindow(iMainWindow);

                    //sTitle = GetActiveWindowTitle();
                    //test3.Text = sTitle;
                }
                else
                {
                    //If is in front, then minimize
                    SendMessage(windowHandle, WM_SYSCOMMAND, new IntPtr(SC_MINIMIZE), new IntPtr(0));
                    _isMaximised = false;
                }
            }
        }
         * */
        #endregion

        #region Notify Icon
/*
        private WindowState lastWindowState;
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.lastWindowState = WindowState;
			this.Hide();
		}

		protected override void OnStateChanged(EventArgs e)
		{
			if (this.WindowState == WindowState.Minimized)
			{
				this.Hide();
			}
			else
			{
				this.lastWindowState = this.WindowState;
			}
		}

		private void OnVisibilityClick(object sender, RoutedEventArgs e)
		{
			this.notifyIcon.Visibility = this.notifyIcon.Visibility == Visibility.Visible ?
				Visibility.Collapsed : Visibility.Visible;
		}

		private void OnBalloonClick(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(this.notifyIcon.BalloonTipText))
			{
				this.notifyIcon.ShowBalloonTip(2000);
			}
		}

		private void OnNotifyIconDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.Show();
				this.WindowState = this.lastWindowState;
			}
		}

		private void OnOpenClick(object sender, RoutedEventArgs e)
		{
			this.Show();
			this.WindowState = this.lastWindowState;
		}

		private void OnExitClick(object sender, RoutedEventArgs e)
		{
			this.Close();
        }*/
        #endregion
    }
}
