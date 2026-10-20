using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // MainScene

    public void ClickStart()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    // LobbyScene

    public void ClickReturntoMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ClickStageScene1()
    {
        SceneManager.LoadScene("StageScene1");
    }

    public void ClickStageScene2()
    {
        SceneManager.LoadScene("StageScene2");
    }

    public void ClickStageScene3()
    {
        SceneManager.LoadScene("StageScene3");
    }


    //

    public void ReturntoLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }





    // 종료 버튼
    public void ClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();

#endif
    }



}