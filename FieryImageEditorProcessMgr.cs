using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CC3_GUI
{
    public class FieryImageEditorProcessMgr
    {
        private string _sInputFile;
        private string _sOutputFile;
        private string _sLang;
        private System.Threading.Thread _th;
        private bool _bFinished;
        private const string _sProgFilename = @"C:\Program Files (x86)\EFI\Fiery Image Editor\Fiery Image Editor.exe";
        private bool _bProgFound;
        private System.Diagnostics.Process _pProc;

        public FieryImageEditorProcessMgr()
        {
            _sInputFile = "";
            _sOutputFile = "";
            _sLang = "";
            _th = null;
            _bFinished = true;
            _bProgFound = System.IO.File.Exists(_sProgFilename);
        }

        private void InitThread()
        {
            if (_th == null || !_th.IsAlive)
            {
                _bFinished = false;
                _th = new System.Threading.Thread(UpdateThread);
                _th.Name = "FieryImageViewerProcMgr";
                _th.IsBackground = true;
                _th.Start();
            }
        }

        private void UpdateThread()
        {
            _pProc = null;
            try
            {
                // memory usage of the process.
                long peakPagedMem = 0, peakWorkingSet = 0, peakVirtualMem = 0;

                _pProc = new Process();
                _pProc.StartInfo.FileName = _sProgFilename;

                string sArgs = "";
                if (_sInputFile != "")
                    sArgs += "/i \"" + _sInputFile + "\" ";
                if (_sOutputFile != "")
                    sArgs += "/o \"" + _sOutputFile + "\" ";
                if (_sLang != "")
                    sArgs += "/lo \"" + _sLang + "\" ";

                _pProc.StartInfo.Arguments = sArgs;
                _pProc.StartInfo.UseShellExecute = false;
                _pProc.StartInfo.RedirectStandardOutput = true;
                _pProc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                _pProc.Start();

                while (!_bFinished)
                {
                    _bFinished = _pProc.HasExited;
                    if (!_bFinished)
                        _bFinished = _pProc.WaitForExit(100);
                    if (!_bFinished)
                    {
                        // Refresh the current process property values.
                        _pProc.Refresh();

                        Debug.WriteLine("");

                        // Display current process statistics.

                        Debug.WriteLine("{0} -", _pProc.ToString());
                        Debug.WriteLine("-------------------------------------");

                        Debug.WriteLine("  physical memory usage: {0}", _pProc.WorkingSet64);
                        Debug.WriteLine("  base priority: {0}", _pProc.BasePriority);
                        Debug.WriteLine("  priority class: {0}", _pProc.PriorityClass);
                        Debug.WriteLine("  user processor time: {0}", _pProc.UserProcessorTime);
                        Debug.WriteLine("  privileged processor time: {0}", _pProc.PrivilegedProcessorTime);
                        Debug.WriteLine("  total processor time: {0}", _pProc.TotalProcessorTime);
                        Debug.WriteLine("  PagedSystemMemorySize64: {0}", _pProc.PagedSystemMemorySize64);
                        Debug.WriteLine("  PagedMemorySize64: {0}", _pProc.PagedMemorySize64);

                        // Update the values for the overall peak memory statistics.
                        peakPagedMem = _pProc.PeakPagedMemorySize64;
                        peakVirtualMem = _pProc.PeakVirtualMemorySize64;
                        peakWorkingSet = _pProc.PeakWorkingSet64;

                        if (_pProc.Responding)
                        {
                            Debug.WriteLine("Status = Running");
                        }
                        else
                        {
                            Debug.WriteLine("Status = Not Responding");
                        }
                    }
                    System.Threading.Thread.Sleep(900);
                }

                string output = _pProc.StandardOutput.ReadToEnd();
                int iExit = _pProc.ExitCode;

                Debug.WriteLine("Output:");
                Debug.WriteLine(output);

                Debug.WriteLine("");
                Debug.WriteLine("Process exit code: {0}", _pProc.ExitCode);

                // Display peak memory statistics for the process.
                Debug.WriteLine("Peak physical memory usage of the process: {0}", peakWorkingSet);
                Debug.WriteLine("Peak paged memory usage of the process: {0}", peakPagedMem);
                Debug.WriteLine("Peak virtual memory usage of the process: {0}", peakVirtualMem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _bFinished = true;

                _pProc.Dispose();
                //pProc = null;

                _sInputFile = "";
                _sOutputFile = "";
                _sLang = "";
            }
        }

        public void OpenImageEditor(string sInputFile, string sOutputFile, string sLang)
        {
            _sInputFile = sInputFile;
            _sOutputFile = sOutputFile;
            _sLang = GetLangId(sLang);

            InitThread();
        }

        /// <summary>
        /// Devuelve el entero correspondiente al idioma requerido
        /// </summary>
        /// <param name="sLang"></param>
        /// <returns>
        /// wxLANGUAGE_SPANISH - 175
        /// wxLANGUAGE_GERMAN - 89
        /// wxLANGUAGE_FRENCH - 78
        /// wxLANGUAGE_ENGLISH - 56
        /// wxLANGUAGE_CZECH - 52
        /// wxLANGUAGE_ITALIAN - 108
        /// wxLANGUAGE_PORTUGUESE_BRAZILIAN - 151
        /// wxLANGUAGE_RUSSIAN - 156
        /// wxLANGUAGE_TURKISH - 210
        /// wxLANGUAGE_CHINESE_SIMPLIFIED - 44
        /// wxLANGUAGE_POLISH - 149
        /// wxLANGUAGE_DUTCH - 54
        /// wxLANGUAGE_KOREAN - 121
        /// wxLANGUAGE_INDONESIAN - 102
        /// </returns>
        private string GetLangId(string sLang)
        {
            string iLang = "56";

            if (sLang == CretaTypes.STR_SPANISH_LANG_ID)
                iLang = "175";
            else if (sLang == CretaTypes.STR_ENGLISH_LANG_ID)
                iLang = "56";
            else if (sLang == CretaTypes.STR_ITALIAN_LANG_ID)
                iLang = "108";
            else if (sLang == CretaTypes.STR_POLSKI_LANG_ID)
                iLang = "149";
            else if (sLang == CretaTypes.STR_CHINESE_LANG_ID)
                iLang = "44";
            else if (sLang == CretaTypes.STR_CZECK_LANG_ID)
                iLang = "52";
            else if (sLang == CretaTypes.STR_FRENCH_LANG_ID)
                iLang = "78";
            else if (sLang == CretaTypes.STR_TURK_LANG_ID)
                iLang = "210";
            else if (sLang == CretaTypes.STR_RUSSIAN_LANG_ID)
                iLang = "156";
            else if (sLang == CretaTypes.STR_INDONESIAN_LANG_ID)
                iLang = "102";
            else if (sLang == CretaTypes.STR_KOREAN_LANG_ID)
                iLang = "121";
            else if (sLang == CretaTypes.STR_PORTUGUESE_LANG_ID)
                iLang = "151";
            else if (sLang == CretaTypes.STR_GERMAN_LANG_ID)
                iLang = "89";

            return iLang;
        }

        public bool IsProcessRunning()
        {
            bool bRunning = !_bFinished;

            return bRunning;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;
        private const UInt32 SC_MINIMIZE = 0xF020;
        private const UInt32 SC_MAXIMIZE = 0xF030;
        public bool MaximizeAndBringToFront()
        {
            bool bOk = false;
            
            if (_th != null && !_bFinished)
            {
                IntPtr winHandle = _pProc.MainWindowHandle;
                //Traigo al frente
                SetForegroundWindow(winHandle);
                //Maximizo la ventana
                SendMessage(winHandle, WM_SYSCOMMAND, new IntPtr(SC_MAXIMIZE), new IntPtr(0));

                bOk = true;
            }

            return bOk;
        }

        /// <summary>
        /// Devuelve si se ha encontrado el ejecutable
        /// </summary>
        /// <returns></returns>
        public bool IsTheProgramInstalled()
        {
            return _bProgFound;
        }
    }
}
