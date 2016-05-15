using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Player script.
/// This script is based on the Game Controller
/// </summary>
public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// 
	/// </summary>
	/// 
	public GameObject TownHall;
	public Text UIText;
	GameObject selected;
	RaycastHit hit;
	int layerMask;


	// Use this for initialization
	void Start () {
        selected = null;// new GameObject ();
		//selected.AddComponent <WorkerScript>();
		//selected.AddComponent <NavMeshAgent>();
		//selected = null;
		// Bit shift the index of the layer (8) to get a bit mask
		layerMask = 1 << 8;

		// This would cast rays only against colliders in layer 8.
		// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;
	}

	void MouseClick(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			Debug.Log (hit.collider.gameObject.name);
			if (selected == null) {
				if (hit.collider.gameObject.GetComponent<WorkerScript> ()) {
					selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<WorkerScript> ().selected = true;
				} else if (hit.collider.gameObject.GetComponent<SoldierScript> ()) {
					selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<SoldierScript> ().selected = true;
				} else if (hit.collider.gameObject.GetComponent<TownHall> ()) {
					selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<TownHall> ().selected = true;
				}
			} else {
				//selected.gameObject.GetComponent<WorkerScript> ().
				if (selected.gameObject.GetComponent<WorkerScript> ()) {
					selected.gameObject.GetComponent<WorkerScript> ().Move (hit);
				}else if (selected.gameObject.GetComponent<SoldierScript> ()) {
					//selected = hit.collider.gameObject;
					selected.gameObject.GetComponent<SoldierScript> ().Move (hit);
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
				if (selected != null && selected.gameObject.GetComponent<SoldierScript> ())
					selected.gameObject.GetComponent<SoldierScript> ().selected = false;
				selected = null;

			} else if (hit.collider.gameObject.name == "Mine" ||hit.collider.gameObject.tag == "Tree") {
				if (selected != null && selected.gameObject.GetComponent<WorkerScript> ()) {
					Debug.Log ("Get to work");
					selected.gameObject.GetComponent<WorkerScript> ().SetWork (hit.collider.gameObject);
				}
				selected.gameObject.GetComponent<WorkerScript> ().selected = false;
				selected = null;
			} else if (hit.collider.gameObject.tag == "Enemy") {
				if (selected != null && selected.gameObject.GetComponent<SoldierScript> ()) {
					selected.gameObject.GetComponent<SoldierScript> ().selected = false;
					selected.gameObject.GetComponent<SoldierScript> ().CurTask = SoldierScript.Task.Fighting;
					selected.gameObject.GetComponent<SoldierScript> ().Attack (hit.collider.gameObject);
				}
			} else {
				Debug.Log(hit.collider.gameObject.name);

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
		UpdateUI ();
	
	}

	void UpdateUI(){
        try
        {
            UIText.text = "Wood: " + TownHall.gameObject.GetComponent<TownHall>().Wood + "   Stone: " + TownHall.gameObject.GetComponent<TownHall>().Stone;
        }
        catch { }

	}
}
