#include "SPI.h"
#include "PN532_SPI.h"
#include "snep.h"
#include "NdefMessage.h"
#include "SoftwareSerial.h"
#include <LiquidCrystal.h>

LiquidCrystal lcd(3, 4, 5, 6, 7, 8);

PN532_SPI pn532spi(SPI, 10);
SNEP nfc(pn532spi);
uint8_t ndefBuf[128];
uint8_t recordBuf[128];


void setup()
{
    Serial.begin(9600);
    lcd.begin(16, 2);
  lcd.clear();
 
  lcd.setCursor(0, 0); 
  lcd.print("GIRIS");
  lcd.setCursor(0, 1); 
  lcd.print("BEKLENIYOR...");
  
  delay(1000);
}

void loop()
{
      char veri;
//      char veri1;
//      char veri2;
//      char veri3;
   unsigned long a=0;
   unsigned long b=0;
   unsigned long c=0;
   unsigned long d=0;
    int msgSize = nfc.read(ndefBuf, sizeof(ndefBuf));
    if (msgSize > 0) {
       
        NdefMessage msg  = NdefMessage(ndefBuf, msgSize);
       
  

        NdefRecord record = msg.getRecord(0);
    
       int recordLength = record.getPayloadLength();
    
        if (recordLength <= sizeof(recordBuf)) {
           record.getPayload(recordBuf);
                   
                 if(recordLength<8){
            Serial.println(recordBuf[0]); 
             Serial.println(recordBuf[1]); 
             Serial.println(recordBuf[2]); 
             Serial.println(recordBuf[3]); 
              Serial.println(recordBuf[4]); 
             Serial.println(recordBuf[5]); 
             Serial.println(recordBuf[6]); 

     delay(500);
                 }
 while(Serial.available()>0)
    {
      veri=Serial.read();
      if(veri=='A')
      {
        lcd.clear();
        lcd.home();
        lcd.print("GIRIS BASARILI...");
        delay(1000);  
        lcd.setCursor(0, 1); 
        lcd.print("KAPI ACILIYOR...");
        delay(2000);
        lcd.setCursor(0, 1);
        lcd.print("KAPI KAPATILIYOR...");
        delay(2000);
        lcd.clear();
        lcd.home();
        lcd.setCursor(0, 0); 
        lcd.print("GIRIS");
        lcd.setCursor(0, 1); 
        lcd.print("BEKLENIYOR...");
      
      }
      else if(veri=='K')
      {
        lcd.clear();
        lcd.home();
        lcd.print("...HATA...");
        lcd.setCursor(0, 1); 
        lcd.print("GECERSIZ KART...");
        delay(2000);
        lcd.clear();
        lcd.home();
        lcd.setCursor(0, 0); 
        lcd.print("GIRIS");
        lcd.setCursor(0, 1); 
        lcd.print("BEKLENIYOR...");
           
           
                 }
                 
        }

        }
        }
        }
    

