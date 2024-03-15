using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPanel;

    public void OnStoryClicked()
    {
        SceneManager.LoadScene("OP");
    }

    public void OnInfClicked()
    {
        if(PlayerPrefs.GetInt("hasCleared", 0) == 0)
        {
            StartCoroutine(ShowBlockPanel());
        }
        else
        {
            SceneManager.LoadScene("Inf");
        }
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    IEnumerator ShowBlockPanel()
    {
        blockPanel.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        blockPanel.SetActive(false);
    }
}
