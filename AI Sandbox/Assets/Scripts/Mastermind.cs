using UnityEngine;
using System.Collections.Generic;

public class KnownThing : MonoBehaviour
{
    public GameObject thing;
    public int GoodOrBad = 0; //false is bad

    public KnownThing(GameObject Thing, int GB)
    {
        thing = Thing;
        GoodOrBad = GB;
    }
	public GameObject GetGame(){
		return thing;
	}
}
public class Mastermind : MonoBehaviour {


	//private List<GameObject> Hunters;

	private List<KnownThing> Things;
	private List<int> SoundEnds;
	private float SafeSoundLevel = 0;
    KnownThing Blank;
	bool started = false;
	// Use this for initialization
	void Start () {


        //Hunters = new List<GameObject> ();
        Things = new List<KnownThing>();
		Blank = new KnownThing(this.gameObject, 2);
		Blank.thing = this.gameObject;
		Blank.GoodOrBad = 2;
		Things.Add(Blank);
        SoundEnds = new List<int> ();
		started = true;
	}
    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        //foreach (KnownThing poss in Things)
        //{
        //    if(poss.thing == null) // hopefully remove all null references, but perhaps we don't want to for permanent running knowledge
        //    {
        //        try { Things.Remove(poss); }
        //        catch { }
        //    }
        //}
     }

	/// <summary>
	/// Removes all blank or null objects within Things
	/// </summary>
	void PurgeElements(){
		foreach (KnownThing poss in Things)
		{
			try
			{
				if (poss.GetGame() == null)
				{
					Things.Remove(poss);
				}
			}
			catch { }

		}
	}

	float AvgSoundEnd(){
		float avg = 0;
		foreach (int x in SoundEnds) {
			avg += x;
		}
		return avg / SoundEnds.Count;
	}
    public GameObject GetUnknown()
    {
		if (!started) {
			Start ();
		}
        GameObject SafestTarget = null;

        foreach (KnownThing poss in Things)
        {
            try
            {
                if (poss.GoodOrBad == 0)
                {
					Describe (poss.thing);
                    return poss.thing;
                }
            }
            catch { }

        }
		PurgeElements();

        return SafestTarget;

    }


	public void Describe(GameObject SafeTarg){
        Object[] all = FindObjectsOfType(typeof(MonoBehaviour)); //returns Object[]
        GameObject UnknownTarget = null;// = new GameObject();
        GameObject SafestTarget = null;
        float SafetyPercentage = 0, NonSafety = 0;
        int totalgood = 0, totalbad = 0, total = all.Length+1;
        //Bayes lol
        foreach (KnownThing poss in Things)
        {
          
            if (poss.GoodOrBad == 1)
            {
                totalbad++;
            }
            else
            {
                totalgood++;
            }


        }

		UnknownTarget = SafeTarg;
        //return null;
		SafetyPercentage = ((UnknownTarget.gameObject.GetComponent<GBScript>().GetGood()/totalgood)*(totalgood/total));
		NonSafety = ((UnknownTarget.gameObject.GetComponent<GBScript>().GetBad() / totalbad) * (totalbad / total));
        //assume it's whatever the higher one is
        if(SafetyPercentage > NonSafety)
        {// it's good
            UnknownTarget.gameObject.GetComponent<GBScript>().goodorbad = 2;
        }
        else
        {
            UnknownTarget.gameObject.GetComponent<GBScript>().goodorbad = 1;
        }
        
       // return UnknownTarget;
	}
	public void AddSoundEnd(GameObject obj, int good){ //true is good, false is bad

        try
        {
            foreach (KnownThing poss in Things)
            {
                if(obj == poss.thing)
                {
                    poss.GoodOrBad = good;
                    return;
                }

            }
        }
        catch { }

		//SoundEnds.Add (end);
	}

	public void AddThing(GameObject ThingFound){
        //ownThing bleh = new KnownThing(ThingFound);
        Things.Add(new KnownThing(ThingFound, ThingFound.GetComponent<GBScript>().goodorbad));
        //Things.FindIndex(ThingFound)
		//try{
		//	Things.Remove(sound); //just in case it gets double called, it can remove.
		//}catch{
		//}
		//Things.Add (sound);

	}

    public void AddDrone(GameObject hunter)
    {
        //	try{
        //		Hunters.Remove(hunter); //just in case it gets double called, it can remove.
        //	}catch{
        //	}
        //	Hunters.Add (hunter);

    }
}
