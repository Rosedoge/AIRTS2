using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneticHandler : MonoBehaviour
{
    private Swarmer[] population;


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
        //Format for Array
        //Rows - Each row = one chromosome
        //Columns - 0: SeekDist, 1: FleeDist, 2: WanderDist, 3: Grab Amount(capacity), 4: Speed, 5: Lifetime, 6: Amount Grabbed(total), 7: Fitness
        int[][] chromosomes = new int[population[0].ChromosomeLength + 1][];
        for (int i = 0; i < chromosomes.Length; i++)
        {
            chromosomes[i] = new int[population.Length];
        }

        
        for (int y = 0; y < population.Length; y++)
        {
            chromosomes[y] = population[y].GetChromosome();
            chromosomes[y][7] = DetermineFitness(chromosomes[y][5], chromosomes[y][6]);
        }

        CrossBreed(population.Length, chromosomes);
    }

    private int DetermineFitness(int timeAlive, int totalCollected)
    {
        return timeAlive / totalCollected;
    }

    private void CrossBreed(int numChromosomes, int[][] chromosomes)
    {
        //Get list of all chromosomes
        List<int[]> chromos = new List<int[]>();
        for (int i = 0; i < numChromosomes; i++)
        {
            chromos.Add(chromosomes[i]);
        }

        //Set up variables
        List<int[]> newPop = new List<int[]>();

        List<int[]> tourney1 = new List<int[]>();
        List<int[]> tourney2 = new List<int[]>();


        //While there are still some in the original population
        while (chromos.Count > 0)
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
                chromos.Remove(chromos[randInt]);
            }

            for (int i = 0; i < size2; i++)
            {
                int randInt = Random.Range(0, chromos.Count);
                tourney2.Add(chromos[randInt]);
                chromos.Remove(chromos[randInt]);
            }


            //Determine winner of each tournament
            for (int i = 0; i < tourney1.Count; i++)
            {
                int[] competitor = tourney1[i];
                if (competitor[7] > winner1[7])
                {
                    if(winner1[7] > -1)
                    {
                        chromos.Add(winner1);
                    }
                    winner1 = competitor;
                }
            }

            for (int i = 0; i < tourney2.Count; i++)
            {
                int[] competitor = tourney2[i];
                if (competitor[7] > winner2[7])
                {
                    if (winner2[7] > -1)
                    {
                        chromos.Add(winner2);
                    }
                    winner2 = competitor;
                }
            }


            //Do a random crossover for real chromosome variables
            int randomCrossPoint = Random.Range(0, 5);

            for (int i = 0; i < winner1.Length; i++)
            {
                if(i >= randomCrossPoint)
                {
                    int holder = winner1[i];
                    winner1[i] = winner2[i];
                    winner2[i] = holder;
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
        for(int i = 0; i < newPop.Count; i++)
        {
            population[i].MakeNewSwarmer(newPop[i]);
        }
    }
}
