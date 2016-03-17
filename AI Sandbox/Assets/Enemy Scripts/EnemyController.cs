using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
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

    void Awake()
    {
		ThingsAround = new List<GameObject> ();
		EnemySwords = GameObject.FindGameObjectsWithTag ("PlayerWarriorSword");
        navigationAgent = this.gameObject.GetComponent<NavMeshAgent>();
		foreach(GameObject sword in EnemySwords){
			Physics.IgnoreCollision(this.gameObject.GetComponent<SphereCollider>(), sword.gameObject.GetComponent<BoxCollider>());
		}


    }

    // Use this for initialization
    void Start()
    {
        SetNearestTarget();
        maybeWaitTimer = Random.Range(-1.0f, 1.0f);
        maybeWaiting = false;
    }

	void FixedUpdate(){
		ThingsAround.RemoveAll (null);

	}

    // Update is called once per frame
    void Update()
    {
        DoBehaviorTree();

		if (Health <= 0) {
			Dead = true;
			Destroy (this.gameObject);

		}
    }



    private void SetNearestTarget()
    {
        if (pathTargets.Count > 0)
        {
            currentTarget = 0;
            float distToCurrent = Vector3.Distance(transform.position, pathTargets[0]);
            for (int i = 0; i < pathTargets.Count; i++)
            {
                if (Vector3.Distance(transform.position, pathTargets[i]) < distToCurrent)
                {
                    currentTarget = i;
                    distToCurrent = Vector3.Distance(transform.position, pathTargets[i]);
                }
            }
        }
        else
        {
            Debug.Log("No path targets found in pathTargets list");
        }
    }

	void OnTriggerEnter(Collider col){
		ThingsAround.Add (col.gameObject);
		if (col.gameObject.tag == "Villager") { // finds a villager
			if (CheckSafety ()) { //more allies than soldiers around
				navigationAgent.destination = col.gameObject.transform.position;
				col.gameObject.GetComponent<WorkerScript> ().Feared = true;
				curTask = Task.chasing;
				Target = col.gameObject;
			} else { //unsafe, keep moving on


			}
		}

//			Health -= 1;
//			Debug.Log ("My health is lower! " + Health);
//
//

	}

	public void Attack(){
		Health -= 1;
		Debug.Log ("My health is lower! " + Health);

	}

	bool CheckSafety(){
		int allies = 0, enemies = 0, soldier = 0;
		for (int x = 0; x < ThingsAround.Count; x++) {
			if (ThingsAround [x].gameObject.tag == "Enemy")
				allies++;
			if (ThingsAround [x].gameObject.tag == "Villager")
				enemies++;
			if (ThingsAround [x].gameObject.tag == "Warrior")
				soldier++;
		}
		if (allies > soldier) {
			Debug.Log ("They're weak, gett'em");
			return true;
		} else if (allies <= soldier) {
			Debug.Log ("Too many punks for my taste");
			return false;
		} else {
			return false;
		}
	}

	void OnTriggerExit(Collider col){
		ThingsAround.Remove(col.gameObject);
		if (col.gameObject.tag == "Villager") {
			//Will set the villager to a 'run' status where it will do it's best to move to safety, in this case, the Town Hall
			col.gameObject.GetComponent<WorkerScript>().CurTask = WorkerScript.Task.None;
			col.gameObject.GetComponent<WorkerScript> ().Feared = false;
			curTask = Task.pathing;
			Target = null;
		}


	}


    private void DoBehaviorTree()
    {
        //This will consist of a structure of if, else if, and else statements
        //Each statement will be driven by a helper method which will perform a
        //check. If the check succeeds, a function will carry out the action.

        if(CheckIsntChasing())
        {
            if(CheckIfNearTarget())
            {
                if (MaybeWait())
                {
                    FollowPath();
                }
            }
        }
    }


    #region BehaviorTreeFunctions

    private bool CheckIsntChasing()
    {
        if(curTask != Task.chasing)
        {
            return true;
        }
        navigationAgent.destination = Target.gameObject.transform.position;
        return false;
    }

    private bool CheckIfNearTarget()
    {
        if (Vector3.Distance(transform.position, pathTargets[currentTarget]) < 1f)
        {
            currentTarget = (currentTarget + 1) % pathTargets.Count;
            navigationAgent.destination = pathTargets[currentTarget];
            return false;
        }
        return true;
    }

    private bool MaybeWait()
    {
        maybeWaitTimer -= Time.deltaTime;

        Debug.Log(maybeWaiting);

        if(maybeWaitTimer <= 0)
        {
            maybeWaitTimer = Random.Range(5.0f, 10.0f);
            if(Random.Range(0,2) == 0)
            {
                maybeWaiting = true;
                navigationAgent.enabled = true;
            }
            else
            {
                maybeWaiting = false;
                navigationAgent.enabled = false;
            }
        }

        return maybeWaiting;
    }

    private bool FollowPath() //Action - always returns true
    {
        Debug.DrawLine(transform.position, pathTargets[currentTarget], Color.black);
        if (Vector3.Distance(transform.position, pathTargets[currentTarget]) < 1f)
        {
            currentTarget = (currentTarget + 1) % pathTargets.Count;
            navigationAgent.destination = pathTargets[currentTarget];
            Debug.Log(currentTarget);
        }

        if (navigationAgent.destination != pathTargets[currentTarget])
        {
            navigationAgent.destination = pathTargets[currentTarget];
        }
        return true;
    }

    #endregion

}
