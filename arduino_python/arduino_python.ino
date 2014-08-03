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
char accion;
int pin, cuenta;
char entrada[256];
 
void setup() {
  
  Serial.begin(9600); // Inicializa la comunicacion serial en 9600 baudios 
  pinMode(ledPin, OUTPUT); // Inicializa el pin relacionado al LED como salida
  
}
 
void loop() {
  
  // si no tenemos suficiente cantidad de bytes esperando entonces terminamos
  if (Serial.available() < 3)
    return;
  
  // leer hasta el fin de linea
  cuenta = Serial.readBytesUntil('\n', entrada, sizeof(entrada)-1);
    
  //agregar terminador
  entrada[cuenta+1] = 0;

  accion = entrada[0];
  
  pin = atoi(entrada+1);
  
  //debugging simple
#ifdef DEBUG
  Serial.print("Accion: ");
  Serial.print((char)accion);
  Serial.print(" pin:");
  Serial.println(pin);
#endif  
  
  switch (accion) {
    case 'H':
      digitalWrite(pin, HIGH);
      break;
    case 'L':
      digitalWrite(pin, LOW);
      break;
    case 'S':
      Serial.println(digitalRead(pin));
      break;
  }
}
