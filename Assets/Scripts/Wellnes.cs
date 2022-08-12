using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wellnes : MonoBehaviour
{
    private TextMeshProUGUI textWellness;
    public int wellnessLevel = 0;
    public bool isWellnessUpdated = true;

    // Start is called before the first frame update
    void Start()
    {
        textWellness = GetComponent<TextMeshProUGUI>();
        textWellness.text = wellnessLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWellnessUpdated)
        {
            textWellness.text = wellnessLevel.ToString();
            isWellnessUpdated = true;
        }
    }
}
