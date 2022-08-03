using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TogglePopup : MonoBehaviour
{   
    public GameObject Popup;
    public Button open;

    public void ToggleOpen(){
        if(Popup != null){
            Popup.SetActive(true);
            open.gameObject.SetActive(false);

        }
    }

    public void ToggleClose(){
        if(Popup != null){
            Popup.SetActive(false);
            open.gameObject.SetActive(true);
        }

    }
}
