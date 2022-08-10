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
    public bool timerIsRunning = false;

    private int originalTimeS;
    private int originalTimeM;
    private float previousMinus;

    void Start()
    {
        textClock = GetComponent<TextMeshProUGUI>();
        minusHealth = false;
        timerIsRunning = true;
        originalTimeM = Mathf.FloorToInt(timeRemaining / 60);
        originalTimeS = Mathf.FloorToInt(timeRemaining % 60);
        previousMinus = timeRemaining;
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
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textClock.text = string.Format("{0:00}:{1:00} / {2:00}:{3:00}", minutes, seconds, originalTimeM, originalTimeS);
    }
}
