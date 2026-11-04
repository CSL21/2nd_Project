using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // MainScene

    public void ClickStart()
    {
        SoundManager.instance.ChangeBGM(BGMType.GameScene);

        SceneManager.LoadScene("LobbyScene");
    }

    // LobbyScene

    public void ClickReturntoMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ClickStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
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

    public void ClickStageScene4()
    {
        SceneManager.LoadScene("StageScene4");
    }

    public void ClickStageScene5()
    {
        SceneManager.LoadScene("StageScene5");
    }

    public void ClickDefenceScene()
    {
        SceneManager.LoadScene("DefenseScene");
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