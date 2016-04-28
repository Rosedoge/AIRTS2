using UnityEngine;
using System.Collections;

public class Swarmer : MonoBehaviour
{
    public int seekDist, fleeDist, wanderDist, grabAmount, speed;

    private float lifeTime;
    private int amountGrabbed;
    private bool alive;
    public GameObject target;

    public GameObject[] fleeTargets;


    // Use this for initialization
    void Start()
    {
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > 60.0f)
            {
                alive = false;
            }
        }
    }


    public void Generate(int[] chromosome)
    {
        alive = false;

        seekDist = chromosome[0];
        fleeDist = chromosome[1];
        wanderDist = chromosome[2];
        grabAmount = chromosome[3];
        speed = chromosome[4];

        lifeTime = 0.0f;
        amountGrabbed = 0;

        alive = true;
    }

    public int[] GetChromosome()
    {
        //Seek, Flee, Wander, Grab Amount, Speed, Lifetime(postmortem), amt grabbed
        int[] chromosome = new int[7];

        chromosome[0] = seekDist;
        chromosome[1] = fleeDist;
        chromosome[2] = wanderDist;
        chromosome[3] = grabAmount;
        chromosome[4] = speed;
        chromosome[5] = (int)(lifeTime * 100.0f);
        chromosome[6] = amountGrabbed;

        return chromosome;
    }

    private void Seek()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity += target.transform.position - this.transform.position;
    }

    private void Flee()
    {
        foreach (GameObject enemy in fleeTargets)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity -= enemy.transform.position - this.transform.position;
        }
    }
}
