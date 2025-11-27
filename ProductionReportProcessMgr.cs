using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CC3_GUI
{
    public class ProductionReportProcessMgr
    {
        private string _sLang;
        private System.Threading.Thread _th;
        private bool _bFinished;
        private const string PRODUCTION_REPORT_FILENAME = "\\ProductionReport\\Informes_Produccion.exe";
        public string sProdReportPath = "";
        private bool _bProgFound = false;
        private System.Diagnostics.Process _pProc;

        public ProductionReportProcessMgr()
        {
            _sLang = "";
            _th = null;
            _bFinished = true;

            sProdReportPath = "..\\" + PRODUCTION_REPORT_FILENAME;
            //Si la ruta de origen no esta bien entonces cargo la ruta como toca
            if (!System.IO.File.Exists(sProdReportPath))
            {
                sProdReportPath = "C:\\Program Files\\Efi Cretaprint\\" + PRODUCTION_REPORT_FILENAME;
                if (System.IO.File.Exists(sProdReportPath))
                {
                    _bProgFound = true;
                }
            }
            else
                _bProgFound = true;
        }

        private void InitThread()
        {
            if (_th == null || !_th.IsAlive)
            {
                _bFinished = false;
                _th = new System.Threading.Thread(UpdateThread);
                _th.Name = "ProductionReportViewerProcMgr";
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
                _pProc.StartInfo.FileName = sProdReportPath;

                string sArgs = "";
                //if (_sInputFile != "")
                //    sArgs += "/i \"" + _sInputFile + "\" ";
                //if (_sOutputFile != "")
                //    sArgs += "/o \"" + _sOutputFile + "\" ";
                if (_sLang != "")
                    sArgs += "/lang \"" + _sLang + "\" ";

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

                _sLang = "";
            }
        }

        public void OpenProductionReport(string sLang)
        {
            _sLang = sLang;

            InitThread();
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
