using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Popup : MonoBehaviour
{

    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {   
        Instantiate(popup);
    }

    void PopulateBody()
    {   
    }
}
