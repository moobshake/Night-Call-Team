using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatientSpawner : MonoBehaviour
{   

    // General Patients Count
    public int wellnessCount = 0; 
    public int deathCount = 0;
    public int level = 1; 

    // Patient
    public TextAsset text;
    public GameObject patient;
    public GameObject[] patients;

    // Helps with assigning patient info to prefab
    private Dictionary<string, string> details;
    private string[] textArray;
    private string[] fields;

    void Start()
    {   
        CreatePatient();
    }

    void CreatePatient(){
        PopulateDetails();
        Instantiate(patient);
        PatientInfo patientInfo = (PatientInfo)patient.GetComponent("PatientInfo");
        patientInfo.Name = details["Name"];
        patientInfo.Age = details["Age"];
        patientInfo.Race = details["Race"];
        patientInfo.Gender = details["Gender"];
        patientInfo.Condition = details["Condition"];
    }

    void PopulateDetails(){
        textArray = text.text.Split (new char[] {'\r', '\n'});
        fields = new[]{"Name", "Age", "Race", "Gender", "Condition"};
        details = new Dictionary<string, string>();

        for (int i = 0; i < (textArray.Length-1); i++)
        {   
            details.Add(fields[i], textArray[i]);
        }
    }
    
}
