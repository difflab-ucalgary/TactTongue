#include <ArduinoJson.h>
#include <avr/pgmspace.h>
#include "TactTongue.h"

static int NUM_ELECTRODES = 18;
TactTongue myTactTongue;


void initializeStimuliArray(int stimuli_array[18], int value){
  for(int i = 0 ; i< NUM_ELECTRODES; i++){
    stimuli_array[i] = value;
  }
  return;
}



int ele[18] = {1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0};
int PP[18];
int Pp[18];
int IBN[18];
int IBP[18];
int OBN[18];

int ibn_value_json, ibp_value_json, obn_value_json, PP_value_json;
int myerror;
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
int electrode_info = doc["electrode_info"]; // 1
if(electrode_info ==1){
  JsonArray trodes = doc["ele"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    ele[i] = trodes[i];
  }
  Serial.print(ele[0]);

}else{
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

  JsonArray Pp_values = doc["Pp"];
  for(int i = 0 ; i < NUM_ELECTRODES; i++){
    Pp[i] = Pp_values[i];
  }




}



}
