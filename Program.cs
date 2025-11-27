using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace CretaPerformanceMonitor
{
    class Program
    {
        static TestPerformance _testPerf = null;
        static string _sLogsPathDir = "";
        static bool _bExtendedMode = false;
        static bool _bSilentMode = false;
        static void Main(string[] args)
        {
            try
            {
                //Si existe algún argumento con /s es para lanzar la aplicación en modo silencioso
                if (args.Contains("/s")) _bSilentMode = true;

                //Pongo prioridad baja del proceso
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

                //Miro si ya hay una instancia corriendo, si es así la mato y continúo con esta
                CheckIfThereAreOnlyOnePrecess();

                //Leo el archivo de configuracion
                ReadConfigFile();

                //Miro si se ha pasado una ruta por parámetros, sino la creo yo 
                CheckLogsFolder();

                //Borro los logs antiguos al arrancar
                DeleteOldLogs();

                //Escribo la primera entrada en el archivo con valores erroneos para indicar el arranque
                System.IO.StreamWriter OurStream;
                OurStream = System.IO.File.AppendText(_sLogsPathDir + "LogPerfMon " + DateTime.Now.ToString("dd-MM") + ".txt");
                OurStream.WriteLine(DateTime.Now.ToString("dd/MM/yyyy H:mm:ss.fff") + "\t-\t-");
                OurStream.Close();

                //Arranco con el hilo contador de CPU
                _testPerf = new TestPerformance(_sLogsPathDir, _bExtendedMode, _bSilentMode);

                //Para salir de la aplicación de consola al pulsar una tecla
                Console.ReadLine();
                //Una vez salgo mato el hilo
                _testPerf.EndThread();
            }
            catch (Exception ex)
            {
                if (!_bSilentMode) Console.WriteLine(ex.Message);
                if (!_bSilentMode) Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Comprueba si hay un proceso en marcha, en cuyo caso lo mata y continúa con la ejecución
        /// </summary>
        static void CheckIfThereAreOnlyOnePrecess()
        {
            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            Process[] aProcess = Process.GetProcessesByName(thisProc.ProcessName);
            if (aProcess.Length > 1)
            {
                int id = thisProc.Id;
                //Si el proceso tiene id distinta a este lo cierro
                for (int i = 0; i < aProcess.Length; ++i)
                {
                    if (aProcess[i].Id != id)
                        aProcess[i].Kill();
                }
            }
        }

        /// <summary>
        /// Lee el archivo de configuración para cargar parámetros si los encuentra
        /// </summary>
        static void ReadConfigFile()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader("CretaPerformanceMonitor.ini"))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.StartsWith(";"))
                    {
                        string sValue = "";
                        if (line.IndexOf("SilentMode=", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            //Si el modo silencioso se ha pasado como argumento de la aplicación omito este parámetro
                            if (_bSilentMode == false)
                            {
                                sValue = line.Substring(line.LastIndexOf("="));
                                _bSilentMode = sValue.IndexOf("True", StringComparison.OrdinalIgnoreCase) >= 0 ? true : false;
                            }
                        }
                        else if (line.IndexOf("ExtendedMode=", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            sValue = line.Substring(line.LastIndexOf("="));
                            _bExtendedMode = sValue.IndexOf("True", StringComparison.OrdinalIgnoreCase) >= 0 ? true : false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Comprueba las rutas de salida de los logs
        /// </summary>
        static void CheckLogsFolder()
        {
            string sProgDataFolder = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
            const string GENERAL_CRETA_FOLDER = "\\Cretaprint\\";
            const string GENERAL_C2_FOLDER = "Cretaprint2\\";
            const string GENERAL_C3_FOLDER = "CC3\\";
            const string GENERAL_PERFMON_FOLDER = "PerfMon\\";
            const string GENERAL_LOGS_FOLDER = "Logs\\";

            //Compruebo si existe la carpeta de logs de la C3, sino la de la C1 y sino creo una especial
            string sLogsFolder = sProgDataFolder + GENERAL_CRETA_FOLDER + GENERAL_C3_FOLDER + GENERAL_LOGS_FOLDER;
            if (!Directory.Exists(sLogsFolder))
            {
                sLogsFolder = sProgDataFolder + GENERAL_CRETA_FOLDER + GENERAL_C2_FOLDER + GENERAL_LOGS_FOLDER;
                if (!Directory.Exists(sLogsFolder))
                {
                    sLogsFolder = sProgDataFolder + GENERAL_CRETA_FOLDER + GENERAL_PERFMON_FOLDER;
                    Directory.CreateDirectory(sLogsFolder);
                }
            }
            _sLogsPathDir = sLogsFolder;
        }
        /// <summary>
        /// Borra los logs con más de 1 mes de antiguedad
        /// </summary>
        static void DeleteOldLogs()
        {
            string[] files = Directory.GetFiles(_sLogsPathDir);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddMonths(-1))
                    fi.Delete();
            }
        }
    }

    /// <summary>
    /// Esta clase lanza un hilo que comprueba el uso de CPU y memoria libre y lo escribe en un archivo
    /// </summary>
    public class TestPerformance
    {
        private string _sOutputFile;
        private bool _bExtendedMode;
        private bool _bSilentMode;
        public TestPerformance(string OutputFile, bool bExtendedMode, bool bSilentMode)
        {
            _sOutputFile = OutputFile;
            _bExtendedMode = bExtendedMode;
            _bSilentMode = bSilentMode;
            InitThread();
        }

        /*public void GetProcessPerformance(Process proc)
        {
            string sProcName = "Cretaprint2"; //proc.ProcessName;
            using (PerformanceCounter cpuUsage = new PerformanceCounter("Process", "% Processor Time", "_Total"))
            using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", sProcName))
            using (PerformanceCounter cpuTotal = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            using (PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes"))
            {
                System.IO.StreamWriter OurStream;
                OurStream = System.IO.File.AppendText("performance.txt");
                if (!_bSilentMode) Console.WriteLine("");
                OurStream.WriteLine("");

                // Prime the Performance Counters
                pcProcess.NextValue();
                cpuUsage.NextValue();
                cpuTotal.NextValue();
                Thread.Sleep(1000);
                double pcProc = pcProcess.NextValue();
                double cpuTotalPerc = Math.Round(cpuTotal.NextValue());
                double cpuTotal1 = Math.Round(cpuUsage.NextValue(), 2);
                double cpuUse = Math.Round(pcProc / cpuTotal1 * 100, 2);


                // Check for Not-A-Number (Division by Zero)
                if (Double.IsNaN(cpuUse))
                    cpuUse = 0;

                //Get CPU Usage
                Console.ForegroundColor = ConsoleColor.Red;
                if (!_bSilentMode) Console.WriteLine("Process: `{0}' CPU Usage: {1}%", sProcName, Convert.ToInt32(cpuUse));
                OurStream.WriteLine("Process: `{0}' CPU Usage: {1}%", sProcName, Convert.ToInt32(cpuUse));

                //Get CPU Total
                Console.ForegroundColor = ConsoleColor.Red;
                if (!_bSilentMode) Console.WriteLine("Process: CPU Total: {0}%", Convert.ToInt32(cpuTotalPerc));
                OurStream.WriteLine("Process: CPU Total: {0}%", Convert.ToInt32(cpuTotalPerc));

                /*
                // Get Process Memory Usage
                Console.ForegroundColor = ConsoleColor.Green;
                double memUseage = proc.PrivateMemorySize64 / 1048576;
                if (!_bSilentMode) Console.WriteLine("Process: `{0}' Memory Usage: {1}MB", sProcName, memUseage);
                OurStream.WriteLine("Process: `{0}' Memory Usage: {1}MB", sProcName, memUseage);
                
                // Get Total RAM free
                Console.ForegroundColor = ConsoleColor.Cyan;
                float mem = memProcess.NextValue();
                if (!_bSilentMode) Console.WriteLine("During: `{0}' RAM Free: {1}MB", sProcName, mem);
                OurStream.WriteLine("During: `{0}' RAM Free: {1}MB", sProcName, mem);

                //Record and close stream
                Console.ForegroundColor = ConsoleColor.Yellow;
                System.DateTime newDate = System.DateTime.Now;
                if (!_bSilentMode) Console.WriteLine("Recorded: {0}", newDate);
                OurStream.WriteLine("Recorded: {0}", newDate);
                OurStream.Close();
            }
        }*/

        public void getMemoryAndCPU()
        {
            try
            {
                PerformanceCounter cpuCounter = new PerformanceCounter();
                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";

                // will always start at 0
                dynamic firstValue = cpuCounter.NextValue();
                System.Threading.Thread.Sleep(1000);
                // now matches task manager reading
                dynamic secondValue = cpuCounter.NextValue();

                PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes");
                float mem = memProcess.NextValue();

                if (Convert.ToInt32(secondValue) > 25)
                {
                    //TODO: Mirar los procesos que más consumen y volcarlos a un log
                }

                string sOutput = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss.fff") + "\t" + Convert.ToInt32(secondValue) + "\t" + mem;
                if (!_bSilentMode) Console.WriteLine(sOutput);

                //Escribo en el archivo de salida
                System.IO.StreamWriter OurStream;
                OurStream = System.IO.File.AppendText(_sOutputFile + "LogPerfMon " + DateTime.Now.ToString("dd-MM") + ".txt");
                OurStream.WriteLine(sOutput);
                OurStream.Close();
            }
            catch (Exception ex)
            {
                if (!_bSilentMode) Console.WriteLine(ex.Message);
                if (!_bSilentMode) Console.WriteLine(ex.StackTrace);
            }
        }

        public void getMemoryAndCPUExtended()
        {
            try
            {
                //comprobar si los procesos existen para ejecutarlos
                string sProcNameCreta = "Cretaprint2";
                Process[] aProcess = Process.GetProcessesByName(sProcNameCreta);
                bool bProcCretaRunning = (aProcess.Length > 0);
                //Si no existe la c2 anyado el kernel de la c3
                if (!bProcCretaRunning)
                {
                    sProcNameCreta = "CC3";
                    aProcess = Process.GetProcessesByName(sProcNameCreta);
                    bProcCretaRunning = (aProcess.Length > 0);
                }
                string sProcNameGIS = "Gis Print Server";
                aProcess = Process.GetProcessesByName(sProcNameGIS);
                bool bProcGisRunning = (aProcess.Length > 0);

                using (PerformanceCounter cpuUsage = new PerformanceCounter("Process", "% Processor Time", "_Total"))
                using (PerformanceCounter pcProcessCreta = new PerformanceCounter("Process", "% Processor Time", sProcNameCreta))
                using (PerformanceCounter pcProcessGis = new PerformanceCounter("Process", "% Processor Time", sProcNameGIS))
                using (PerformanceCounter cpuTotal = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
                using (PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes"))
                {
                    // Prime the Performance Counters
                    if (bProcCretaRunning) pcProcessCreta.NextValue();
                    if (bProcGisRunning) pcProcessGis.NextValue();
                    cpuUsage.NextValue();
                    cpuTotal.NextValue();
                    Thread.Sleep(1000);
                    double pcProcCreta = bProcCretaRunning ? pcProcessCreta.NextValue() : 0.0;
                    double pcProcGis = bProcGisRunning ? pcProcessGis.NextValue() : 0.0;
                    double cpuTotalPerc = Math.Round(cpuTotal.NextValue());
                    double cpuTotal1 = Math.Round(cpuUsage.NextValue(), 2);
                    double cpuUseCreta = Math.Round(pcProcCreta / cpuTotal1 * 100, 2);
                    double cpuUseGIS = Math.Round(pcProcGis / cpuTotal1 * 100, 2);
                    float mem = memProcess.NextValue();

                    // Check for Not-A-Number (Division by Zero)
                    if (Double.IsNaN(cpuUseCreta))
                        cpuUseCreta = 0;
                    if (Double.IsNaN(cpuUseGIS))
                        cpuUseGIS = 0;

                    string sOutput = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss.fff") + 
                        "\t TOTAL_CPU:\t" + Convert.ToInt32(cpuTotalPerc) +
                        "\t CRETA_CPU:\t" + (bProcCretaRunning? Convert.ToInt32(cpuUseCreta) : -1) +
                        "\t GIS_CPU:\t" + (bProcGisRunning ? Convert.ToInt32(cpuUseGIS) : -1) + 
                        "\t FREE_MEM:\t" + mem;
                    if (!_bSilentMode) Console.WriteLine(sOutput.Replace('\t','-'));

                    //Escribo en el archivo de salida
                    System.IO.StreamWriter OurStream;
                    OurStream = System.IO.File.AppendText(_sOutputFile + "LogPerfMon " + DateTime.Now.ToString("dd-MM") + ".txt");
                    OurStream.WriteLine(sOutput);
                    OurStream.Close();
                }
            }
            catch (Exception ex)
            {
                if (!_bSilentMode) Console.WriteLine(ex.Message);
                if (!_bSilentMode) Console.WriteLine(ex.StackTrace);
            }
        }

        #region Get Data Thread
        System.Threading.Thread _th = null;
        bool _bFinished;

        DateTime datStart;
        TimeSpan elapsedTime;
        private void InitThread()
        {
            if (_th == null)
            {
                _bFinished = false;
                _th = new System.Threading.Thread(UpdateThread);
                _th.Name = "CPU consumption";

                _th.IsBackground = true;
                _th.Start();
                datStart = DateTime.Now;
            }
        }
        public void EndThread()
        {
            _bFinished = true;
            _th.Join();
            _th = null;
        }

        private const int TIME_TO_SLEEP = 100;
        private const int TIME_TO_CHECK = 1000; //1 segundo
        private void UpdateThread()
        {
            while (!_bFinished)
            {
                try
                {
                    elapsedTime = DateTime.Now.Subtract(datStart);
                    if (elapsedTime.TotalMilliseconds >= TIME_TO_CHECK)
                    {
                        if (_bExtendedMode)
                            getMemoryAndCPUExtended();
                        else
                            getMemoryAndCPU();
                        
                        datStart = DateTime.Now;
                    }
                }
                finally
                {
                    System.Threading.Thread.Sleep(TIME_TO_SLEEP);
                }
            }
        }
        #endregion
    }
}