using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Energy : MonoBehaviour
{
    private TextMeshProUGUI textEnergy;
    public bool isEnergyUpdated;
    public bool isPlayerFainted;
    public int energyLevel;
    public int energyLevelMax;
    public EnergyBar energyBar;
    GameObject clock;
    GameObject player;
    GameObject prompt;
    GameObject alert;

    // Start is called before the first frame update
    void Start()
    {
        textEnergy = GetComponent<TextMeshProUGUI>();
        textEnergy.text = energyLevel.ToString() + "/100";
        clock = GameObject.FindGameObjectWithTag("Clock");
        player = GameObject.FindGameObjectWithTag("Doctor");
        prompt = GameObject.FindGameObjectWithTag("Prompt");
        alert = GameObject.FindGameObjectWithTag("Alert");
        isEnergyUpdated = true;
        isPlayerFainted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (clock.GetComponent<Clock>().minusHealth) {
            if (energyLevel != 0)
            {
                energyLevel -= 1;
                energyBar.SetEnergy(energyLevel);
            }

            if (energyLevel <= 100 && energyLevel > 75 && (player.GetComponent<SC_TPSController>().speed != player.GetComponent<SC_TPSController>().initialSpeed))
            {
                player.GetComponent<SC_TPSController>().speed = player.GetComponent<SC_TPSController>().initialSpeed;
                alert.GetComponent<Alert>().alertLevel = 4;
                alert.GetComponent<Alert>().isAlertUpdated = false;
            }
            else if (energyLevel <= 75 && energyLevel > 50 && (player.GetComponent<SC_TPSController>().speed != player.GetComponent<SC_TPSController>().initialSpeed / 1.5f))
            {
                player.GetComponent<SC_TPSController>().speed = player.GetComponent<SC_TPSController>().initialSpeed / 1.5f;
                alert.GetComponent<Alert>().alertLevel = 3;
                alert.GetComponent<Alert>().isAlertUpdated = false;
            }
            else if (energyLevel <= 50 && energyLevel > 25 && (player.GetComponent<SC_TPSController>().speed != player.GetComponent<SC_TPSController>().initialSpeed / 2))
            {
                player.GetComponent<SC_TPSController>().speed = player.GetComponent<SC_TPSController>().initialSpeed / 2;
                alert.GetComponent<Alert>().alertLevel = 2;
                alert.GetComponent<Alert>().isAlertUpdated = false;
            }
            else if (energyLevel <= 25 && energyLevel > 1 && (player.GetComponent<SC_TPSController>().speed != player.GetComponent<SC_TPSController>().initialSpeed / 3))
            {
                player.GetComponent<SC_TPSController>().speed = player.GetComponent<SC_TPSController>().initialSpeed / 3;
                alert.GetComponent<Alert>().alertLevel = 1;
                alert.GetComponent<Alert>().isAlertUpdated = false;
            }
            else if (energyLevel == 0 && !isPlayerFainted)
            {
                player.GetComponent<SC_TPSController>().speed = 0.0f;
                prompt.GetComponent<Prompt>().promptText = "Player fainted!";
                prompt.GetComponent<Prompt>().isPromptUpdated = false;
                isPlayerFainted = true;
                alert.GetComponent<Alert>().alertLevel = 0;
                alert.GetComponent<Alert>().isAlertUpdated = false;
            }

            textEnergy.text = energyLevel.ToString() + "/100";
            clock.GetComponent<Clock>().minusHealth = false;
        }
        if (!isEnergyUpdated)
        {
            if (energyLevel > 100)
            {
                energyLevel = 100;
                energyBar.SetEnergy(energyLevel);
            }
            textEnergy.text = energyLevel.ToString() + "/100";
            energyBar.SetEnergy(energyLevel);
            isEnergyUpdated = true;
            isPlayerFainted = false;
        }
    }
}
