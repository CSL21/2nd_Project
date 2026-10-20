using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Presents nine authored stage slots and one replayable procedural run.
/// </summary>
public class BattleMapSelector : MonoBehaviour
{
    private static readonly string[] StageTemplates =
    {
        "StageScene1", "StageScene2", "StageScene3",
        "StageScene1", "StageScene2", "StageScene3",
        "StageScene1", "StageScene2", "StageScene3",
        "StageScene1"
    };

    private GUIStyle titleStyle;
    private GUIStyle buttonStyle;
    private GUIStyle backStyle;

    private void OnGUI()
    {
        EnsureStyles();

        GUI.color = new Color(0.055f, 0.07f, 0.10f, 0.97f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        float panelWidth = Mathf.Min(760f, Screen.width - 40f);
        float panelHeight = Mathf.Min(620f, Screen.height - 40f);
        float left = (Screen.width - panelWidth) * 0.5f;
        float top = (Screen.height - panelHeight) * 0.5f;
        GUI.Box(new Rect(left, top, panelWidth, panelHeight), GUIContent.none);
        GUI.Label(new Rect(left + 20f, top + 20f, panelWidth - 40f, 70f), "SELECT A MAP", titleStyle);

        const int columns = 3;
        const float gap = 14f;
        float gridWidth = panelWidth - 60f;
        float cellWidth = (gridWidth - gap * (columns - 1)) / columns;
        int rows = Mathf.CeilToInt(StageTemplates.Length / (float)columns);
        float cellHeight = Mathf.Min(112f, (panelHeight - 190f - gap * (rows - 1)) / rows);
        float gridLeft = left + 30f;
        float gridTop = top + 110f;

        for (int i = 0; i < StageTemplates.Length; i++)
        {
            int column = i % columns;
            int row = i / columns;
            Rect buttonRect = new Rect(
                gridLeft + column * (cellWidth + gap),
                gridTop + row * (cellHeight + gap),
                cellWidth,
                cellHeight);
            string label = i == 9 ? "MAP 10\nRANDOM RUN" : "MAP " + (i + 1);

            if (GUI.Button(buttonRect, label, buttonStyle))
            {
                PlayerPrefs.SetInt("SelectedMap", i + 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene(StageTemplates[i]);
            }
        }

        if (GUI.Button(new Rect(left + 30f, top + panelHeight - 58f, 180f, 42f), "BACK TO LOBBY", backStyle))
            SceneManager.LoadScene("LobbyScene");
    }

    private void EnsureStyles()
    {
        if (titleStyle != null) return;

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 34,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };
        backStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 18
        };
    }
}
