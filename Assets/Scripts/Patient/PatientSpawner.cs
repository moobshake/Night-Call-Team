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
    public TextAsset patientFile;
    public GameObject patient;
    public GameObject[] patients;
    private Quaternion sleepPos;

    // Location
    public GameObject[] beds;
    public bool [] occupied;

    // Helps with assigning patient info to prefab
    private Dictionary<string, Patient> patientsAvail;
    private Dictionary<int, string> patientIndex;
    private string[] textArray;
    private string[] fields;

    void Start()
    {   
        beds = GameObject.FindGameObjectsWithTag("Bed");
        occupied = new bool[beds.Length];
        sleepPos = Quaternion.Euler(-90,0,0);
        CreatePatients();
    }

    void CreatePatients(){
        GetPatientInfo();

        // Create patient objects
        for(int i = 0; i<patientsAvail.Count; i++){
            Instantiate(patient, GetRoom(), sleepPos);
        }
        
        patients = GameObject.FindGameObjectsWithTag("Patient");

        // Populate patients with details
        for(int j=0; j<patients.Length-1; j++){
            PatientInfo info = (PatientInfo)patients[j].GetComponent("PatientInfo");
            string x = patientIndex[j];
            Patient p = patientsAvail[x];
            info.Name = p.Name;
            info.Age = p.Age;
            info.Race = p.Race;
            info.Gender = p.Gender;
            info.Condition = p.Condition;
        }

    }

    Vector3 GetRoom(){
        int i = 0;

        foreach (GameObject bed in beds)
        {   
            if(!occupied[i]){
                Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
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

    void GetPatientInfo(){
        Patients pData = JsonUtility.FromJson<Patients>(patientFile.text);
        patientsAvail = new Dictionary<string, Patient>();
        patientIndex = new Dictionary<int, string>();
        var index = 0;

        foreach(Patient p in pData.patients){
            patientsAvail.Add(p.Name, p);
            patientIndex.Add(index, p.Name);
            index++;
        }  
    }
    
}