using UnityEngine;
using System.Collections;

public class Swarmer : MonoBehaviour
{
    public int seekDist, fleeDist, wanderStrength, grabAmount, speed;

    private float lifeTime;
    private int amountGrabbed;
    private bool alive;
    public GameObject target;
    public GameObject homeBase;

    private bool headedHome;

    public int inHand;
    private float grabTimer;

    public GameObject[] fleeTargets;

    public bool IsAlive
    {
        get { return alive; }
    }

    public int ChromosomeLength
    {
        get { return 7; }
    }

    // Use this for initialization
    void Start()
    {
        alive = false;

        //Columns - 0: SeekDist, 1: FleeDist, 2: WanderDist, 3: Grab Amount(capacity), 4: Speed, 5: Lifetime, 6: Amount Grabbed(total), 7: Fitness
        //             0 - 100      0-50          0-5            0-5                       0-5

        seekDist = Random.Range(0, 100);
        fleeDist = Random.Range(0, 50);
        wanderStrength = Random.Range(0, 5);
        grabAmount = Random.Range(0, 5);
        speed = Random.Range(0, 5);

        lifeTime = 0.0f;
        amountGrabbed = 0;
        grabTimer = 1;
        inHand = 0;

        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);

            MaybeSeek();
            MaybeFlee();
            Wander();
            if (!headedHome)
            {
                GrabResources();
            }
            else
            {
                DropOff();
            }


            lifeTime += Time.deltaTime;
            if (lifeTime > 60.0f)
            {
                alive = false;
            }
        }
    }


    //Reinitialize this swarmer with a new chromosome (effectively making a "new" one)
    public void MakeNewSwarmer(int[] chromosome)
    {
        alive = false;

        seekDist = chromosome[0];
        fleeDist = chromosome[1];
        wanderStrength = chromosome[2];
        grabAmount = chromosome[3];
        speed = chromosome[4];

        lifeTime = 0.0f;
        amountGrabbed = 0;
        grabTimer = 1;
        inHand = 0;

        alive = true;
    }

    public int[] GetChromosome()
    {
        //Seek, Flee, Wander, Grab Amount, Speed, Lifetime(postmortem), amt grabbed
        int[] chromosome = new int[7];

        chromosome[0] = seekDist;
        chromosome[1] = fleeDist;
        chromosome[2] = wanderStrength;
        chromosome[3] = grabAmount;
        chromosome[4] = speed;
        chromosome[5] = (int)(lifeTime * 100.0f);
        chromosome[6] = amountGrabbed;
        chromosome[7] = 0; //Fitness placeholder

        return chromosome;
    }

    private void MaybeSeek()
    {
        if(!headedHome)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= seekDist)
            {
                this.gameObject.GetComponent<Rigidbody>().velocity += target.transform.position - this.transform.position;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, homeBase.transform.position) <= seekDist)
            {
                this.gameObject.GetComponent<Rigidbody>().velocity += homeBase.transform.position - this.transform.position;
            }
        }
    }

    private void MaybeFlee()
    {
        GameObject nearest = null;
        foreach (GameObject enemy in fleeTargets)
        {
            if(nearest == null)
            {
            nearest = enemy;
            }
            if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) < Vector3.Distance(this.gameObject.transform.position, nearest.transform.position))
            {
                nearest = enemy;
            }
        }

        if(Vector3.Distance(transform.position, nearest.transform.position) <= fleeDist)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity += this.transform.position - nearest.transform.position;
        }

    }

    private void Wander()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity += gameObject.transform.position + (gameObject.GetComponent<Rigidbody>().velocity.normalized * wanderStrength);
    }
   

    private void GrabResources()
    {
        if(Vector3.Distance(this.gameObject.transform.position, target.transform.position) < 12.0f)
        {
            if (inHand < grabAmount)
            {
                if(grabTimer <= 0)
                {
                    inHand++;
                    grabTimer = 1;
                }
                grabTimer -= Time.deltaTime;
            }
            else
            {
                headedHome = true;
                grabTimer = 1;
            }
        }
    }

    private void DropOff()
    {
        if (Vector3.Distance(this.gameObject.transform.position, homeBase.transform.position) < 5.0f)
        {
            if (inHand < grabAmount)
            {
                if (grabTimer <= 0)
                {
                    inHand--;
                    grabTimer = 1;
                }
                grabTimer -= Time.deltaTime;
            }
            else
            {
                headedHome = false;
                grabTimer = 1;
            }
        }
    }
}
