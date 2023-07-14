/* Online C Compiler and Editor */
#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <stdlib.h>

int NUM_CHANNELS = 18;
int NUM_PARAMETER_ARRAY = 6;
char buf[] ="[1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]:[10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10]:[9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9]:[3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3]:[150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150]:[5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5]";
char *array[6];


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
    printf("%s\n", commandArray);
    char *p = strtok (commandArray, ",");
    char* temp;
    
    
    while (p != NULL)
    {
        if(isdigit(*p)){
            valueArray[i] = atoi(p);
            printf(" i = %d, %d\n", i, valueArray[i]);
            i++;
        }
        p = strtok (NULL, ",");
    }
}
int main()
{
    int i = 0, count=0;
   // init_tactongue_command();
    getCommandArray();
    int ele_array[18];
    int pw_array[18];
    int pp_array[18];
    int ibn_array[18];
    int ibp_array[18];
    int obn_array[18];
   
    getValuesFromCommandArray(array[0], ele_array);
    getValuesFromCommandArray(array[1], pw_array);
    getValuesFromCommandArray(array[2], pp_array);
    getValuesFromCommandArray(array[3], ibn_array);
    getValuesFromCommandArray(array[4], ibp_array);
    getValuesFromCommandArray(array[5], obn_array);
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", ele_array[i]);
    }
    printf("\n");
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", pw_array[i]);
    }
    printf("\n");
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", pp_array[i]);
    }
    printf("\n");
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", ibn_array[i]);
    }
    
    printf("\n");
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", ibp_array[i]);
    }
    printf("\n");
    
    for(i=0; i < NUM_CHANNELS; i++){
        printf("%d, ", obn_array[i]);
    }
    
    
    
    return 0;
}
