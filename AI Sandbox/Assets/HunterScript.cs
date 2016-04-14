using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HunterScript : MonoBehaviour {

	public GameObject Mastermind;

	private List<GameObject> ThingsAround;
	private List<GameObject> Sounds;
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

	void Awake()
	{
		ThingsAround = new List<GameObject> ();
		EnemySwords = GameObject.FindGameObjectsWithTag ("PlayerWarriorSword"); //Knows that there are swords for the sake of being attacked
		navigationAgent = this.gameObject.GetComponent<NavMeshAgent>();
		foreach(GameObject sword in EnemySwords){
			Physics.IgnoreCollision(this.gameObject.GetComponent<SphereCollider>(), sword.gameObject.GetComponent<BoxCollider>());
		}
		//Mastermind.GetComponent<Mastermind> ().AddDrone (this.gameObject);
		//Target = Mastermind.GetComponent<Mastermind> ().MstrGetTarget ();
	}



	void OnTriggerEnter(Collider col){
		ThingsAround.Add (col.gameObject);
		if (col.gameObject.GetComponent<WorkerScript> () || col.gameObject.GetComponent<SoldierScript> () || col.gameObject.GetComponent<EnemyController> ()) {
			try{
				Mastermind.GetComponent<Mastermind> ().AddSound (col.gameObject);
			}catch{
			}
			try{
				Mastermind.GetComponent<Mastermind> ().AddSound (col.gameObject);
			}catch{
			}
			try{
				Mastermind.GetComponent<Mastermind> ().AddSound (col.gameObject);
			}catch{
			}
		}
	}

	public void Death(){
		Mastermind.GetComponent<Mastermind> ().AddSoundEnd (1);


	}

	// Use this for initialization
	void Start () {
		Mastermind.GetComponent<Mastermind> ().AddDrone (this.gameObject);
		Target = GetTarget ();
	}

	GameObject GetTarget(){
		return Mastermind.GetComponent<Mastermind> ().MstrGetTarget ();


	}
	
	// Update is called once per frame
	void Update () {
		if (Target == null) {
			Target = GetTarget ();
			Debug.Log ("Target name is: " + Target.gameObject.name);
		} else {
			if (this.gameObject.GetComponent<NavMeshAgent> ().destination != Target.transform.position) {
				this.gameObject.GetComponent<NavMeshAgent> ().destination = Target.transform.position;


			}

		}
	
	}
}
