#include "TactTongue.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>


int commandArray[18];
char *array[6];
char buf[] = "[1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]:[10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10]";
int cElectrodeMap[5][4] = 
  {
    {0, 0, 1, 0},
    {2, 3, 4, 5},
    {6, 7, 8, 9},
    {10,11,12,13},
    {14,15,16,17}
  };

int on[5][4] = 
  {
    {0,0,0,0},
    {0,0,0,0},
    {0,0,0,0},
    {0,0,0,0},
    {0,0,0,0}
  };

int ele_array[18] = {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0};
int pw_array[18] = {10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10};
int pp_array[18] = {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9};
int ibn_array[18];
int ibp_array[18];
int obn_array[18];
//these are waveform parameter as described by Kurt Kaczmarek here https://www.sciencedirect.com/science/article/pii/S1026309811001702
//than can be changed to create a large range of sensations on the tongue.
int  PP[] = {10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10};
int  Pp[] = {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9};
int  IN[] = {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3};
int  IP[] = {150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150};
int  ON[] = {9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9};

TactTongue myTactTongue;

void getCommandArray(){
    char *p = strtok (buf, ":");
    int count = 0; int i =0;
    
    while (p != NULL)
    {
        array[i++] = p;
        p = strtok (NULL, ":");
    }
}


char* substr(const char *src, int m, int n)
{
    // get the length of the destination string
    int len = n - m;
 
    // allocate (len + 1) chars for destination (+1 for extra null character)
    char *dest = (char*)malloc(sizeof(char) * (len + 1));
 
    // extracts characters between m'th and n'th index from source string
    // and copy them into the destination string
    for (int i = m; i < n && (*(src + i) != '\0'); i++)
    {
        *dest = *(src + i);
        dest++;
    }
 
    // null-terminate the destination string
    *dest = '\0';
 
    // return the destination string
    return dest - len;
}

void getValuesFromCommandArray(char* buf, int* valueArray){
    int count = 0; int i =0;
    char* commandArray = substr(buf, 1, strlen(buf)-1);
   // Serial.println(commandArray);
    char *p = strtok (commandArray, ",");
    char* temp;
    
    
    while (p != NULL)
    {
        if(isdigit(*p)){
            valueArray[i] = atoi(p);
          //  Serial.println(valueArray[i]);
           // printf(" i = %d, %d\n", i, valueArray[i]);
            i++;
        }
        p = strtok (NULL, ",");
    }
}



void left()
{
  on[1][0] = 1;on[2][0] = 1;on[3][0] = 1;on[4][0] = 1;
  on[1][1] = 1;on[2][1] = 1;on[3][1] = 1;on[4][1] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  } 
}

void right()
{
  on[1][3] = 1;on[2][3] = 1;on[3][3] = 1;on[4][3] = 1;
  on[1][2] = 1;on[2][2] = 1;on[3][2] = 1;on[4][2] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }

}

void up()
{
  on[1][0] = 1;on[1][2] = 1;on[2][0] = 1;on[2][2] = 1;
  on[1][1] = 1;on[1][3] = 1;on[2][1] = 1;on[2][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  } 
}



void waterfall_down(){

  on[1][0] = 1;on[1][1] = 1;on[1][2] = 1;on[1][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][0] = 1;on[2][1] = 1;on[2][2] = 1;on[2][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][0] = 1;on[3][1] = 1;on[3][2] = 1;on[3][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[4][0] = 1;on[4][1] = 1;on[4][2] = 1;on[4][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void diagnol1_down(){

  on[1][0] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][1] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][2] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[4][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void diagnol1_down(){

  on[4][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][2] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][1] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[1][0] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void diagnol2_up(){

  on[4][0] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][1] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][2] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[1][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void diagnol2_down(){

  on[1][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][2] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][1] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[4][0] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void waterfall_up(){

  on[4][0] = 1;on[4][1] = 1;on[4][2] = 1;on[4][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][0] = 1;on[3][1] = 1;on[3][2] = 1;on[3][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][0] = 1;on[2][1] = 1;on[2][2] = 1;on[2][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);


  on[1][0] = 1;on[1][1] = 1;on[1][2] = 1;on[1][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  delay(500);
}

void square(){
  on[1][0] = 1;on[1][2] = 1;on[2][0] = 1; on[2][3] = 1; on[4][0] = 1; on[4][2] = 1;
  on[1][1] = 1;on[1][3] = 1;on[3][0] = 1; on[3][3] = 1; on[4][1] = 1; on[4][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IN, IP, ON);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IN, IP, ON);
  }
  myTactTongue.Stimulate();
  convertIntArrayToString(commandArray);
  delay(500);
}

void updatepattern()
{
  //Take care of the first two elements outside the main square
    commandArray[cElectrodeMap[0][1]] = on[0][1];
   // Serial.print("array ");Serial.print(cElectrodeMap[i][j]);Serial.print("  is ");Serial.print("  is ");
    commandArray[cElectrodeMap[0][2]] = on[0][2];
//if the On array is ative, set the corresponding electrode active.
for (int j = 0; j<4;j++)
{
for (int i = 1; i<5;i++)
{
    commandArray[cElectrodeMap[i][j]] = on[i][j];
}
}
}

String direction;
void setup() {
  // put your setup code here, to run once:
  myTactTongue.Begin(); //Initialize Cthulhu library
  Serial.begin(9600); //Set serial output datarate

}
char lastCommand = "0";
String arrayString;
void convertIntArrayToString(int array[18]){
  arrayString = "[";
  for(int i = 0 ; i < 18;i++){
    arrayString += array[i];
  }
  arrayString += "]";
  Serial.println(arrayString);
 // Serial.write(arrayString);
 // Serial.write("\n");
}



void loop() {
  if(Serial.available()){
    String teststr = Serial.readString();

    if(teststr == "down"){
      Serial.write("down");
      Serial.write("\n");
      down();
    }
    elseif(teststr == "up"){
      Serial.write("up");
      Serial.write("\n");
      down();
    }
    elseif(teststr == "left"){
      Serial.write("left");
      Serial.write("\n");
      left();
    }
    elseif(teststr == "right"){
      Serial.write("right");
      Serial.write("\n");
      right();
    }
    elseif(teststr == "square"){
      Serial.write("square");
      Serial.write("\n");
      square();
    }

    else{
        teststr.toCharArray(buf, strlen(buf)+1);
        getCommandArray();
        getValuesFromCommandArray(array[0], ele_array);
        getValuesFromCommandArray(array[1], pp_array);
      // convertIntArrayToString(ele_array);
      // convertIntArrayToString(pp_array);
      // square();
        for(int i = 0 ; i < 18;i++){
          Serial.write(ele_array[i]);
          Serial.write("\n");
        }
        
        myTactTongue.UpdateStimuli(ele_array, pp_array, Pp, IN, IP, ON);
        myTactTongue.Stimulate();

    }
  
  }
  myTactTongue.UpdateStimuli(ele_array, pp_array, Pp, IN, IP, ON);
  myTactTongue.Stimulate();
  
  delay(100);
}
