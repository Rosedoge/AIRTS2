using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mastermind : MonoBehaviour {


	private List<GameObject> Hunters;
	private List<GameObject> Sounds;
	private List<int> SoundEnds;
	private float SafeSoundLevel = 0;
	// Use this for initialization
	void Start () {


		Hunters = new List<GameObject> ();
		Sounds = new List<GameObject> ();
		SoundEnds = new List<int> ();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float AvgSoundEnd(){
		float avg = 0;
		foreach (int x in SoundEnds) {
			avg += x;
		}
		return avg / SoundEnds.Count;
	}


	public GameObject MstrGetTarget(){	
		float SafetyPercentage = 0;
		float TempSafe = 0;
		GameObject SafeTarget = new GameObject ();
		//Needs to find a sound that seems the best choice to follow.
		foreach (GameObject poss in Sounds) {
			try{
				TempSafe = (poss.GetComponent<WorkerScript> ().SoundVal / 9) * (AvgSoundEnd ()) / (5 / 10);
				//poss.GetComponent<WorkerScript> ().SoundVal
			}catch{
			}
			try{
				TempSafe = (poss.GetComponent<EnemyController> ().SoundVal / 9) * (AvgSoundEnd ()) / (4 / 10);
				//Mastermind.GetComponent<EnemyController> ().AddSound (col.gameObject);
			}catch{
			}
			try{
				TempSafe = (poss.GetComponent<SoldierScript> ().SoundVal / 9) * (AvgSoundEnd ()) / (5 / 10);
				//Mastermind.GetComponent<SoldierScript> ().AddSound (col.gameObject);
			}catch{
			}
			//SafetyPercentage = (4 / 9) * (AvgSoundEnd ()) / (1 / 10);
			if (TempSafe > SafetyPercentage) {
				SafetyPercentage = TempSafe;
				SafeTarget = poss;

			}
		}


		return SafeTarget;
	}
	public void AddSoundEnd(int end){
		SoundEnds.Add (end);
	}

	public void AddSound(GameObject sound){
		try{
			Sounds.Remove(sound); //just in case it gets double called, it can remove.
		}catch{
		}
		Sounds.Add (sound);

	}

	public void AddDrone(GameObject hunter){
		try{
			Hunters.Remove(hunter); //just in case it gets double called, it can remove.
		}catch{
		}
		Hunters.Add (hunter);

	}
}
