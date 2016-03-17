using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public List<Vector3> pathTargets;
    private int currentTarget;
	public enum Task {chasing, pathing};
	Task curTask = Task.pathing;
    private NavMeshAgent navigationAgent;

	private GameObject Target;

    private float maybeWaitTimer;
    private bool maybeWaiting;

    void Awake()
    {
        navigationAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        SetNearestTarget();
        maybeWaitTimer = Random.Range(-1.0f, 1.0f);
        maybeWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        DoBehaviorTree();
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
		if (col.gameObject.tag == "Villager") {
			//Will set the villager to a 'run' status where it will do it's best to move to safety, in this case, the Town Hall
			col.gameObject.GetComponent<WorkerScript>().CurTask = WorkerScript.Task.Scared;
			navigationAgent.destination = col.gameObject.transform.position;
			curTask = Task.chasing;
			Target = col.gameObject;
			Debug.Log ("Chasing a worker, haha");
		}


	}

	void OnTriggerExit(Collider col){
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
