using UnityEngine;
using System.Collections;

public class SoldierScript : MonoBehaviour {

	Behaviour halo;// = (Behaviour)GetComponent("Halo");
	public bool selected = false;
	public enum Task {Fighting,Guard};
	public Task CurTask;

	GameObject target;
	//Work Related
	GameObject TargetObject;
	GameObject Home;
	float Lasttime;

	// Use this for initialization
	void Start () {
		//gameObject.GetComponent<Halo
		Home = GameObject.FindGameObjectWithTag("TownHall");
		try
		{
			halo = (Behaviour)GetComponent("Halo");
			halo.enabled = false;
		}
		catch { }
		CurTask = Task.Guard;
	}

	public void Attack(GameObject tar){
		this.gameObject.GetComponent<NavMeshAgent> ().destination = tar.gameObject.transform.position;
		//Debug.Log ("Stop Working");
		target = tar;//.gameObject.transform.position;
		this.gameObject.GetComponent<Animation> ().Play ("Walk");
		selected = false;

	}


	public void Move(RaycastHit hit){
		this.gameObject.GetComponent<NavMeshAgent> ().destination = hit.point;
		//Debug.Log ("Stop Working");
		target = hit.collider.gameObject;
		//target.transform.position = hit.point;
		this.gameObject.GetComponent<Animation> ().Play ("Walk");
		CurTask = Task.Guard;
	}

	void FixedUpdate()
	{
		if ( gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0.01 )
		{
			//Debug.Log("unity answers saves the day!");
		}
	}

	// Update is called once per frame
	void Update () {
		
		if(CurTask == Task.Guard){
			if (target != null) {
				float distance = Vector3.Distance (this.gameObject.transform.position, target.gameObject.transform.position);
				if (distance < 0.5f) {
					this.gameObject.GetComponent<Animation> ().Play ("idle");
				}
			}
		}else if(CurTask == Task.Fighting){
			if (target.gameObject != null && target.gameObject.GetComponent<EnemyController>().Dead == false) {
				float distance = Vector3.Distance (this.gameObject.transform.position, target.gameObject.transform.position);
				if (distance < 2.5f) {
					gameObject.transform.LookAt(target.gameObject.transform.position);
					this.gameObject.GetComponent<Animation> ().Play ("Attack");
					//this.gameObject.GetComponent<NavMeshAgent> ().destination = this.gameObject.transform.position;
				} else {
					this.gameObject.GetComponent<NavMeshAgent> ().destination = target.gameObject.transform.position;

				}
				Debug.Log ("Task is: " + CurTask + "   And my Distance to the enemy is: " + distance);
			} else {
				CurTask = Task.Guard;
				target = this.gameObject;

			}

		}
		try //omg ty
		{
			if (selected)
				halo.enabled = true;
			else
				halo.enabled = false;
		}
		catch { }

	}
}
