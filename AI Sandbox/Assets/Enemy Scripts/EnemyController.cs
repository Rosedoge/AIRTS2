using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public List<Vector3> pathTargets;
    private int currentTarget;

    private NavMeshAgent navigationAgent;


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
        FollowPath();
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
