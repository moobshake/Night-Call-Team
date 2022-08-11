using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    public GameObject Popup;
    public Button open;


    public void Toggle(){
        if(Popup != null){
            Vector3 current = Popup.transform.localScale;
            Vector3 hide = new Vector3(0,0,0);
            Vector3 show = new Vector3(1,1,1);

            if(current == hide){
                Popup.transform.localScale = show;
            }else{
                Popup.transform.localScale = hide;
            }
        }
    }
}