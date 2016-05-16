using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GBScript : MonoBehaviour {

    public int goodorbad = 0;
    private List<GameObject> ThingsAround;
    public int GoodAround = 0, BadAround = 0;
    // Use this for initialization
    void Start () {
        ThingsAround = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {


    }

	public int GetGood(){
		return GoodAround;
	}

	public int GetBad(){
		return BadAround;
	}
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Villager")
        {
            GoodAround += 1;

        }
        else if (col.gameObject.tag == "Warrior")
        {
            GoodAround += 1;
        }
        else if (col.gameObject.tag == "Enemy")
        {

            BadAround += 1;

        }
    }

    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Villager")
        {
            GoodAround -= 1;

        }
        if (col.gameObject.tag == "Warrior")
        {
            GoodAround -= 1;
        }
        if (col.gameObject.tag == "Enemy")
        {

            BadAround -= 1;
        }
    }
}
