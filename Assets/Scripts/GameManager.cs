using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    int buttonWidth, buttonHeight;
    int HUDWindowHeight, HUDWindowWidth;
    int bottomWindowMargin;
    int leftIndent, topIndent;

	// Use this for initialization
	void Start () {    
        SetWindowProperties();
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Screen.lockCursor = false;
        //}
        //else
        //{
        //    Screen.lockCursor = true;
        //}
	}

    void SetWindowProperties()
    {
        buttonWidth = Screen.width / 10;
        buttonHeight = 40;
        HUDWindowHeight = buttonHeight;
        HUDWindowWidth = 4 * (buttonWidth + 20);
        bottomWindowMargin = 30;
        leftIndent = (Screen.width / 2) - (HUDWindowWidth / 2);
        topIndent = Screen.height - HUDWindowHeight - bottomWindowMargin;
    }

    void ShowCommandHUD(int windowID)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Assault Rifle", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) 
        {

        }
        GUILayout.Space(20);
        if (GUILayout.Button("SMG", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {

        }
        GUILayout.Space(20);
        if (GUILayout.Button("Shotgun", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {

        }
        GUILayout.Space(20);
        if (GUILayout.Button("Pistol", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {

        }
        GUILayout.EndHorizontal();
    }

    void OnGUI()
    {
        GUIStyle windowStyle = new GUIStyle(GUI.skin.window);
        windowStyle.padding = new RectOffset(5, 5, 12, 12);
        if (Input.GetKey(KeyCode.Space))
        {
            GUILayout.Window(0, new Rect(leftIndent, topIndent, HUDWindowWidth, HUDWindowHeight), ShowCommandHUD, "", windowStyle);
        }
    }
}
