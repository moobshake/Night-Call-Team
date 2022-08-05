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
    private Quaternion sleepPos;

    // Location
    public GameObject[] beds;
    public bool [] occupied;

    // Helps with assigning patient info to prefab
    private Dictionary<string, string> details;
    private string[] textArray;
    private string[] fields;

    void Start()
    {   
        beds = GameObject.FindGameObjectsWithTag("Bed");
        occupied = new bool[beds.Length];
        sleepPos = Quaternion.Euler(-90,0,0);
        CreatePatient();
    }

    void CreatePatient(){
        PopulateDetails();
        Instantiate(patient, GetRoom(), sleepPos);
        PatientInfo patientInfo = (PatientInfo)patient.GetComponent("PatientInfo");
        patientInfo.Name = details["Name"];
        patientInfo.Age = details["Age"];
        patientInfo.Race = details["Race"];
        patientInfo.Gender = details["Gender"];
        patientInfo.Condition = details["Condition"];
    }

    Vector3 GetRoom(){
        int i = 0;

        foreach (GameObject bed in beds)
        {   
            if(!occupied[i]){
                Vector3 offset = new Vector3(0.0f, 0.7f, 0.0f);
                Vector3 bedpos = bed.transform.position + offset;
                occupied[i] = true;
                return bedpos;
            }
            i++;

            if(i == beds.Length){
                break;
            } 
        }
        return new Vector3(-10f, -10f, -10f);
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
