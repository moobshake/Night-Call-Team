using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Death : MonoBehaviour
{
    private TextMeshProUGUI textDeath;
    public int deathLevel = 0;
    public bool isDeathUpdated = true;

    // Start is called before the first frame update
    void Start()
    {
        textDeath = GetComponent<TextMeshProUGUI>();
        textDeath.text = deathLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeathUpdated)
        {
            textDeath.text = deathLevel.ToString();
            isDeathUpdated = true;
        }
    }
}
