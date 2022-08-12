using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI textClock;
    public int secondsToMinusHealth;
    public bool minusHealth;
    public float timeRemaining;
    public bool timerIsRunning;
    public Button startButton;
    public Button restartButton;
    private float originalTime;
    private int originalTimeS;
    private int originalTimeM;
    private float previousMinus;

    GameObject patientSpawner;

    public TextMeshProUGUI summarySavedText;
    public TextMeshProUGUI summaryDieText;

    void Start()
    {
        originalTime = timeRemaining;
        patientSpawner = GameObject.FindGameObjectWithTag("PatientSpawner");
        textClock = GetComponent<TextMeshProUGUI>();
        minusHealth = false;
        timerIsRunning = false;
        originalTimeM = Mathf.FloorToInt(timeRemaining / 60);
        originalTimeS = Mathf.FloorToInt(timeRemaining % 60);
        previousMinus = timeRemaining;
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(StartGame);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if (Mathf.FloorToInt((previousMinus - timeRemaining) % 60) >= secondsToMinusHealth && !minusHealth) {
                    minusHealth = true;
                    previousMinus = timeRemaining;
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                EndGame();
            }
        }
    }

    void EndGame()
    {
        patientSpawner.GetComponent<PatientSpawner>().gameStarted = false;

        GameObject[] patients = GameObject.FindGameObjectsWithTag("Patient");
        foreach (GameObject p in patients)
        {
            Destroy(p.gameObject);
        }
        GameObject wellnessCount = GameObject.FindGameObjectWithTag("WellnessCount");
        GameObject deathCount = GameObject.FindGameObjectWithTag("DeathCount");

        summarySavedText.text = "You have saved: " + wellnessCount.GetComponent<Wellnes>().wellnessLevel.ToString();
        summaryDieText.text = "Unable to treat: " + deathCount.GetComponent<Death>().deathLevel.ToString();
        restartButton.gameObject.SetActive(true);
        Button rBtn = restartButton.GetComponent<Button>();
        rBtn.onClick.AddListener(Restart);
    }

    void Restart()
    {
        timeRemaining = originalTime;
        restartButton.gameObject.SetActive(false);

        GameObject energy = GameObject.FindGameObjectWithTag("Energy");
        energy.GetComponent<Energy>().energyLevel = energy.GetComponent<Energy>().energyLevelMax;
        energy.GetComponent<Energy>().isEnergyUpdated = false;

        GameObject wellnessCount = GameObject.FindGameObjectWithTag("WellnessCount");
        wellnessCount.GetComponent<Wellnes>().wellnessLevel = 0;
        wellnessCount.GetComponent<Wellnes>().isWellnessUpdated = false;

        GameObject deathCount = GameObject.FindGameObjectWithTag("DeathCount");
        deathCount.GetComponent<Death>().deathLevel = 0;
        deathCount.GetComponent<Death>().isDeathUpdated = false;

        timerIsRunning = true;
        patientSpawner.GetComponent<PatientSpawner>().StartGame();
        patientSpawner.GetComponent<PatientSpawner>().gameStarted = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textClock.text = string.Format("{0:00}:{1:00} / {2:00}:{3:00}", minutes, seconds, originalTimeM, originalTimeS);
    }

    void StartGame()
    {
        timerIsRunning = true;
        startButton.gameObject.SetActive(false);
        patientSpawner.GetComponent<PatientSpawner>().StartGame();
        patientSpawner.GetComponent<PatientSpawner>().gameStarted = true;
    }
}
