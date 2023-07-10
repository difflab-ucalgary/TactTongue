#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <stdlib.h>

int NUM_CHANNELS = 18;
int NUM_PARAMETER_ARRAY = 6;

struct {
    int *ele_array;
    int *pp_array;
    int *PP_array;
    int *ibn_array;
    int *ibp_array;
    int *obn_array;
    
}tactongue_command;

void init_tactongue_command(){
    tactongue_command.ele_array = (int*)malloc(NUM_CHANNELS);
    tactongue_command.pp_array = (int*)malloc(NUM_CHANNELS);
    tactongue_command.PP_array = (int*)malloc(NUM_CHANNELS);
    tactongue_command.ibn_array = (int*)malloc(NUM_CHANNELS);
    tactongue_command.ibp_array = (int*)malloc(NUM_CHANNELS);
    tactongue_command.obn_array = (int*)malloc(NUM_CHANNELS);
}

int main ()
{
    char buf[] ="[1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]:[5,5,5]:[7,7,7]:[150,150,150]:[5,5,5]:[1500,1500,1500]";
    int i,j = 0;
    char *p = strtok (buf, ":");
    //char **array =  (char**)malloc(NUM_PARAMETER_ARRAY);
    char *array[6];
    char *endptr, *ptr;
    int count = 0;
    long numbers[512];
    
    init_tactongue_command();
    

    while (p != NULL)
    {
        array[i++] = p;
        p = strtok (NULL, ":");
    }

    for (i = 0; i < NUM_PARAMETER_ARRAY; ++i){
        printf("%s\n", array[i]);
        printf("%d\n", (int)strlen(array[i]));
    }
    ptr= array[0];
    endptr = array[0];
    i=0;
    count= 0;
    // for(j = 0 ; j < NUM_PARAMETER_ARRAY;j++){
    //     while (array[j][i] != '\0'){
    //         if (isdigit(array[j][i])){
    //             switch(j){
    //                 case 0: tactongue_command.ele_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
    //                 case 1: tactongue_command.pp_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
                        
    //                 case 2: tactongue_command.PP_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
    //                 case 3: tactongue_command.ibn_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
                        
    //                 case 4: tactongue_command.ibp_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
                        
    //                 case 5: tactongue_command.obn_array[count++] = atoi(&array[j][i]);
    //                     i++;
    //                     break;
    //             }
                
                        
                
    //         }
    //         else{
    //             i++;
    //         }
            
    //     }
        
    // }
    
    
    
    while (array[0][i] != '\0') {
    if (isdigit(array[0][i])) {
        tactongue_command.ele_array[count++] = atoi(&array[0][i]);
        i++;
        
    } else {
        i = i + 1;
    }
}

    count = 0;
    i=0;
    while (array[1][i] != '\0') {
    if (isdigit(array[1][i])) {
        tactongue_command.pp_array[count++] = atoi(&array[1][i]);
        i++;
        
    } else {
        i = i + 1;
    }
}
    for(i = 0; i < NUM_CHANNELS; i++){
        printf("%d\n", (int)tactongue_command.ele_array[i]);
        printf("%d\n", (int)tactongue_command.pp_array[i]);
    }
    
        
    return 0;
}
