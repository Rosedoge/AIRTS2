using UnityEngine;
using System.Collections;

public class OpenByButton : MonoBehaviour {

    MenuWindow MenuObject;
    // Use MenuManager type instead MenuWindow of if  you want  to operate  with whole menu system on scene
    KeyCode buttonCode = KeyCode.Escape;
	
	// Update is called once per frame
	void Update () {
		   if (Input.GetKeyUp(buttonCode)) {

               if (MenuObject) { MenuObject.enabled = !MenuObject.enabled; }
               else {
                   MenuWindow Script = gameObject.GetComponent<MenuWindow>();
                   if (Script) { Script.enabled = !Script.enabled; }
                   else { Debug.Log("Sorry but there no MenuWindow script attached to current object and no assigned to ", MenuObject); }
               }
			  
			// if (Time.timeScale == 0) Time.timeScale = 1; else Time.timeScale = 0;
		}
	}
}
