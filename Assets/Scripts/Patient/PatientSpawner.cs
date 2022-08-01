using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatientSpawner : MonoBehaviour
{   
    public int wellnessCount = 0; 
    public int deathCount = 0;
    public int level = 1; 
    public TextAsset patientDetails;
    public Patient patient;

    // Start is called before the first frame update
    void Start()
    {   
        patient = new Patient();
        CreatePatient();
        // Instantiate(CreatePtient());
    }

    void CreatePatient(){
        print(patientDetails.text);
        JsonUtility.FromJsonOverwrite(patientDetails.text, patient);
        Instantiate(patient);
        Debug.Log(patient);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
