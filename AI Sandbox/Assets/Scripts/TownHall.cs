using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class TownHall : MonoBehaviour {

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
			Debug.Log ("Wewd: " + Wood);
		}

	}
	void Start () {



	}
    void EndGame()
    {
        SceneManager.LoadScene(1);



    }
	
	// Update is called once per frame
	void Update () {
	    if(Wood >= 200 && Stone >= 200)
        {

            EndGame();

        }
	}
}
