/*
	Federico Pfaffendorf
	yo@federicopfaffendorf.com.ar
	(t) @fpfaffendorf
*/
 
import processing.serial.*;
Serial port;
 
boolean status;
 
void setup()  {
 
  port = new Serial (this, "/dev/ttyACM0", 9600);  // Reemplazar "/dev/ttyACM0" por el puerto local utilizado.
                                                   // Caso Windows sería un puerto COM.
  status = false;
  port.write ('L');
  
}
 
void draw() {
 
  String html[] = loadStrings("http://localhost/php-arduino/ws-client/switch-get.php"); // Modificar de acuerdo al servidor local

  print (hour() + ":"); // Imprime la primera linea de la respuesta html junto con la fecha y hora
  print (minute() + ":");
  print (second() + " ");
  println (html [0]);  
 
  if ((html[0].equals("true")) && (status == false)) 
  {
    
    port.write('H'); // Envía al puerto un caracter H
    status = true;
 
  } 
  else 
  if ((! html[0].equals("true")) && (status == true)) 
  {
 
    port.write('L'); // Envía al puerto un caracter L
    status = false;
    
  }
 
  delay (1000); // Demora de 1 segundo hasta la próxima petición
  
 }
