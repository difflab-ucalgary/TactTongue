#include <ArduinoJson.h>
#include <avr/pgmspace.h>
#include "TactTongue.h"

static int NUM_ELECTRODES = 18;
TactTongue myTactTongue;

int ele[18] = {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0};
int PP[18];
int Pp[18];
int IBN[18];
int IBP[18];
int OBN[18];
int commandArray[18];
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

int ibn_value_json, ibp_value_json, obn_value_json, PP_value_json, Pp_value_json;
int myerror;
int direction_command;

void updatepattern()
{
  
  commandArray[cElectrodeMap[0][1]] = on[0][1];
  commandArray[cElectrodeMap[0][2]] = on[0][2];
  for (int j = 0; j<4;j++){
    for (int i = 1; i<5;i++){
      commandArray[cElectrodeMap[i][j]] = on[i][j];
    }
  }
}

void initializeStimuliArray(int stimuli_array[18], int value){
  for(int i = 0 ; i< NUM_ELECTRODES; i++){
    stimuli_array[i] = value;
  }
  return;
}

void left()
{
  on[1][0] = 1;on[2][0] = 1;on[3][0] = 1;on[4][0] = 1;
  on[1][1] = 1;on[2][1] = 1;on[3][1] = 1;on[4][1] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  } 
}

void right()
{
  on[1][3] = 1;on[2][3] = 1;on[3][3] = 1;on[4][3] = 1;
  on[1][2] = 1;on[2][2] = 1;on[3][2] = 1;on[4][2] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }

}

void up()
{
  on[1][0] = 1;on[1][2] = 1;on[2][0] = 1;on[2][2] = 1;
  on[1][1] = 1;on[1][3] = 1;on[2][1] = 1;on[2][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  } 
}

void down()
{
  on[3][0] = 1;on[3][2] = 1;on[4][0] = 1;on[4][2] = 1;
  on[3][1] = 1;on[3][3] = 1;on[4][1] = 1;on[4][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  } 
}


void waterfall_down(){

  on[1][0] = 1;on[1][1] = 1;on[1][2] = 1;on[1][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][0] = 1;on[2][1] = 1;on[2][2] = 1;on[2][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][0] = 1;on[3][1] = 1;on[3][2] = 1;on[3][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[4][0] = 1;on[4][1] = 1;on[4][2] = 1;on[4][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

}

void waterfall_up(){

  on[4][0] = 1;on[4][1] = 1;on[4][2] = 1;on[4][3] = 1;
  updatepattern();
  int myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[3][0] = 1;on[3][1] = 1;on[3][2] = 1;on[3][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[2][0] = 1;on[2][1] = 1;on[2][2] = 1;on[2][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);

  on[1][0] = 1;on[1][1] = 1;on[1][2] = 1;on[1][3] = 1;
  updatepattern();
  myerror = myTactTongue.CheckWaveform(commandArray, PP, Pp, IBN, IBP, OBN);
  if(myerror == 0){
     myTactTongue.UpdateStimuli(commandArray, PP, Pp, IBN, IBP, OBN);
  }
  myTactTongue.Stimulate();
  delay(500);
}

void setup() {
  // Initialize serial port
  myTactTongue.Begin();
  Serial.begin(9600);
  while (!Serial) continue;

  initializeStimuliArray(PP, 10);
  initializeStimuliArray(Pp, 4);
  initializeStimuliArray(IBN, 3);
  initializeStimuliArray(IBP, 150);
  initializeStimuliArray(OBN, 9);
}

void loop() {
  if (Serial.available() > 0){
    String stimuliCommand = Serial.readString();
    Serial.println("Got Command");
    Serial.println(stimuliCommand);
    Serial.print("Received Command: ");
    Serial.println(stimuliCommand);  
    deserializeJson(stimuliCommand);
    myerror = myTactTongue.CheckWaveform(ele, PP, Pp, IBN, IBP, OBN);
    if(myerror == 0){
     myTactTongue.UpdateStimuli(ele, PP, Pp, IBN, IBP, OBN);
     myTactTongue.Stimulate();
    }
  }else{
    myerror = myTactTongue.CheckWaveform(ele, PP, Pp, IBN, IBP, OBN);
    if(myerror == 0){
     myTactTongue.UpdateStimuli(ele, PP, Pp, IBN, IBP, OBN);
     myTactTongue.Stimulate();
    }
  }
    delay(10);
}

void deserializeJson(String stimuliCommand){
  StaticJsonDocument<1024> doc;

  DeserializationError error = deserializeJson(doc, stimuliCommand);

  if (error) {
  Serial.print(F("deserializeJson() failed: "));
  Serial.println(error.f_str());
  return;
}
int msg_type = doc["msg_type"]; // 1
if(msg_type ==1){
  JsonArray trodes = doc["ele"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    ele[i] = trodes[i];
  }
  Serial.print(ele[0]);

}else if(msg_type == 0) {
  ibn_value_json = doc["IBN"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    IBN[i] = ibn_value_json;
  }

  ibp_value_json = doc["IBP"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    IBP[i] = ibp_value_json;
  }

  obn_value_json = doc["OBN"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    OBN[i] = obn_value_json;
  }

  PP_value_json = doc["PP"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    PP[i] = PP_value_json;
  }

  Pp_value_json = doc["Pp"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    Pp[i] = Pp_value_json;
  }
}else if(msg_type == 2) {
  JsonArray Pp_values_json = doc["Pp"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    Pp[i] = Pp_values_json[i];
  }

}else if(msg_type == 3){
  direction_command = doc["direction_command"];
  if(direction_command == 0){
    left();
  }
  if(direction_command == 1){
    right();
  }
  if(direction_command == 2){
    up();
  }
  if(direction_command == 3){
    down();
  }
  if(direction_command == 12){
    waterfall_up();
  }
  if(direction_command == 13){
    waterfall_down();
  }

}else if(msg_type == 4){
  Pp_value_json = doc["Pp"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    Pp[i] = Pp_value_json;
  }

  PP_value_json = doc["PP"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    PP[i] = PP_value_json;
  }

  ibn_value_json = doc["IBN"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    IBN[i] = ibn_value_json;
  }

  ibp_value_json = doc["IBP"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    IBP[i] = ibp_value_json;
  }

  obn_value_json = doc["OBN"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    OBN[i] = obn_value_json;
  }

}



}
