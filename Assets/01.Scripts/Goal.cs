using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoalObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI clearText;

    [SerializeField] private int goldReward = 500;
    [SerializeField] private int statPointReward = 10;

    private bool isCleared = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCleared) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isCleared = true;

            Debug.Log("클리어!");

            if (clearText != null)
            {
                clearText.text = "Clear!";
                clearText.gameObject.SetActive(true);
            }

            if (PlayerStatus.instance != null)
            {
                PlayerStatus.instance.Gold += goldReward;
                PlayerStatus.instance.StatPoint += statPointReward;
                Debug.Log($"보상 지급 완료: +{goldReward} Gold, +{statPointReward} StatPoint");
            }

            StartCoroutine(ReturnToMainSceneAfterDelay());
        }
    }

    IEnumerator ReturnToMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LobbyScene");
    }
}