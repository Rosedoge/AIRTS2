using UnityEngine;
using System.Collections;

public class WorkerScript : MonoBehaviour {
	Behaviour halo;// = (Behaviour)GetComponent("Halo");
	public bool selected = false;
	public enum Task {None,Working,Fighting,Guard,Scared};
	public Task CurTask;

	//Work Related
	GameObject TargetObject;
	GameObject Home;
	float Lasttime;
	int CarryingResource = 0;
	public bool AtResource = false, Feared = false;
	public bool AtDrop = false;
	string ResourceTarget;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Mine") {
			AtResource = true;
			//Debug.Log ("STILL HERE LOL");
		}
		if (col.gameObject.name == "Tree") {
			AtResource = true;
			//Debug.Log ("STILL HERE LOL");
		}
		if (col.gameObject.name == "TownHall") {
			AtDrop = true;
		}
//		if (col.gameObject.tag == "Enemy") {
//			CurTask = Task.Scared;
//			Feared = true;
//		}


	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.name == "Mine") {
			AtResource = false;
			//Debug.Log ("Left the mine!");
		}
		if (col.gameObject.name == "Tree") {
			AtResource = false;
			//Debug.Log ("Left the mine!");
		}
		if (col.gameObject.name == "TownHall") {
			AtDrop = false;
		}
		if (col.gameObject.tag == "Enemy") {
			CurTask = Task.None;
			Feared = false;
		}

	}
	void Start () {
		//gameObject.GetComponent<Halo
		Home = GameObject.FindGameObjectWithTag("TownHall");
        try
        {
            halo = (Behaviour)GetComponent("Halo");
            halo.enabled = false;
        }
        catch { }
		CurTask = Task.None;
	}

	public void Move(RaycastHit hit){
		this.gameObject.GetComponent<NavMeshAgent> ().destination = hit.point;
		Debug.Log ("Stop Working");
		CurTask = Task.None;
	}

	public void SetWork(GameObject tar){
		CurTask = Task.Working;
		TargetObject = tar;
		if (tar.gameObject.name == "Mine") {
			ResourceTarget = "Mine";
		} else if (tar.gameObject.tag == "Tree") {
			ResourceTarget = "Tree";


		}
	}


	void MouseClick(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (selected) { //Is a Selected Unity
			
			if (Physics.Raycast (ray, out hit)) {
				this.gameObject.GetComponent<NavMeshAgent> ().destination = hit.point;
				//Debug.Log ("Tar is: " + hit.transform.position);
//			newPosition = hit.point;
//			transform.position = newPosition;
			}
		} else {
			Physics.Raycast (ray, out hit);
			if (hit.transform.position.x == gameObject.transform.position.x && hit.transform.position.y == gameObject.transform.position.y) {
				selected = true;


				}
		}

	}
	/// <summary>
	/// Determine whether or not the object is at the TownHall or if it's at the mine
	/// 
	/// </summary>

	void Work(){
		if (ResourceTarget == "Mine") {
			Debug.Log ("Base " + AtDrop + " Or Mine " + AtResource);
			if (AtDrop) {
				if (CarryingResource <= 0) {
					//Gotta go back to work
					this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
				} else {
					Debug.Log ("Dropping stuff off, b0ss");
					Home.gameObject.GetComponent<TownHall> ().TakeResources ("Stone", CarryingResource);
					CarryingResource = 0;
				}

			} else if (AtResource) {
				Debug.Log ("Trying to Mine");
				if (CarryingResource >= 10) {
					//Gotta go back to base
					//this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
					this.gameObject.GetComponent<NavMeshAgent> ().destination = Home.gameObject.transform.position;
					Debug.Log ("Going to drop this off " + CarryingResource);
				} else {
					//Gotta Get Mine Material
					if (TargetObject.gameObject.GetComponent<MineScript> ()) { // It is a rock
						if (Time.time - Lasttime > 1) {
							if (TargetObject.gameObject.GetComponent<MineScript> ().GainRock ()) {
								this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
								TargetObject.gameObject.GetComponent<MineScript> ().GainRock ();
								CarryingResource += 1;
								Debug.Log (CarryingResource);
								Lasttime = Time.time;
							} else { //empty, gotta go


							}
						}
					}
				}
			} else {
				if (CarryingResource <= 0) {
					this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
				} else if (CarryingResource > 0) {
					this.gameObject.GetComponent<NavMeshAgent> ().destination = Home.gameObject.transform.position;
					Debug.Log ("Going to drop this off " + CarryingResource);
				}
			}
		} else if (ResourceTarget == "Tree") {
			Debug.Log ("Base " + AtDrop + " Or Tree " + AtResource);
			if (AtDrop) {
				if (CarryingResource <= 0) {
					//Gotta go back to work
					this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
				} else {
					Debug.Log ("Dropping stuff off, b0ss");
					Home.gameObject.GetComponent<TownHall> ().TakeResources ("Wood", CarryingResource);
					CarryingResource = 0;
				}

			} else if (AtResource) {
				Debug.Log ("Trying to Chop");
				if (CarryingResource >= 10) {
					//Gotta go back to base
					//this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
					this.gameObject.GetComponent<NavMeshAgent> ().destination = Home.gameObject.transform.position;
					Debug.Log ("Going to drop this off " + CarryingResource);
				} else {
					//Gotta Get Mine Material
					if (TargetObject.gameObject.GetComponent<WoodScript> ()) { // It is a rock
						if (Time.time - Lasttime > 1) {
							if (TargetObject.gameObject.GetComponent<WoodScript> ().GainWood ()) {
								this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
								TargetObject.gameObject.GetComponent<WoodScript> ().GainWood ();
								CarryingResource += 1;
								Debug.Log (CarryingResource);
								Lasttime = Time.time;
							} else { //empty, gotta go


							}
						}
					}
				}
			} else {
				if (CarryingResource <= 0) {
					this.gameObject.GetComponent<NavMeshAgent> ().destination = TargetObject.gameObject.transform.position;
				} else if (CarryingResource > 0) {
					this.gameObject.GetComponent<NavMeshAgent> ().destination = Home.gameObject.transform.position;
					Debug.Log ("Going to drop this off " + CarryingResource);
				}
			}

		}


	}
	void Update()
	{
		this.gameObject.GetComponent<NavMeshAgent> ().speed = 3.5f;
		if (CurTask == Task.None) {
			this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);


		}
		if ((CurTask == Task.Scared) && !Feared) {
			Feared = true;
			this.gameObject.GetComponent<NavMeshAgent> ().destination = Home.gameObject.transform.position;
			Debug.Log ("Run away!");
		}
		if (CurTask == Task.Working) {
			Work ();
			//Debug.Log ("Starting to Work!");

		}


        try //omg ty
        {
            if (selected)
                halo.enabled = true;
            else
                halo.enabled = false;
        }
        catch { }
//		if (Input.GetMouseButtonDown(0))
//		{
//			MouseClick ();
//
//		}
//		if (Input.GetMouseButtonDown (1)) {
//
//
//		}
	}
}
