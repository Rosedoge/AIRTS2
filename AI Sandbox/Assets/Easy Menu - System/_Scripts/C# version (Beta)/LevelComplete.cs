using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour {
    public MenuWindow levelWindow;
    private int levelsNum;

	// Use this for initialization
	void Start () {
        if (!levelWindow) { levelWindow = this.GetComponent<MenuWindow>(); }

        if (levelWindow) {
            for (var i = 0; i < levelWindow.Elements.Length; i++) {
                if (PlayerPrefs.HasKey("UnlockLevel" + i.ToString())) { levelWindow.Elements[i].Locked(false); }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("p"))
            PlayerPrefs.DeleteAll();
	}

    //Use this function to unlock level next to current(completed)
    public void CompleteLevel (int index) 
    {
      if (levelWindow)
        if (levelWindow.Elements.Length>(index+1))
          levelWindow.Elements[index+1].Locked(false);
     
      PlayerPrefs.SetInt("UnlockLevel"+(index+1).ToString(), 1);
    }

    //----------------------------------------------------------------------------------
    //Use this function to level with specified index
    public void UnlockLevel (int index) 
    {
      if (levelWindow)
        if (levelWindow.Elements.Length>index)
          levelWindow.Elements[index].Locked(false);
      
      PlayerPrefs.SetInt("UnlockLevel"+index.ToString(), 1);
    }
}
