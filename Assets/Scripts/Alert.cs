using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour
{
    private TextMeshProUGUI textAlert;
    public int alertLevel;
    public int alertMax;
    public AlertBar alertBar;
    public bool isAlertUpdated = true;
    //GameObject clock;
    //GameObject player;
    //GameObject prompt;
    //GameObject energy;

    // Start is called before the first frame update
    void Start()
    {
        textAlert = GetComponent<TextMeshProUGUI>();
        textAlert.text = alertLevel.ToString() + "/4";
        //clock = GameObject.FindGameObjectWithTag("Clock");
        //player = GameObject.FindGameObjectWithTag("Doctor");
        //prompt = GameObject.FindGameObjectWithTag("Prompt");
        //energy = GameObject.FindGameObjectWithTag("Energy");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlertUpdated)
        {
            textAlert.text = alertLevel.ToString() + "/4";
            alertBar.SetAlert(alertLevel);
            isAlertUpdated = true;
        }
    }
}
