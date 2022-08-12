using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatientSpawner : MonoBehaviour
{
    public bool gameStarted = false;

    // Patient
    public TextAsset patientFile;
    public GameObject patient;
    public GameObject[] patients;
    private Quaternion sleepPos;

    // Location
    public GameObject[] beds;
    public bool[] occupied;

    private bool[] spawnPlace = new bool[] { true, true, false, false, true };

    // Helps with assigning patient info to prefab
    private Dictionary<string, Patient> patientsAvail;
    private Dictionary<int, string> patientIndex;

    private bool SpawnNew = false;

    GameObject deathCount;
    private float currentTime;
    public int SecondsToDie = 20;

    private void Start()
    {
        beds = GameObject.FindGameObjectsWithTag("Bed");
        deathCount = GameObject.FindGameObjectWithTag("DeathCount");
        sleepPos = Quaternion.Euler(-90, 0, 0);
    }

    public void StartGame()
    {
        CreatePatients();
    }

    void CreatePatients()
    {
        currentTime = Time.time;
        occupied = new bool[beds.Length];

        GetPatientInfo();

        Shuffle();

        // Create patient objects
        for (int i = 0; i < patientsAvail.Count; i++)
        {
            Instantiate(patient, GetRoom(), sleepPos);
        }

        patients = GameObject.FindGameObjectsWithTag("Patient");

        // Populate patients with details
        for (int j = 0; j < patients.Length; j++)
        {
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

    Vector3 GetRoom()
    {
        int i = 0;

        foreach (GameObject bed in beds)
        {
            if (!occupied[i] && spawnPlace[i])
            {
                Vector3 offset = new Vector3(0.0f, 0.4f, 1.0f);
                Vector3 bedpos = bed.transform.position + offset;
                occupied[i] = true;
                return bedpos;
            }
            i++;

            if (i == beds.Length)
            {
                break;
            }
        }
        return new Vector3(-10f, -10f, -10f);
    }

    void GetPatientInfo()
    {
        Patients pData = JsonUtility.FromJson<Patients>(patientFile.text);
        patientsAvail = new Dictionary<string, Patient>();
        patientIndex = new Dictionary<int, string>();
        var index = 0;

        foreach (Patient p in pData.patients)
        {
            patientsAvail.Add(p.Name, p);
            patientIndex.Add(index, p.Name);
            index++;
        }
    }

    private void Update()
    {
        patients = GameObject.FindGameObjectsWithTag("Patient");

        if (patients.Length == 0 && !SpawnNew && gameStarted)
        {
            SpawnNew = true;
        }
        else if (patients.Length == 0 && SpawnNew && gameStarted)
        {
            CreatePatients();
            SpawnNew = false;
        }
        else if (Time.time - currentTime > 20 && patients.Length != 0 && gameStarted)
        {
            foreach (GameObject p in patients)
            {
                Destroy(p.gameObject);
                deathCount.GetComponent<Death>().deathLevel += 1;
            }
            deathCount.GetComponent<Death>().isDeathUpdated = false;
        }
    }

    private void Shuffle()
    {
        for (int i = 0; i < spawnPlace.Length; i++)
        {
            int rnd = Random.Range(0, spawnPlace.Length);
            bool tempGO = spawnPlace[rnd];
            spawnPlace[rnd] = spawnPlace[i];
            spawnPlace[i] = tempGO;
        }
    }
}