using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour {

	public int NumRock = 100;
	//public string name = "Quarry";



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public bool GainRock(){

		if (NumRock - 1 >= 0) {
			NumRock -= 1;
			return true;
		} else {
			return false;
		}



	}
}
