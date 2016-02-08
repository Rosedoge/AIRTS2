using UnityEngine;
using System.Collections;
/// <summary>
/// Player script.
/// This script is based on the Game Controller
/// </summary>
public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// 
	/// </summary>
	/// 

	GameObject selected;
	RaycastHit hit;


	// Use this for initialization
	void Start () {
		selected = new GameObject ();
		selected.AddComponent <WorkerScript>();
		//selected = null;
	}

	void MouseClick(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			if (selected == null) {
				if (hit.collider.gameObject.GetComponent<WorkerScript> ()) {
					selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<WorkerScript> ().selected = true;
				}
				if (hit.collider.gameObject.GetComponent<TankScript> ()) {
					selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<TankScript> ().selected = true;
				}
			} else {
				//selected.gameObject.GetComponent<WorkerScript> ().
				if (selected.gameObject.GetComponent<WorkerScript> ()) {
					selected.gameObject.GetComponent<WorkerScript> ().Move (hit);
				}else if (selected.gameObject.GetComponent<TankScript> ()) {
					//selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<TankScript> ().Move (hit);
				}


			}




		} // end of Physics if

	}

	void RightClick(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.name == "Terrain") {
				if (selected != null && selected.gameObject.GetComponent<WorkerScript> ())
					selected.gameObject.GetComponent<WorkerScript> ().selected = false;
				if (selected != null && selected.gameObject.GetComponent<TankScript> ())
					selected.gameObject.GetComponent<TankScript> ().selected = false;
				selected = null;

			}else if (hit.collider.gameObject.name == "Mine") {
				if (selected != null && selected.gameObject.GetComponent<WorkerScript> ()) {
					Debug.Log ("Get to work");
					selected.gameObject.GetComponent<WorkerScript> ().SetWork(hit.collider.gameObject);
				}
				selected.gameObject.GetComponent<WorkerScript> ().selected = false;
				selected = null;

			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (selected.gameObject.name);
		if (Input.GetMouseButtonDown (0)) {
			MouseClick ();

		} else if (Input.GetMouseButtonDown (1)) {
				RightClick();
		}
	
	}
}
