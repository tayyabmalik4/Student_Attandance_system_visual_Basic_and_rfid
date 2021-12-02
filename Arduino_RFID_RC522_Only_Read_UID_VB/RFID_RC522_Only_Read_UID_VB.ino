#include <SPI.h>
#include <MFRC522.h>

#define SS_PIN 10
#define RST_PIN 9
MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance.

int readsuccess;
byte readcard[4];
char str[32] = "";
String StrUID;

void setup() {
  Serial.begin(9600); // Initialize serial communications with the PC
  SPI.begin();      // Init SPI bus
  mfrc522.PCD_Init(); // Init MFRC522 card
  pinMode(7,OUTPUT);
  pinMode(8,OUTPUT);
}
// --------------------------------------------------------------------
void loop() {
  readsuccess = getid();
 
  if(readsuccess){
    Serial.println(StrUID);
    delay(1000);
  }
  if (Serial.available()){
    int LED=Serial.read();
    if (LED=='1'){
      digitalWrite(7,HIGH);
      digitalWrite(8,HIGH);
      delay(1000);
      digitalWrite(7,LOW);
      digitalWrite(8,LOW);
    }
    else if(LED=='2'){
      digitalWrite(6,HIGH);
      digitalWrite(8,HIGH);
      delay(1000);
      digitalWrite(6,LOW);
      digitalWrite(8,LOW);
    }
//    else if (LED=='0'){
//      digitalWrite(7,LOW);
//      digitalWrite(8,LOW);
//    }
//    else if (LED=='3'){
//      digitalWrite(6,LOW);
//      digitalWrite(
//    }
  }
}
// --------------------------------------------------------------------
int getid(){  
  if(!mfrc522.PICC_IsNewCardPresent()){
    return 0;
  }
  if(!mfrc522.PICC_ReadCardSerial()){
    return 0;
  }
  for(int i=0;i<4;i++){
    readcard[i]=mfrc522.uid.uidByte[i]; //storing the UID of the tag in readcard
    array_to_string(readcard, 4, str);
    StrUID = str;
  }
  mfrc522.PICC_HaltA();
  return 1;
}
// --------------------------------------------------------------------
void array_to_string(byte array[], unsigned int len, char buffer[])
{
    for (unsigned int i = 0; i < len; i++)
    {
        byte nib1 = (array[i] >> 4) & 0x0F;
        byte nib2 = (array[i] >> 0) & 0x0F;
        buffer[i*2+0] = nib1  < 0xA ? '0' + nib1  : 'A' + nib1  - 0xA;
        buffer[i*2+1] = nib2  < 0xA ? '0' + nib2  : 'A' + nib2  - 0xA;
    }
    buffer[len*2] = '\0';
}
