#include <SoftwareSerial.h>   // Incluimos la librería  SoftwareSerial  
SoftwareSerial miBT(10,11);  // Definimos los pines RX y TX del Arduino conectados al Bluetooth
char dato= 0;
int ledazul= 2;
int ledverde= 3;
char brillo= 0;

void setup()
{
 Serial.begin(9600);
 miBT.begin(38400);
 pinMode(ledazul,OUTPUT);
 pinMode(ledverde,OUTPUT);
}
 
void loop()
{
  if(miBT.available())    // Si llega un dato por el puerto BT se envía al monitor serial
  {
    dato=miBT.read();
     if (dato == '1')
     {
      Serial.print("\x02");
      Serial.print(dato);
      Serial.print("\x03");
     }
     else
     if (dato == '2')
     {
      Serial.print("\x02");
      Serial.print(dato);
      Serial.print("\x03");
     }
     else
     if (dato == '3')
     {
      Serial.print("\x02");
      Serial.print(dato);
      Serial.print("\x03");
     }
      else
      if (dato == '4')
     {
      Serial.print("\x02");
      Serial.print(dato);
      Serial.print("\x03");
     }
      else
      if (dato == '5')
      {
      Serial.print("\x02");
      Serial.print(dato);
      Serial.print("\x03");
     }

  } 
}
