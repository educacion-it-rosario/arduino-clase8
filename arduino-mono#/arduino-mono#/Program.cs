using System;
using System.IO.Ports;
using System.Threading;

namespace arduinomono
{
  class Program
  {

    private static void Encabezado ()
    {
      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("C# + Arduino usando el System.IO.Ports");
      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("Por Federico Pfaffendorf");
      Console.WriteLine ("http://www.federicopfaffendorf.com.ar");
      Console.WriteLine ("--------------------------------------");
    }

    private static void Pie ()
    {
      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("COM: " + puertoArduino);
      Console.WriteLine ("LED: " + LED);
      Console.WriteLine ("--------------------------------------");
    }

    private static void EsperarParaContinuar()
    {
      while (Console.KeyAvailable) {
        Thread.Sleep (100);
        Console.ReadKey ();
      }
    }

    private static void PresionarParaContinuar ()
    {

      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("Presione tecla para continuar ...");
      Console.WriteLine ("--------------------------------------");

      while (!Console.KeyAvailable) {
        Thread.Sleep (100);
      }
      Console.ReadKey ();

    }

    private static void Menu ()
    {
      Encabezado ();
      Console.WriteLine ("|L| Listar puertos COM disponibles");
      Console.WriteLine ("|S| Setear puerto COM del Arduino");
      Console.WriteLine ("|E| Encender LED");
      Console.WriteLine ("|A| Apagar LED");
      Console.WriteLine ("|P| Leer POT");
      Console.WriteLine ("|Q| Salir");
      Pie ();
    }

    private static void ListarPuertos ()
    {
      Console.Clear ();

      Encabezado ();

      string[] v = SerialPort.GetPortNames ();
      Array.Sort (v);

      foreach (string s in v) {
        Console.WriteLine (s);
      }

      PresionarParaContinuar ();

    }

    private static String puertoArduino = "";
    private static SerialPort serialPort = null;

    private static void SetearPuertoArduino ()
    {

      Console.Clear ();

      Encabezado ();

      Console.WriteLine ("Indicar solo el número de puerto COM");
      Console.WriteLine ("al que se encuentra conectado la placa");
      Console.WriteLine ("--------------------------------------");
      Console.Write ("Número de puerto: ");

      try {
        Console.CursorVisible = true;
        puertoArduino = Console.ReadLine().Trim();
        Console.CursorVisible = false;

        if (Array.Find(SerialPort.GetPortNames (),
                       s => s.Equals (puertoArduino)) == null)
          throw new Exception ();

        serialPort = new SerialPort(puertoArduino, 9600);
        serialPort.Open();

        Console.WriteLine ("Puerto " + puertoArduino + " seteado");

      } catch (Exception) {
        Console.WriteLine ("(!) Puerto incorrecto");
        puertoArduino = "";
        serialPort = null;
      }
      PresionarParaContinuar ();
    }

    private static bool LED = false;

    private static void EncenderLED (bool state)
    {
      serialPort.WriteLine (state?"H":"L");
      LED = state;
    }

    private static void LeerPOT ()
    {

      Console.Clear ();
      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("POT:     0");
      Console.WriteLine ("--------------------------------------");
      Console.WriteLine ("Presione una tecla para regresar ...");
      Console.WriteLine ("--------------------------------------");

      serialPort.DiscardInBuffer ();
      serialPort.WriteLine ("P");
      while (!Console.KeyAvailable) {
        Console.SetCursorPosition (7, 1);
        Console.WriteLine (serialPort.ReadLine().Trim().PadLeft (3, ' '));
      }
      serialPort.WriteLine ("p");
    }

    static bool loop() {
      Console.Clear ();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.CursorVisible = false;

      Menu ();

      char c = Console.ReadKey (true).KeyChar;

      switch (char.ToUpper (c)) {

      case 'L':
        {
          ListarPuertos ();
          break;
        }
      case 'S':
        {
          SetearPuertoArduino ();
          break;
        }
      case 'E':
        {
          if (puertoArduino.Equals ("") == false) {
            EncenderLED (true);
          }
          EsperarParaContinuar();
          break;
        }
      case 'A':
      {
        if (puertoArduino.Equals ("") == false) {
          EncenderLED (false);
        }
        EsperarParaContinuar();
        break;
      }
      case 'P':
        {
          if (puertoArduino.Equals ("") == false) {
            LeerPOT ();
          }
          EsperarParaContinuar();
          break;
        }
      case 'Q':
        {
          return false;
        }
      }

      return true;
    }

    static void Main ()
    {
      bool cont = true;
      while (cont) {
        try {
          cont = loop();
        } catch (Exception e) {
          Console.WriteLine (e.ToString());
          Console.WriteLine ("Algo fallo.....");
          continue;
        }
      }

      if (serialPort != null && serialPort.IsOpen) {
        serialPort.Close ();
      }
    }
  }
}
