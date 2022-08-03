using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompt : MonoBehaviour
{
    private TextMeshProUGUI prompt;
    public bool isPromptUpdated;
    private float currentTime;
    public int promptFadeTime;
    public string promptText;

    // Start is called before the first frame update
    void Start()
    {
        prompt = GetComponent<TextMeshProUGUI>();
        prompt.text = "";
        isPromptUpdated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPromptUpdated)
        {
            prompt.text = promptText;
            isPromptUpdated = true;
            currentTime = Time.time;
        }
        if (isPromptUpdated && Time.time - currentTime > promptFadeTime)
        {
            prompt.text = "";
        }
    }
}
