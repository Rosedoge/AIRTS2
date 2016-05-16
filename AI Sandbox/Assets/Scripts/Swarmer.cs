using UnityEngine;
using System.Collections;

public class Swarmer : MonoBehaviour
{
    public int seekDist, fleeDist, wanderStrength, grabAmount, speed;

    public float lifeTime;
    private int amountGrabbed;
    private bool alive;
    public GameObject target;
    public GameObject homeBase;

    private Vector3 velocity;

    private bool headedHome;

    public int inHand;
    private float grabTimer;
    private float wanderTimer;
    private Vector3 wanderDirection;

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
        //             0 - 100      0-50          0-30            0-5                       0-5

        seekDist = Random.Range(0, 101);
        fleeDist = Random.Range(0, 51);
        wanderStrength = Random.Range(0, 31);
        grabAmount = Random.Range(0, 6);
        speed = Random.Range(0, 6);

        lifeTime = 0.0f;
        amountGrabbed = 0;
        grabTimer = 1.0f;
        inHand = 0;
        wanderDirection = new Vector3(0, 0, 0);
        wanderTimer = 0;


        transform.position = homeBase.transform.position;

        fleeTargets = GameObject.FindGameObjectsWithTag("Warrior");

        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        alive = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            velocity = new Vector3(0, -1.0f, 0);

            MaybeSeek();
            MaybeFlee();
            if (!headedHome)
            {
                if (Vector3.Distance(this.gameObject.transform.position, target.transform.position) > 15.0f)
                {
                    Wander();
                }
                else { GrabResources(); }
            }
            else
            {
                if (Vector3.Distance(this.gameObject.transform.position, homeBase.transform.position) > 4.0f)
                {
                    Wander();
                }
                else { DropOff(); }
            }


            velocity.y = this.gameObject.GetComponent<Rigidbody>().velocity.y;
            velocity = velocity.normalized * speed;
            this.gameObject.GetComponent<Rigidbody>().velocity = velocity;

            if (transform.position.y != homeBase.transform.position.y)
            {
                Vector3 pos = transform.position;
                pos.y = homeBase.transform.position.y;
                transform.position = pos;
            }

            lifeTime += Time.deltaTime;
            if (lifeTime > homeBase.GetComponent<GeneticHandler>().LifeTime)
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
        grabTimer = 1.0f;
        inHand = 0;
        wanderDirection = new Vector3(0, 0, 0);
        wanderTimer = 0;

        transform.position = homeBase.transform.position;

        this.gameObject.GetComponent<MeshRenderer>().enabled = true;

        alive = true;
    }

    public int[] GetChromosome()
    {
        //Seek, Flee, Wander, Grab Amount, Speed, Lifetime(postmortem), amt grabbed
        int[] chromosome = new int[8];

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
        if (!headedHome)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= seekDist)
            {
                velocity += target.transform.position - this.transform.position;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, homeBase.transform.position) <= seekDist)
            {
                velocity += homeBase.transform.position - this.transform.position;
            }
        }
    }

    private void MaybeFlee()
    {
        GameObject nearest = null;
        foreach (GameObject enemy in fleeTargets)
        {
            if (nearest == null)
            {
                nearest = enemy;
            }
            if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) < Vector3.Distance(this.gameObject.transform.position, nearest.transform.position))
            {
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            if (Vector3.Distance(transform.position, nearest.transform.position) <= fleeDist)
            {
                velocity += (this.transform.position - nearest.transform.position)*100.0f;
            }

            if(Vector3.Distance(transform.position, nearest.transform.position) <= 3.0f)
            {
                alive = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }

    private void Wander()
    {

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            Vector2 rand2 = Random.insideUnitCircle;
            wanderDirection = new Vector3(rand2.x, 0, rand2.y);
            wanderTimer = Random.Range(.5f, 3.0f);
        }

        velocity += wanderDirection * (wanderStrength * .2f);
    }


    private void GrabResources()
    {
        if (inHand < grabAmount)
        {
            if (grabTimer <= 0)
            {
                inHand++;
                grabTimer = 1.0f;
                amountGrabbed++;
                if (Random.Range(0,2) == 0)
                {
                    target.GetComponent<TownHall>().Stone--;
                }
                else
                {
                    target.GetComponent<TownHall>().Wood--;
                }
            }
            grabTimer -= Time.deltaTime;
        }
        else
        {
            headedHome = true;
            grabTimer = 1;
        }
    }

    private void DropOff()
    {
        if (inHand > 0)
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
