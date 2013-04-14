using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public int gamePoints;
    public PlayerStats player;

    // HUD Properties
    int buttonWidth, buttonHeight;
    int HUDWindowHeight, HUDWindowWidth;
    int bottomWindowMargin;
    int leftIndent, topIndent;

    // GO Properties
    int GOButtonHeight;
    int GOWindowHeight, GOWindowWidth;
    int GOTopIndent, GOLeftIndent;
    GUIStyle GameOverText = new GUIStyle();

	// Use this for initialization
	void Start () {
        SetHUDProperties();
        SetGOProperties();
        Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.lockCursor = false;
        }
	}

    void SetHUDProperties()
    {
        buttonWidth = Screen.width / 10;
        buttonHeight = 40;
        HUDWindowHeight = buttonHeight;
        HUDWindowWidth = 4 * (buttonWidth + 20);
        bottomWindowMargin = 30;
        leftIndent = (Screen.width / 2) - (HUDWindowWidth / 2);
        topIndent = Screen.height - HUDWindowHeight - bottomWindowMargin;
    }

    void SetGOProperties()
    {
        GOWindowWidth = Screen.width / 4;
        GOWindowHeight = Screen.height / 3;
        GOLeftIndent = (Screen.width / 2) - (GOWindowWidth / 2);
        GOTopIndent = (Screen.height / 2) - (GOWindowHeight / 2);
        GOButtonHeight = GOWindowHeight / 4;

        GameOverText.fontSize = GOButtonHeight;
        GameOverText.fontStyle = FontStyle.Bold;
        GameOverText.normal.textColor = Color.red;
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

    void ShowGameOver(int windowID)
    {
        GUIStyle GOButtonText = new GUIStyle(GUI.skin.button);
        GOButtonText.fontSize = GOButtonHeight / 2;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("GAME OVER", GameOverText);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("RETRY", GOButtonText, GUILayout.Height(GOButtonHeight)))
        {
            Application.LoadLevel("ThirdPersonShooterScene1");
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("QUIT", GOButtonText, GUILayout.Height(GOButtonHeight)))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        GUIStyle windowStyleHUD = new GUIStyle(GUI.skin.window);
        windowStyleHUD.padding = new RectOffset(5, 5, 12, 12);
        GUIStyle windowStyleGO = new GUIStyle(GUI.skin.window);
        windowStyleGO.padding = new RectOffset(20, 20, 20, 20);

        if (Input.GetKey(KeyCode.Space))
        {
            GUILayout.Window(0, new Rect(leftIndent, topIndent, HUDWindowWidth, HUDWindowHeight), ShowCommandHUD, "", windowStyleHUD);
        }
        if (player.GetHealth() <= 0)
        {
            Screen.lockCursor = false;
            GUILayout.Window(1, new Rect(GOLeftIndent, GOTopIndent, GOWindowWidth, GOWindowHeight), ShowGameOver, "", windowStyleGO);
        }     
    }
}
