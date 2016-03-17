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
		Debug.Log ("Stop Working");
		target = tar;//.gameObject.transform.position;
		this.gameObject.GetComponent<Animation> ().Play ("Walk");


	}


	public void Move(RaycastHit hit){
		this.gameObject.GetComponent<NavMeshAgent> ().destination = hit.point;
		Debug.Log ("Stop Working");
		target = hit.collider.gameObject;
		this.gameObject.GetComponent<Animation> ().Play ("Walk");
		CurTask = Task.Guard;
	}

	// Update is called once per frame
	void Update () {
		
		if(CurTask == Task.Guard){
            try
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, target.gameObject.transform.position);
                if (distance < 0.5f)
                {
                    this.gameObject.GetComponent<Animation>().Play("idle");
                }
            }
            catch { }

		}else if(CurTask == Task.Fighting){
			float distance = Vector3.Distance (this.gameObject.transform.position, target.gameObject.transform.position);
			if (distance < 2.5f) {
				this.gameObject.GetComponent<Animation> ().Play ("Attack");
				this.gameObject.GetComponent<NavMeshAgent> ().destination = this.gameObject.transform.position;
			} else {
				this.gameObject.GetComponent<NavMeshAgent> ().destination = target.gameObject.transform.position;

			}
			Debug.Log ("Task is: " + CurTask + "   And my Distance to the enemy is: " + distance);

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
