using UnityEngine;
using System.Collections;

public class TownHall : BuildingScript {

	public bool selected = false;
	public int Health = 1500;
	public int Stone=0, Food=0, Wood=0;


	// Use this for initialization

	public void TakeResources(string type, int amt){
		if (type == "Stone") {
			Stone += amt;
			Debug.Log ("Stone: " + Stone);
		} else if (type == "Food") {

			Food += amt;
		} else if (type == "Wood") {
			Wood += amt;
		}

	}
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
