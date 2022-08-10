using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertBar : MonoBehaviour
{
    public Slider alertBar;
    public Alert alert;

    void Start()
    {
        alert = GameObject.FindGameObjectWithTag("Alert").GetComponent<Alert>();
        alertBar = GetComponent<Slider>();
        alertBar.maxValue = alert.alertMax;
        alertBar.value = alert.alertLevel;
    }

    public void SetAlert(int hp)
    {
        alertBar.value = hp;
    }
}
