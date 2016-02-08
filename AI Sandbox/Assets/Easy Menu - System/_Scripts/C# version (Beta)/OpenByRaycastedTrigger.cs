using UnityEngine;
using System.Collections;

public class OpenByRaycastedTrigger : MonoBehaviour {

    MenuWindow MenuObject;  
    GameObject TriggerObject;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) RaycastTrigger();
	}

    void RaycastTrigger() {
		Ray castRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		castRay.direction = castRay.direction.normalized;
			      
		 RaycastHit hitInfo;
		 
			      if (Physics.Raycast(castRay, out hitInfo)) 
			            {
			              if(hitInfo.collider.gameObject == TriggerObject) OpenCloseMenu ();
			               Debug.DrawRay(castRay.origin, castRay.direction*100, Color.red);
						}
						
    }

    //-----------------------------------------------------------------						
    void OpenCloseMenu() {
	   
		   if (MenuObject)
			    MenuObject.enabled = !MenuObject.enabled;
			   else
			    {
			      MenuWindow pauseScript = gameObject.GetComponent<MenuWindow>();
			      if (pauseScript)  pauseScript.enabled = !pauseScript.enabled;
			        else
			          Debug.Log ("Sorry but there is no MenuWindow script attached to current object and no assigned to ", MenuObject);
			    }
			  
	
    }
}
