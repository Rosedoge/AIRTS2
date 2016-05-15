using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneticHandler : MonoBehaviour
{
    private Swarmer[] population;
    public float lifeTime;

    public float LifeTime
    {
        get { return lifeTime; }
    }

    // Use this for initialization
    void Start()
    {
        var pop = GameObject.FindGameObjectsWithTag("Swarmer");
        population = new Swarmer[pop.Length];
        for (int i = 0; i < pop.Length; i++)
        {
            population[i] = pop[i].GetComponent<Swarmer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool checkAllDead = true;
        for (int i = 0; i < population.Length; i++)
        {
            if (population[i].IsAlive)
            {
                checkAllDead = false;
                break;
            }
        }

        if (checkAllDead)
        {
            GenerateNewPopulation();
        }
    }

    private void GenerateNewPopulation()
    {
        //Columns - 0: SeekDist, 1: FleeDist, 2: WanderDist, 3: Grab Amount(capacity), 4: Speed, 5: Lifetime, 6: Amount Grabbed(total), 7: Fitness
        //             0 - 100      0-50          0-30            0-5                       0-5

        List<int[]> chromosomes = new List<int[]>();

        Debug.Log("-------!!!!!!!!!!!!!!!!!!!!!!!!!!!!-------NEW GENERATION-------!!!!!!!!!!!!!!!!!!!!!!!!!!!!-------");
        for (int i = 0; i < population.Length; i++)
        {
            int[] chromosome = population[i].GetChromosome();
            chromosome[7] = DetermineFitness(chromosome[5], chromosome[6]);
            chromosomes.Add(chromosome);
            ChromosomeToDebugPrint(chromosome);
        }

        CrossBreed(population.Length, chromosomes);
    }

    private int DetermineFitness(int timeAlive, int totalCollected)
    {
        if (timeAlive > 0)
        {
            int fitness = totalCollected * timeAlive;
            if (fitness == 0)
            {
                fitness = Random.Range(0, 400);
            }

            return fitness;
        }
        return 0;
    }

    private void CrossBreed(int numChromosomes, List<int[]> chromosomes)
    {
        //Get list of all chromosomes
        List<int[]> chromos = chromosomes;

        //Set up variables
        List<int[]> newPop = new List<int[]>();

        List<int[]> tourney1 = new List<int[]>();
        List<int[]> tourney2 = new List<int[]>();


        //While there are still some in the original population
        while (newPop.Count < chromos.Count)
        {
            //How big are the two tournaments (will always be even)
            int size1 = chromos.Count / 2;
            int size2 = chromos.Count - size1;


            //Placeholder winners
            int[] winner1 = new int[8] { 0, 0, 0, 0, 0, 0, 0, -10 };
            int[] winner2 = new int[8] { 0, 0, 0, 0, 0, 0, 0, -10 };


            //Add random members to each tournament
            for (int i = 0; i < size1; i++)
            {
                int randInt = Random.Range(0, chromos.Count);
                tourney1.Add(chromos[randInt]);
            }

            for (int i = 0; i < size2; i++)
            {
                int randInt = Random.Range(0, chromos.Count);
                tourney2.Add(chromos[randInt]);
            }


            //Determine winner of each tournament
            for (int i = 0; i < tourney1.Count; i++)
            {
                int[] competitor = tourney1[i];
                if (competitor[7] > winner1[7])
                {
                    winner1 = competitor;
                }
            }

            for (int i = 0; i < tourney2.Count; i++)
            {
                int[] competitor = tourney2[i];
                if (competitor[7] > winner2[7])
                {
                    winner2 = competitor;
                }
            }



            //Do a random crossover for real chromosome variables
            int randomCrossPoint = Random.Range(0, 5);

            for (int i = 0; i < winner1.Length; i++)
            {
                if (i >= randomCrossPoint)
                {
                    int holder = winner1[i];
                    winner1[i] = winner2[i];
                    winner2[i] = holder;
                }
            }

            if (Random.Range(0, 4) == 0)
            {
                if (Random.Range(0, 1) == 0)
                {
                    winner1 = Mutate(winner1);
                }
                else
                {
                    winner2 = Mutate(winner2);
                }
            }


            //Reset lifetime, gathered ammount, fitness
            winner1[5] = 0;
            winner1[6] = 0;
            winner1[7] = 0;

            winner2[5] = 0;
            winner2[6] = 0;
            winner2[7] = 0;


            //Add the two new chromosomes to the new population
            newPop.Add(winner1);
            newPop.Add(winner2);
        }


        //Generate new swarmers based off of this data
        for (int i = 0; i < newPop.Count; i++)
        {
            population[i].MakeNewSwarmer(newPop[i]);
        }
    }

    private int[] Mutate(int[] chromosome)
    {
        int index = Random.Range(0, 4);

        //Columns - 0: SeekDist, 1: FleeDist, 2: WanderDist, 3: Grab Amount(capacity), 4: Speed, 5: Lifetime, 6: Amount Grabbed(total), 7: Fitness
        //             0 - 100      0-50          0-30            0-5                       0-5

        switch (index)
        {
            case 0:
                chromosome[index] = Random.Range(0, 100);
                break;
            case 1:
                chromosome[index] = Random.Range(0, 50);
                break;
            case 2:
                chromosome[index] = Random.Range(0, 30);
                break;
            default:
                chromosome[index] = Random.Range(0, 5);
                break;

        }

        return chromosome;
    }

    private void ChromosomeToDebugPrint(int[] chr)
    {
        if (chr.Length == 8)
        {
            Debug.Log("{ Seek:" + chr[0] + ", Flee:" + chr[1] + ", Wander:" + chr[2] +
                ", Capacity:" + chr[3] + ", Speed:" + chr[4] + ", Lifetime:" + chr[5] +
                ", Grabbed:" + chr[6] + ", Fitness:" + chr[7] + " }");
        }
    }
}
