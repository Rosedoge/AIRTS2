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

    void Awake()
    {
        navigationAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        SetNearestTarget();
    }

    // Update is called once per frame
    void Update()
    {
		if (curTask == Task.pathing) {
			FollowPath ();
		}
		if (curTask == Task.chasing) {
			navigationAgent.destination = Target.gameObject.transform.position;
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

    private void FollowPath()
    {
        Debug.DrawLine(transform.position, pathTargets[currentTarget], Color.black);
        if(Vector3.Distance(transform.position, pathTargets[currentTarget]) < 1f)
        {
            currentTarget = (currentTarget + 1) % pathTargets.Count;
            navigationAgent.destination = pathTargets[currentTarget];
            Debug.Log(currentTarget);
        }

        if(navigationAgent.destination != pathTargets[currentTarget])
        {
            navigationAgent.destination = pathTargets[currentTarget];
        }
    }
}
