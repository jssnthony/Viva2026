using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    private GUIStyle timerStyle;
    private GUIStyle livesStyle;
    private GUIStyle messageStyle;
    private GUIStyle subStyle;
    private bool stylesInitialized;
    private bool restartRequested;

    void InitStyles()
    {
        if (stylesInitialized) return;
        stylesInitialized = true;

        timerStyle = new GUIStyle();
        timerStyle.fontSize = 36;
        timerStyle.alignment = TextAnchor.UpperRight;
        timerStyle.normal.textColor = Color.white;

        livesStyle = new GUIStyle();
        livesStyle.fontSize = 28;
        livesStyle.alignment = TextAnchor.UpperLeft;
        livesStyle.normal.textColor = Color.red;

        messageStyle = new GUIStyle();
        messageStyle.fontSize = 52;
        messageStyle.alignment = TextAnchor.MiddleCenter;

        subStyle = new GUIStyle();
        subStyle.fontSize = 24;
        subStyle.alignment = TextAnchor.MiddleCenter;
        subStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        InitStyles();

        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        if (!gm.IsGameOver)
        {
            DrawTimer(gm);
            DrawLives(gm);
        }
        else
        {
            DrawGameOver(gm);
        }
    }

    void DrawTimer(GameManager gm)
    {
        int minutes = Mathf.FloorToInt(gm.CurrentTime / 60f);
        int seconds = Mathf.FloorToInt(gm.CurrentTime % 60f);
        string timeStr = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerStyle.normal.textColor = gm.CurrentTime <= 30f ? Color.red : Color.white;
        GUI.Label(new Rect(Screen.width - 160, 10, 150, 50), timeStr, timerStyle);
    }

    void DrawLives(GameManager gm)
    {
        string livesStr = "";
        for (int i = 0; i < gm.CurrentLives; i++)
            livesStr += "\u2665 ";
        GUI.Label(new Rect(10, 10, 200, 50), livesStr, livesStyle);
    }

    void Update()
    {
        if (restartRequested)
        {
            restartRequested = false;
            GameManager gm = GameManager.Instance;
            if (gm != null) gm.Restart();
        }
    }

    void DrawGameOver(GameManager gm)
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

        string message = gm.IsVictory ? "\u00a1VICTORIA!" : "DERROTA";
        messageStyle.normal.textColor = gm.IsVictory ? Color.green : Color.red;
        GUI.Label(new Rect(0, Screen.height / 2f - 70, Screen.width, 60), message, messageStyle);

        GUI.Label(new Rect(0, Screen.height / 2f + 10, Screen.width, 40), "Toca para reiniciar", subStyle);

        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.TouchDown)
        {
            restartRequested = true;
            Event.current.Use();
        }
    }
}
