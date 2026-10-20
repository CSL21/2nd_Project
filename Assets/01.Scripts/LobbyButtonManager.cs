using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButtonManager : MonoBehaviour
{
    public GameObject shopObject;
    public GameObject Inventory;

    public GameObject multiXImage;
    public GameObject pierceXImage;


    public void OpenInv()
    {
        if (Inventory != null)

            Inventory.SetActive(true);

    }

    public void CloseInv()
    {
        if (Inventory != null)

            Inventory.SetActive(false);
    }


    public void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void CloseShop()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}