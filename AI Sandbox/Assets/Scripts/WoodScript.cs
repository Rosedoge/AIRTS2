using UnityEngine;
using System.Collections;

public class WoodScript : MonoBehaviour {

	public int NumWood = 10000;
	//public string name = "Quarry";



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}



	public bool GainWood(){

		if (NumWood - 1 >= 0) {
			NumWood -= 1;
			return true;
		} else {
			return false;
		}



	}
}
