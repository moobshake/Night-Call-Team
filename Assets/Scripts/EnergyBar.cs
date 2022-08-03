using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyBar : MonoBehaviour
{
    public Slider energyBar;
    public Energy energy;

    void Start()
    {
        energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<Energy>();
        energyBar = GetComponent<Slider>();
        energyBar.maxValue = energy.energyLevelMax;
        energyBar.value = energy.energyLevel;
    }

    public void SetEnergy(int hp){
        energyBar.value = hp;
    }
}