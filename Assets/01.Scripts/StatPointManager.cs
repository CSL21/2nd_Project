using UnityEngine;
using UnityEngine.SceneManagement;

public class StatButtonController : MonoBehaviour
{
    private void RefreshLobbyUI()
    {
        UIManager uiManager = Object.FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateLobbyUI();
        }
    }

    public void ClickHp()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.TryUpgradeHp())
        {
            RefreshLobbyUI();
        }
    }

    public void ClickATK()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.TryUpgradeAtk())
        {
            RefreshLobbyUI();
        }
    }

    public void ClickATS()
    {
        if (PlayerStatus.instance == null) return;

        if (PlayerStatus.instance.TryUpgradeAts())
        {
            RefreshLobbyUI();
        }
    }
}