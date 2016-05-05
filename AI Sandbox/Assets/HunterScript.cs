using UnityEngine;
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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<WorkerScript>() || col.gameObject.GetComponent<SoldierScript>())
        {
            Mastermind.GetComponent<Mastermind>().AddSoundEnd(col.gameObject, 2);
            Death();

        }
        else if (col.gameObject.GetComponent<EnemyController>())
        {
            Mastermind.GetComponent<Mastermind>().AddSoundEnd(col.gameObject, 1); // bad thing yo
            Death();
        }

    }

	void OnTriggerEnter(Collider col){
		ThingsAround.Add (col.gameObject);
		if (col.gameObject.GetComponent<WorkerScript> () || col.gameObject.GetComponent<SoldierScript> () || col.gameObject.GetComponent<EnemyController> ()) {
			try{
				Mastermind.GetComponent<Mastermind> ().AddThing (col.gameObject);
			}catch{
			}
			//try{
			//	Mastermind.GetComponent<Mastermind> ().AddThing (col.gameObject);
			//}catch{
			//}
			//try{
			//	Mastermind.GetComponent<Mastermind> ().AddThing (col.gameObject);
			//}catch{
			//}
		}
	}

	public void Death(){
        //Mastermind.GetComponent<Mastermind> ().AddSoundEnd (1);
        Destroy(this.gameObject);

	}

	// Use this for initialization
	void Start () {
		Mastermind.GetComponent<Mastermind> ().AddDrone (this.gameObject);
		Target = GetTarget ();
	}

	GameObject GetTarget(){

		return Mastermind.GetComponent<Mastermind> ().GetUnknown();


	}
	
	// Update is called once per frame
	void Update () {
		if (Target == null) {
			Target = GetTarget ();
            try
            {
                Debug.Log("Target name is: " + Target.gameObject.name);
            }
            catch { }
		} else {
			if (this.gameObject.GetComponent<NavMeshAgent> ().destination != Target.transform.position) {
				this.gameObject.GetComponent<NavMeshAgent> ().destination = Target.transform.position;
			}

		}
	
	}
}
