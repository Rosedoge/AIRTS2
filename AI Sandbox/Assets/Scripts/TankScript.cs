using UnityEngine;
using System.Collections;

public class TankScript : MonoBehaviour {

	Behaviour halo;// = (Behaviour)GetComponent("Halo");
	public bool selected = false;
	public enum Task {None,Fighting,Guard,Moving};
	Task CurTask;

	public GameObject LGun, RGun;

	//Attack Related
	GameObject TargetObject;


	// Use this for initialization
	void Start () {
		//gameObject.GetComponent<Halo
		//Home = GameObject.FindGameObjectWithTag("TownHall");
		halo = (Behaviour)GetComponent("Halo");
		halo.enabled = false;
		CurTask = Task.None;
	}

	public void Move(RaycastHit hit){
		this.gameObject.GetComponent<Animator> ().SetBool ("Walking", true);
		this.gameObject.GetComponent<NavMeshAgent> ().destination = hit.point;
		//Debug.Log ("Stop Working");
		CurTask = Task.Moving;

	}


	// Update is called once per frame
	void Update () {
	

		if (selected)
			halo.enabled = true;
		else
			halo.enabled = false;

		if (this.gameObject.GetComponent<NavMeshAgent> ().destination == this.gameObject.transform.position) {
			this.gameObject.GetComponent<Animator> ().SetBool ("Walking", false);

			CurTask = Task.None;
		}
	}
}
