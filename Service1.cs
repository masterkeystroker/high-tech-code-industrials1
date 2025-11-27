using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PLC
{
    static class Ini
    {
        static bool Salir;

        static void Main(string[] args)
        {
#if !DEBUG
            //Servicio de windows
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service1() 
			};
            ServiceBase.Run(ServicesToRun);
#else
            //Programa de consola
            Program P = new Program(@"C:\ProgramData\Cretaprint\Plotter\Mem.cfg");
            int tCiclo = 20;
            DateTime IniCiclo = DateTime.Now;
            Salir = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            System.Threading.Thread.Sleep(500);
            while (Salir == false)
            {
                try
                {
                    //Ciclo normal de programa
                    IniCiclo = DateTime.Now;

                    //Ciclo
                    P.Scan();

                    //Comprobar si se debe salir del ciclo de scan
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo K = Console.ReadKey();
                        if (K.KeyChar == 'q' || K.KeyChar == 'Q') Salir = true;
                    }
                }
                finally
                {
                    System.Threading.Thread.Sleep(tCiclo);
                }
            }

            Console.Clear();
            Console.WriteLine("Cerrando...");
            P.Dispose();
            P = null;
            Console.WriteLine("Cerrado");
#endif
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
#if DEBUG
            Salir = true;
            e.Cancel = true;
#endif
        }

    }

#if !DEBUG
    
    /// <summary>
    /// Servicio de windows
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        private Program P;
        private System.Threading.Thread th;
        private bool Salir;

        public Service1()
        {
            Salir = false;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // si el primer parámetro es DEBUG se lanza el debugger
                if (args.GetLength(0) > 0 && args[0].Equals("DEBUG")) System.Diagnostics.Debugger.Launch();
                base.OnStart(args);

                Salir = false;
                P = new Program(@"C:\ProgramData\Cretaprint\Plotter\Mem.cfg");

                System.Threading.Thread.Sleep(300);
                P.Scan();

                th = new System.Threading.Thread(Scan);
                th.IsBackground = true;
                th.Start();
            }
            catch(Exception ex)
            {
                Program.Suceso(100, ex.Message, ex.StackTrace);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            try
            {
                Salir = true;
                if (th != null) th.Join();
                th = null;

                P.Dispose();
                P = null;

                base.OnStop();
            }
            catch (Exception ex)
            {
                Program.Suceso(100, ex.Message, ex.StackTrace);
            }
        }

        private void Scan()
        {
            //Ciclo
            System.Threading.Thread.Sleep(500);

            while (Salir == false)
            {
                try
                {
                    P.Scan();
                }
                catch (Exception ex)
                {
                    Program.Suceso(100, ex.Message, ex.StackTrace);
                }
                finally
                {
                    System.Threading.Thread.Sleep(20);
                }
            }
        }

    }

#endif
}
