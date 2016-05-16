using UnityEngine;
using System.Collections;

public class ColorRandomizer : MonoBehaviour {

    Color[] colors;
    float onTime;
    float offTime;
 
    void Start()
    {
        colors = new Color[10];
       // colors.
       // DoFlicker();
        ChangeTime();
        InvokeRepeating("DoFlicker", 1, 1);
    }

   void DoFlicker()
    {
       
            //yield WaitForSeconds(offTime);
           // light.enabled = true;
            //yield WaitForSeconds(onTime);
            this.gameObject.GetComponent<Light>().color = colors[Random.Range(0, colors.Length)];
           // light.enabled = false;
            //yield WaitForEndOfFrame();
        
    }
    void ChangeTime()
    {
        colors[0] = Color.red;
        colors[1] = Color.black;
        colors[2] = Color.blue;
        colors[3] = Color.green;
        colors[4] = Color.cyan;
        colors[5] = Color.yellow;
        colors[6] = Color.magenta;
    }


}
