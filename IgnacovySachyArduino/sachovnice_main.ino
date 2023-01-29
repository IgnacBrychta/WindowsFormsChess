#include <LiquidCrystal_I2C.h>
#include "FastLED.h"

#define BUFFER_SIZE 32
#define TIMER_PACKET_SIZE 12
#define CONFIG_PACKET_SIZE 1
#define INFORMATION_PACKET_SIZE 16
#define CHESSBOARD_SIZE 64
#define NUM_LEDS 256

#define HallCol1 A0
#define HallCol2 A1
#define HallCol3 A2
#define HallCol4 A3
#define HallCol5 A4
#define HallCol6 A5
#define HallCol7 A6
#define HallCol8 A7

#define HallRow1 22
#define HallRow2 23
#define HallRow3 24
#define HallRow4 25
#define HallRow5 26
#define HallRow6 27
#define HallRow7 28
#define HallRow8 29

#define DATA_PIN_LED_MATRIX 31

#define MotorColumn_F 33
#define MotorColumn_B 34
#define MotorRow_F 35
#define MotorRow_B 36

#define ElectromagnetPolarity1 37
#define ElectromagnetPolarity2 38

#define ProhraTlacitkoBila 39
#define ProhraTlacitkoCerna 40
#define RemizaTlacitko 41
#define RestartTlacitko 42
#define PritelNaTelefonuTlacitko 43

#define DETECTION_THRESHOLD 550
#define LED_MATRIX_BRIGHTNESS 10

CRGB LED_Matrix[NUM_LEDS];
CRGB boardColor;
char buffer[BUFFER_SIZE];

bool figs[CHESSBOARD_SIZE];
int pieceMoved_index_1 = -1;
int pieceMoved_index_2 = -1;
uint8_t chessboardArrayIndex = 0;
LiquidCrystal_I2C lcd_White(0x27, INFORMATION_PACKET_SIZE, 2);
LiquidCrystal_I2C lcd_Black(0x23, INFORMATION_PACKET_SIZE, 2);
char pressedButton = '0';
bool twoTimers = false;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  SetPinModes();

  FastLED.addLeds<WS2812B, DATA_PIN_LED_MATRIX, RGB>(LED_Matrix, NUM_LEDS);
  FastLED.setBrightness(LED_MATRIX_BRIGHTNESS);
  ResetLEDmatrix();

  lcd_Black.init();
  lcd_Black.backlight();
  lcd_White.init();
  lcd_White.backlight();
}
void ResetLEDmatrix() {
  for(int i = 0; i < NUM_LEDS; i++) 
  {
    LED_Matrix[i] = CRGB::Black;
  }
  FastLED.show();
}
void SetPinModes() {
  pinMode(LED_BUILTIN, OUTPUT);
  // For reading each column of Hall effect sensors
  pinMode(HallCol1, INPUT);
  pinMode(HallCol2, INPUT);
  pinMode(HallCol3, INPUT);
  pinMode(HallCol4, INPUT);
  pinMode(HallCol5, INPUT);
  pinMode(HallCol6, INPUT);
  pinMode(HallCol7, INPUT);
  pinMode(HallCol8, INPUT);
  // For selecting which row of Hall effect sensors to read
  pinMode(HallRow1, OUTPUT);
  pinMode(HallRow2, OUTPUT);
  pinMode(HallRow3, OUTPUT);
  pinMode(HallRow4, OUTPUT);
  pinMode(HallRow5, OUTPUT);
  pinMode(HallRow6, OUTPUT);
  pinMode(HallRow7, OUTPUT);
  pinMode(HallRow8, OUTPUT);
  // For axis movement
  pinMode(MotorColumn_F, OUTPUT);
  pinMode(MotorColumn_B, OUTPUT);
  pinMode(MotorRow_F, OUTPUT);
  pinMode(MotorRow_B, OUTPUT);
  // Electromagnet
  pinMode(ElectromagnetPolarity1, OUTPUT);
  pinMode(ElectromagnetPolarity2, OUTPUT);
  // RGB
  //pinMode(DATA_PIN_LED_MATRIX, OUTPUT);
  // Buttons
  pinMode(ProhraTlacitkoBila, INPUT);
  pinMode(ProhraTlacitkoCerna, INPUT);
  pinMode(RemizaTlacitko, INPUT); // dvě tlačítka pro jeden vstup
  pinMode(RestartTlacitko, INPUT); // jedno tlačítko
  pinMode(PritelNaTelefonuTlacitko, INPUT); // dvě tlačítka pro jeden vstup
}
bool CheckForChessboardChanges() {
  return false;
  bool figurkyZmeny[64];
  for(int row = 0; row < 8; row++) {
    digitalWrite(HallRow1 + row, HIGH); // activate Hall sensor row
    int index = row * 8;
    figurkyZmeny[index]   = IsChessPiecePresent(HallCol1);
    figurkyZmeny[index+1] = IsChessPiecePresent(HallCol2);
    figurkyZmeny[index+2] = IsChessPiecePresent(HallCol3);
    figurkyZmeny[index+3] = IsChessPiecePresent(HallCol4);
    figurkyZmeny[index+4] = IsChessPiecePresent(HallCol5);
    figurkyZmeny[index+5] = IsChessPiecePresent(HallCol6);
    figurkyZmeny[index+6] = IsChessPiecePresent(HallCol7);
    figurkyZmeny[index+7] = IsChessPiecePresent(HallCol8);
    digitalWrite(HallRow1 + row, LOW); // deactivate Hall sensor row
  }
  bool chessboardChanged = false;
  for(int i = 0; i < 64; i++) {
    if(figurkyZmeny[i] != figs[i]) {
      if(pieceMoved_index_1 == -1)
      {
        pieceMoved_index_1 = i;
      }
      else
      {
        pieceMoved_index_2 = i;
      }
      chessboardChanged = true;
    }
  }
  return chessboardChanged;
}
bool IsChessPiecePresent(int pin) {
  int analogValue = analogRead(pin);
  return analogValue > DETECTION_THRESHOLD;
}
void loop() {
  // put your main code here, to run repeatedly:
  int availableBytes = Serial.available();
  if(availableBytes > 0)
  {
    int packetType = Serial.read();
    if(packetType == 1) 
    {
      chessboardArrayIndex = 0;
      Serial.readBytes(buffer, BUFFER_SIZE);

      DecodeFigs();
      DecodeHighlighting();
    }
    else if(packetType == 2)
    {
      Serial.readBytes(buffer, TIMER_PACKET_SIZE);
    
      DecodeGameInfo();
    }
    else if(packetType == 3)
    {
      Serial.readBytes(buffer, CONFIG_PACKET_SIZE);
      Serial.println(F("arduino chess board init"));
      lcd_White.clear();
      lcd_Black.clear();
      ResetLEDmatrix();
      if(buffer[0] == '2')
      {
        twoTimers = true;
      }
      else
      {
        twoTimers = false;
      }
    }
    else if(packetType == 4)
    {
      lcd_White.clear();
      lcd_Black.clear();
      Serial.readBytes(buffer, INFORMATION_PACKET_SIZE);
      for(int i = 0; i < INFORMATION_PACKET_SIZE; i++)
      {
        char character = buffer[i];
        lcd_White.print(character);
        lcd_Black.print(character);
      }
    }
  }
  if(CheckForChessboardChanges() && false)
  {
    Serial.println(F("piece moved"));
    Serial.print(pieceMoved_index_1);
    Serial.print(' ');
    Serial.print(pieceMoved_index_2);
    pieceMoved_index_1 = -1;
    pieceMoved_index_2 = -1;
  }
  if(CheckForButtonPress())
  {
    Serial.println(F("button pressed"));
    Serial.print(pressedButton);
    delay(350); // antispam
  }
}
bool CheckForButtonPress() {
  if(digitalRead(ProhraTlacitkoBila) == HIGH)
  {
    pressedButton = '1';
    return true;
  }
  else if(digitalRead(ProhraTlacitkoCerna) == HIGH)
  {
    pressedButton = '2';
    return true;
  }
  else if(digitalRead(RemizaTlacitko) == HIGH)
  {
    pressedButton = '3';
    return true;
  }
  else if(digitalRead(RestartTlacitko) == HIGH)
  {
    pressedButton = '4';
    return true;
  }
  else if(digitalRead(PritelNaTelefonuTlacitko) == HIGH)
  {
    pressedButton = '5';
    return true;
  }
  return false;
}
void EmptySerialBuffer() {
  while(Serial.available())
  {
    Serial.read();
  }
}
void DecodeGameInfo() {
  // 12:34 56:79 [+|-] 99 [W|B]
  lcd_White.clear();
  lcd_Black.clear();
  if(twoTimers)
  {
    lcd_White.print(buffer[0]);
    lcd_White.print(buffer[1]);
    lcd_White.print(':');
    lcd_White.print(buffer[2]);
    lcd_White.print(buffer[3]);
    lcd_White.print(F("    "));
    if(buffer[8] == '+')
    {
      lcd_White.print('+');
    }
    else
    {
      lcd_White.print('-');
    }
    lcd_White.print(buffer[9]);
    lcd_White.print(buffer[10]);
    lcd_White.print(F("  "));
    lcd_White.print(buffer[11]);

    lcd_White.setCursor(0, 1);
    lcd_White.print(buffer[4]);
    lcd_White.print(buffer[5]);
    lcd_White.print(':');
    lcd_White.print(buffer[6]);
    lcd_White.print(buffer[7]);


    lcd_Black.print(buffer[4]);
    lcd_Black.print(buffer[5]);
    lcd_Black.print(':');
    lcd_Black.print(buffer[6]);
    lcd_Black.print(buffer[7]);
    lcd_Black.print(F("    "));
    if(buffer[8] == '+')
    {
      lcd_Black.print('-');
    }
    else
    {
      lcd_Black.print('+');
    }
    lcd_Black.print(buffer[9]);
    lcd_Black.print(buffer[10]);
    lcd_Black.print(F("  "));
    lcd_Black.print(buffer[11]);
    
    lcd_Black.setCursor(0, 1);
    lcd_Black.print(buffer[0]);
    lcd_Black.print(buffer[1]);
    lcd_Black.print(':');
    lcd_Black.print(buffer[2]);
    lcd_Black.print(buffer[3]);
  }
  else
  {
    lcd_White.print(buffer[0]);
    lcd_White.print(buffer[1]);
    lcd_White.print(':');
    lcd_White.print(buffer[2]);
    lcd_White.print(buffer[3]);
    lcd_White.print(F("    "));
    if(buffer[8] == '+')
    {
      lcd_White.print('+');
    }
    else
    {
      lcd_White.print('-');
    }
    lcd_White.print(buffer[9]);
    lcd_White.print(buffer[10]);
    lcd_White.print(F("  "));
    lcd_White.print(buffer[11]);

    lcd_Black.print(buffer[0]);
    lcd_Black.print(buffer[1]);
    lcd_Black.print(':');
    lcd_Black.print(buffer[2]);
    lcd_Black.print(buffer[3]);
    lcd_Black.print(F("    "));
    if(buffer[8] == '+')
    {
      lcd_Black.print('-');
    }
    else
    {
      lcd_Black.print('+');
    }
    lcd_Black.print(buffer[9]);
    lcd_Black.print(buffer[10]);
    lcd_Black.print(F("  "));
    lcd_Black.print(buffer[11]);
  }
  
}
void DecodeHighlighting() {
  int index = 3;
  int byteCount = 0;
  int temp_iterace = 1;
  while(true) 
  {
    bool colorBit1 = GetBitFromByte(buffer[index], 2);
    bool colorBit2 = GetBitFromByte(buffer[index], 1);
    bool colorBit3 = GetBitFromByte(buffer[index], 0);
    uint8_t colorByte = 0;
    bitWrite(colorByte, 0, colorBit1);
    bitWrite(colorByte, 1, colorBit2);
    bitWrite(colorByte, 2, colorBit3);
    
    GetColorFromByte(colorByte);
    SetColor();

    colorBit1 = GetBitFromByte(buffer[index], 6);
    colorBit2 = GetBitFromByte(buffer[index], 5);
    colorBit3 = GetBitFromByte(buffer[index], 4);
    colorByte = 0;
    bitWrite(colorByte, 0, colorBit1);
    bitWrite(colorByte, 1, colorBit2);
    bitWrite(colorByte, 2, colorBit3);
    
    GetColorFromByte(colorByte);
    SetColor();
    temp_iterace++;
    byteCount++;
    index--;
    if(byteCount % 4 == 0)
    {
      index+=8; // next row
      byteCount = 0;
      chessboardArrayIndex+=16; // next row jump
      
      if(index == 35)
      {
        break;
      }
    }
  }
  FastLED.show();
}
void SetColor() {
                                                  // 46
  int modulo = chessboardArrayIndex % 16;         // 46 % 16 = 14
  int currentColumn = 16 - modulo;                // 16 - 14 = 2
  int indexPlus = currentColumn * 2 - 1;          // 2 * 4 - 1 = 3
  int index = chessboardArrayIndex + indexPlus;   // 46 + 3 = 49

  LED_Matrix[chessboardArrayIndex] = boardColor;
  LED_Matrix[index] = boardColor;
  chessboardArrayIndex++;


  modulo = chessboardArrayIndex % 16;         // 47 % 16 = 15
  currentColumn = 16 - modulo;                // 16 - 15 = 1
  indexPlus = currentColumn * 2 - 1;          // 1 * 2 - 1 = 1
  index = chessboardArrayIndex + indexPlus;   // 47 + 1 = 48

  LED_Matrix[chessboardArrayIndex] = boardColor;
  LED_Matrix[index] = boardColor;
  chessboardArrayIndex++;
}
void DecodeFigs() {
  int figIndex = 0;
  for(int i = 0; i < BUFFER_SIZE; i++) 
  {
    bool figPresent = GetBitFromByte(buffer[i], 7);
    figs[figIndex] = figPresent;
    figIndex++;
    //Serial.print(figPresent);
    figPresent = GetBitFromByte(buffer[i], 3);
    figs[figIndex] = figPresent;
    figIndex++;
    //Serial.print(figPresent);
    //if(figIndex % 8 == 0) {;
    //  Serial.println();
    //}
  }
}
bool GetBitFromByte(uint8_t b, int bitNumber) {
    return ((b >> bitNumber) & 1) == 1;
}
void GetColorFromByte(uint8_t colorNumber) {
  // NOT RGB, but GRB for some reason
  switch(colorNumber) {
    case 0:
      boardColor = CRGB::Black;
      break;
    case 0b100:
      boardColor = CRGB(148, 0, 198); // possible moves
      break;
    case 0b001:
      boardColor = CRGB(0, 255, 0); // king checkmated
      break;
    case 0b101:
      boardColor = CRGB(22, 148, 192);  // figs that can stop checkmate
      break;
    case 0b010:
      boardColor = CRGB(64, 0, 0); // last move start
      break;
    case 0b110:
      boardColor = CRGB(255, 0, 0); // last move end
      break;
    case 0b011:
      boardColor = CRGB(0, 128, 64); // possible moves; capture
      break;
  }
}