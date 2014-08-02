using System;
using System.IO.Ports;
using System.Threading;

namespace CSharpArduino
{
    class Program
    {

        private static void Encabezado()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("C# + Arduino usando el System.IO.Ports");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Por Federico Pfaffendorf");
            Console.WriteLine("http://www.federicopfaffendorf.com.ar");
            Console.WriteLine("--------------------------------------");
        }

        private static void Pie()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("COM: " + puertoArduino);
            Console.WriteLine("LED: " + LED);
            Console.WriteLine("--------------------------------------");
        }

        private static void EsperarParaContinuar()
        {

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Espere ...");
            Console.WriteLine("--------------------------------------");

            Thread.Sleep(2000);

        }

        private static void Menu()
        {
            Encabezado();
            Console.WriteLine("|L| Listar puertos COM disponibles");
            Console.WriteLine("|S| Setear puerto COM del Arduino");
            Console.WriteLine("|E| Encender/Apagar LED");
            Console.WriteLine("|P| Leer POT");
            Console.WriteLine("|Q| Salir");
            Pie();
        }

        private static void ListarPuertos()
        {
            Console.Clear();

            Encabezado();

            string[] v = SerialPort.GetPortNames();
            Array.Sort(v);

            foreach (string s in v)
            {
                Console.WriteLine(s);
            }

            EsperarParaContinuar();

        }

        private static byte puertoArduino = 0;

        private static void SetearPuertoArduino()
        {

            Console.Clear();

            Encabezado();

            Console.WriteLine("Indicar solo el número de puerto COM");
            Console.WriteLine("al que se encuentra conectado la placa");
            Console.WriteLine("--------------------------------------");
            Console.Write("Número de puerto: ");

            try
            {

                Console.CursorVisible = true;
                puertoArduino = byte.Parse(Console.ReadLine());
                Console.CursorVisible = false;

                if (
                    Array.Find(
                    SerialPort.GetPortNames(), 
                    s => s.Equals("COM" + puertoArduino)
                    ) == null
                   ) throw new Exception();

                Console.WriteLine("Puerto " + puertoArduino + " seteado");

            }
            catch (Exception)
            {
                Console.WriteLine("(!) Puerto incorrecto");
                puertoArduino = 0;
            }

            EsperarParaContinuar();

        }

        private static bool LED = false;

        private static void EncenderLED()
        {

            using (SerialPort sp = new SerialPort("COM" + puertoArduino, 9600))
            {

                sp.Open();

                sp.Write(LED ? "L" : "H");

                sp.Close();

            }

            LED = !LED;

        }

        private static void LeerPOT()
        {

            Console.Clear();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("POT:     0");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Presione una tecla para regresar ...");
            Console.WriteLine("--------------------------------------");

            using (SerialPort sp = new SerialPort("COM" + puertoArduino, 9600))
            {

                sp.Open();

                while (!Console.KeyAvailable)
                {
                    Console.SetCursorPosition(7, 1);
                    sp.DiscardInBuffer();
                    Console.WriteLine(sp.ReadByte().ToString().PadLeft(3, ' '));
                }

                sp.Close();

            }

        }

        static void Main()
        {

            while (true)
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.CursorVisible = false;

                Menu();

                char c = Console.ReadKey(true).KeyChar;

                switch (char.ToUpper(c))
                {

                    case 'L':
                        ListarPuertos();
                        break;

                    case 'S':
                        SetearPuertoArduino();
                        break;

                    case 'E':
                        if (puertoArduino != 0) EncenderLED();
                        break;

                    case 'P':
                        if (puertoArduino != 0) LeerPOT();
                        break;

                    case 'Q':
                        return;
                        break;

                }

            }

        }
    }
}