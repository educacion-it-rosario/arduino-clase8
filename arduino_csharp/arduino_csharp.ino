/*
    Manuel Naranjo
    naranjo.manuel@gmail.com
    (t) @mnaranjo85
    
    Basado en el trabajo de:
	Federico Pfaffendorf
	yo@federicopfaffendorf.com.ar
	(t) @fpfaffendorf
*/

const int ledPin = 13; // Pin al que esta relacionado el LED
const int analogPin = 5; // canal analogico a leer
int rxByte;
byte val;
boolean analogico = false;
 
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
      Serial.flush();
    }

    if (rxByte == 'L') // Si se encontro un byte que convertido a char es una L
    {
      digitalWrite(ledPin, LOW); // Pone en bajo el pin relacionado al LED
      Serial.flush();
    }
    
    if (rxByte == 'S') // si se envio una S
    {
      Serial.flush();
      Serial.println(digitalRead(ledPin));
    }
    
    if (rxByte == 'P') // si se envio una P leer analogico
    {
      analogico = true;
    }
    
    if (rxByte == 'p') // finalizar lectura analogico
    {
      analogico = false;
    }
  }
  
  if (analogico) {
    val = analogRead(analogPin);
    Serial.println(val);
  }
  
  delay(100);
  
}
