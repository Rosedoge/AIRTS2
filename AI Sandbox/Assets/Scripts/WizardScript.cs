using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WizardScript : MonoBehaviour {
	private List<GameObject> ThingsAround;
	public List<Vector3> pathTargets;
	private int currentTarget;
	public enum Task {chasing, pathing};
	Task curTask = Task.pathing;
	private NavMeshAgent navigationAgent;
	public GameObject[] EnemySwords;
	private GameObject Target;

	public int Health = 5;

	public bool Dead = false;
	private float maybeWaitTimer;
	private bool maybeWaiting;


	// Use this for initialization
	void Start () {

	}



	void Awake()
	{
		ThingsAround = new List<GameObject> ();
		EnemySwords = GameObject.FindGameObjectsWithTag ("PlayerWarriorSword");
		navigationAgent = this.gameObject.GetComponent<NavMeshAgent> ();
		foreach (GameObject sword in EnemySwords) {
			Physics.IgnoreCollision (this.gameObject.GetComponent<SphereCollider> (), sword.gameObject.GetComponent<BoxCollider> ());
		}

	}

	void FixedUpdate(){
		try{
			ThingsAround.RemoveAll (null);
			Debug.Log("Null object removed from around me");
		}
		catch{
		}

	}

	void OnTriggerEnter(Collider col){
		ThingsAround.Add (col.gameObject);

	}

	void OnTriggerExit(Collider col){
		try{
			ThingsAround.Remove(col.gameObject);
		}
		catch{
		}

	}

	
	// Update is called once per frame
	void Update () {


	
	}



	void DecisionBase(){

		// See if there are any targets around
		if(CheckSafety()){ //More Enemies than allies
			//Find the closest one
			Target = FindClosest(); //Target is now the cloest Object that's a Worker

		}
	}

	GameObject FindClosest(){
		float MinDist = 1000;
		GameObject MinGameobject = new GameObject();
		foreach (GameObject obj in ThingsAround) {
			if (obj.tag == "Villager") {
				if (Vector3.Distance (this.gameObject.transform.position, obj.transform.position) < MinDist) {
					MinDist = Vector3.Distance (this.gameObject.transform.position, obj.transform.position) - MinDist;
					MinGameobject = obj;
				}
			}
		}//end of each
		if (MinGameobject != null) {
			return MinGameobject;
		} else {
			return null;
		}

	}

	bool CheckSafety(){
		int allies = 0, enemies = 0;// soldier = 0;
		for (int x = 0; x < ThingsAround.Count; x++) {
			if (ThingsAround [x].gameObject.tag == "Enemy")
				allies++;
			if (ThingsAround [x].gameObject.tag == "Villager")
				enemies++;
			if (ThingsAround [x].gameObject.tag == "Warrior")
				enemies++;
		}
		if (enemies >= allies) {
			return true;
		} else {
			return false;

		}
//		if (allies > soldier) {
//			Debug.Log ("They're weak, gett'em");
//			return true;
//		} else if (allies <= soldier) {
//			Debug.Log ("Too many punks for my taste");
//			return false;
//		} else {
//			return false;
//		}
	}

}
