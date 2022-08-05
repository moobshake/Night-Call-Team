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
    private float current;

    void Awake()
    {
        textClock = GetComponent<TextMeshProUGUI>();
        current = Time.time;
        minusHealth = false;
    }

    // Update is called once per frame
    void Update()
    {
        DateTime time = DateTime.Now;
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Minute);
        string second = LeadingZero(time.Second);
        textClock.text = hour + ":" + minute + ":" + second;

        if (Time.time - current >= secondsToMinusHealth && !minusHealth)
        {
            minusHealth = true;
            current = Time.time;
        }
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
