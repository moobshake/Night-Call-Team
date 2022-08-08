using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreatmentOptions : MonoBehaviour
{   
    public Button[] options;

    void Start()
    {
        options = GetComponentsInChildren<Button>();
    }

    void Update(){
        MakeChoice();
    }

    void MakeChoice(){
        
        if(Input.GetKeyDown(KeyCode.U)){
            // options[0].onClick.Invoke();
            ChosenTreatment("Start Oxygen");
        }else if(Input.GetKeyDown(KeyCode.I)){
            print("Clicked 2");
            // options[1].onClick.Invoke();
            ChosenTreatment("Start Fluid");
        }else if(Input.GetKeyDown(KeyCode.O)){
            // options[2].onClick.Invoke();
            ChosenTreatment("Capillary Blood Glucose");
        }else if(Input.GetKeyDown(KeyCode.P)){
            // options[3].onClick.Invoke();
            ChosenTreatment("Administer Medicine");
        }
        
        
    }

    public void ChosenTreatment(string choice)
    {
        GameObject[] patient = GameObject.FindGameObjectsWithTag("Patient");
        PatientInfo p = patient[0] as PatientInfo;
        print(p.Condition);
    }


}
