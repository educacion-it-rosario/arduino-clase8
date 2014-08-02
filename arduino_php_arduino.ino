/*
	Federico Pfaffendorf
	yo@federicopfaffendorf.com.ar
	(t) @fpfaffendorf
*/

const int ledPin = 2; // Pin al que esta relacionado el LED
int rxByte;
 
void setup() {
  
  Serial.begin(9600); // Inicializa la comunicacion serial en 9600 baudios 
  pinMode(ledPin, OUTPUT); // Inicializa el pin relacionado al LED como salida
  
}
 
void loop() {
  
  if (Serial.available() > 0) // Chequea si hay nueva informacion en el Serial
  {
    
    rxByte = Serial.read(); // Lee un byte del buffer del Serial

    if (rxByte == 'H') // Si se encontro un Byte que convertido a char es una H
    {
      digitalWrite(ledPin, HIGH); // Pone en alto el pin relacionado al LED
    }

    if (rxByte == 'L') // Si se encontro un byte que convertido a char es una L
    {
      digitalWrite(ledPin, LOW); // Pone en bajo el pin relacionado al LED
    }
  
  }
  
}
