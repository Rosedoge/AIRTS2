using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
    public MenuWindow[] windows;		// List of all windows
    public int activeWindow;			// Start/current window index
    public bool autoIndex = false; // All windows will be indexed automatically according to their  order in windows array
    public Vector2 defaultScreenSize = new Vector2 (800,480); // Default size of Screen. Size of all windows (and their elements)
                                                        // will be adjusted according to it. IF windows autoAdjustSize = true 

    private Vector2 screenSizeMultiplier = new Vector2(0,0);
    private Action actionToPerform;
    private int lastActive = -1;

	// Use this for initialization
	public void Start () {
        screenSizeMultiplier.x = Screen.width / defaultScreenSize.x;
        screenSizeMultiplier.y = Screen.height / defaultScreenSize.y;

        if (windows.Length > 0) {
            for (int i = 0; i < windows.Length; i++) {
                windows[i].SetParent(this);
                windows[i].enabled = false;
                windows[i].SetIndex(i);
                windows[i].SetScreenSizeMultiplier(screenSizeMultiplier);
            }

            windows[activeWindow].enabled = true;
        }

        // lastActive = activeWindow;
	}
	
	// Update is called once per frame
	public void Update () {
        if (actionToPerform != Action.none) {

            if (windows.Length > 0) {
                for (int i = 0; i < windows.Length; i++) {
                    if (windows[i].GetAction() != Action.none) {
                        actionToPerform = windows[i].GetAction();
                        windows[i].SetAction(Action.none);
                        lastActive = i;
                        break;
                    }
                }
            }

            float WinParam;

            switch (actionToPerform) {

                case Action.close:
                    windows[lastActive].enabled = false;
                    break;

                case Action.close_GoToWindow:
                    WinParam = windows[lastActive].GetActionParameter();
                    windows[lastActive].enabled = false;
                    if (windows.Length >= WinParam) {
                        windows[(int)WinParam].enabled = true;
                        activeWindow = (int)WinParam;
                    }
                    break;

                case Action.GoToWindow:
                    WinParam = windows[lastActive].GetActionParameter();
                    if (windows.Length >= WinParam) {
                        windows[(int)WinParam].enabled = true;
                        activeWindow = (int)WinParam;
                    }
                    break;


                case Action.close_GoToNextWindow:
                    windows[lastActive].enabled = false;
                    if (windows.Length >= lastActive + 1) {
                        windows[lastActive + 1].enabled = true;
                        activeWindow = lastActive + 1;
                    }
                    break;

                case Action.close_GoToPreviousWindow:
                    windows[lastActive].enabled = false;
                    if (lastActive - 1 >= 0) {
                        windows[lastActive - 1].enabled = true;
                        activeWindow = lastActive - 1;
                    }
                    break;

                case Action.GoToNextWindow:
                    if (windows.Length >= lastActive + 1) {
                        windows[lastActive + 1].enabled = true;
                        activeWindow = lastActive + 1;
                    }
                    break;

                case Action.GoToPreviousWindow:
                    if (lastActive - 1 >= 0) {
                        windows[lastActive - 1].enabled = true;
                        activeWindow = lastActive - 1;
                    }
                    break;


                case Action.close_MenuManager:
                    if (windows.Length > 0) {
                        for (int i = 0; i < windows.Length; i++) { windows[i].enabled = false; }
                    }
                    this.enabled = false;
                    break;
            }

            actionToPerform = Action.none;
        }  
	}

    public void OnEnable () {
        if (windows[activeWindow]) windows[activeWindow].enabled = true;
    }

    public void OnDisable () {
       if (windows[activeWindow]) windows[activeWindow].enabled = false;
    }

    //----------------------------------------------------------------------------------
    // Functions  for set/get local action variable
    public void SetAction (Action action) {
      actionToPerform = action;
    }

    public Action GetAction () {
      return actionToPerform;
    }

    //----------------------------------------------------------------------------------
}
