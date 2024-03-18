using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private FishData fishdata;
    [SerializeField] private UnityEngine.UI.Image buttonImage;

    [Header("------Journal Right Page------")]
    [SerializeField] private GameObject journalPage;
    [SerializeField] private UnityEngine.UI.Image journalImage;
    [SerializeField] private TextMeshProUGUI journalRarity;
    [SerializeField] private TextMeshProUGUI journalCnt;
    [SerializeField] private TextMeshProUGUI journalName;
    [SerializeField] private TextMeshProUGUI journalDescription;

    [Header("------Audio Clip------")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip pageClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        buttonImage.sprite = fishdata.FishImage;

        if(PlayerPrefs.GetInt(fishdata.FishName, 0) == 0)
        {
            buttonImage.color = Color.black;
        }
        else
        {
            buttonImage.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        highlight.SetActive(true);

        audioSource.clip = hoverClip;
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        highlight.SetActive(false);
    }

    public void OnClick()
    {
        journalPage.SetActive(true);

        if(PlayerPrefs.GetInt(fishdata.FishName, 0) == 0)
        {
            // empty journal
            journalImage.sprite = fishdata.FishImage;
            journalImage.color = Color.black;
            journalRarity.text = "???";
            if(fishdata.FishRarity == fishrarity.SSR)
            {
                journalRarity.color = Color.magenta;
            }
            else if(fishdata.FishRarity == fishrarity.SR)
            {
                journalRarity.color = Color.yellow;
            }
            else if(fishdata.FishRarity == fishrarity.R)
            {
                journalRarity.color = Color.cyan;
            }
            journalCnt.text = "0";
            journalName.text = "???";
            journalDescription.text = "???";
        }
        else
        {
            journalImage.sprite = fishdata.FishImage;
            journalImage.color = Color.white;
            journalRarity.text = fishdata.FishRarity.ToString();
            if(fishdata.FishRarity == fishrarity.SSR)
            {
                journalRarity.color = Color.magenta;
            }
            else if(fishdata.FishRarity == fishrarity.SR)
            {
                journalRarity.color = Color.yellow;
            }
            else if(fishdata.FishRarity == fishrarity.R)
            {
                journalRarity.color = Color.cyan;
            }
            journalCnt.text = PlayerPrefs.GetInt(fishdata.FishName).ToString();
            journalName.text = fishdata.FishName.ToString();
            journalDescription.text = fishdata.FishDescription.ToString();
        }

        audioSource.clip = pageClip;
        audioSource.Play();
    }
}
