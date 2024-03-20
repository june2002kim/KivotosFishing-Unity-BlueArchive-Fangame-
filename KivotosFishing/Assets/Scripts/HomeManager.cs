using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("------UGUI------")]
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private GameObject unlockPanel;

    [Header("------ACHIEVEMENT------")]
    [SerializeField] private FishData[] checkFish;
    [SerializeField] private GameObject newBg;
    [SerializeField] private GameObject trophy;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI infText;
    [SerializeField] private TextMeshProUGUI quitText;
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private Image volumeButton;
    [SerializeField] private Image journalButton;
    [SerializeField] private Sprite whiteVolume;
    [SerializeField] private Sprite whiteJournal;
    private bool allClear;

    void Start()
    {
        if(PlayerPrefs.GetInt("hasCleared", 0) == 1)
        {
            StartCoroutine(ShowUnlockPanel());
        }

        allClear = true;
        CheckAchievement();
    }

    private void CheckAchievement()
    {
        for(int i = 0; i < checkFish.Length ; i++)
        {
            if(PlayerPrefs.GetInt(checkFish[i].FishName, 0) == 0)
            {
                allClear = false;
                break;
            }
        }

        if(allClear)
        {
            GotTrophy();
        }
    }

    private void GotTrophy()
    {
        newBg.SetActive(true);
        trophy.SetActive(true);

        titleText.color = Color.white;
        storyText.color = Color.white;
        infText.color = Color.white;
        quitText.color = Color.white;
        myText.color = Color.white;

        volumeButton.sprite = whiteVolume;
        journalButton.sprite = whiteJournal;
    }

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

    IEnumerator ShowUnlockPanel()
    {
        unlockPanel.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        unlockPanel.SetActive(false);
    }
}
