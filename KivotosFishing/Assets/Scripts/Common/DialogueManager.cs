using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;

public class DialogueManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private TextData[] textDatas;

    [Header("------UGUI------")]
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("------Variables------")]
    [SerializeField] public int SceneIndex = 0;
    [SerializeField] public int lineIndex = 0;
    [SerializeField] public bool lastLine = false;

    private AudioSource dialogAudioSource;

    void Start()
    {
        dialogAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lineIndex < textDatas[SceneIndex].dialogueStrings.Count)
            {
                // show dialogBox
                if (lineIndex == 0)
                {
                    dialogBox.SetActive(true);
                }
                else
                {
                    // reset emoji
                    textDatas[SceneIndex].dialogueStrings[lineIndex - 1].UpEmojiLocation.SetActive(false);
                    textDatas[SceneIndex].dialogueStrings[lineIndex - 1].DownEmojiLocation.SetActive(false);
                }

                // show profile image
                profileImage.sprite = textDatas[SceneIndex].dialogueStrings[lineIndex].CharacterInfo.ProfileImage[textDatas[SceneIndex].dialogueStrings[lineIndex].profileIndex];

                // write name
                if (textDatas[SceneIndex].dialogueStrings[lineIndex].IsUnknown)
                {
                    nameText.text = "???";
                }
                else
                {
                    nameText.text = textDatas[SceneIndex].dialogueStrings[lineIndex].CharacterInfo.CharacterName.ToString() + " <#87CEFA><sub>" + textDatas[SceneIndex].dialogueStrings[lineIndex].CharacterInfo.CharacterSchool.ToString() + "</color></sub>";
                }

                // show up emoji
                if (textDatas[SceneIndex].dialogueStrings[lineIndex].IsUpEmoji)
                {
                    textDatas[SceneIndex].dialogueStrings[lineIndex].UpEmojiLocation.SetActive(true);
                    textDatas[SceneIndex].dialogueStrings[lineIndex].UpEmojiLocation.GetComponent<SpriteRenderer>().sprite = textDatas[SceneIndex].dialogueStrings[lineIndex].UpEmoji;
                }
                else
                {
                    textDatas[SceneIndex].dialogueStrings[lineIndex].UpEmojiLocation.SetActive(false);
                }

                // show down emoji
                if (textDatas[SceneIndex].dialogueStrings[lineIndex].IsDownEmoji)
                {
                    textDatas[SceneIndex].dialogueStrings[lineIndex].DownEmojiLocation.SetActive(true);
                    textDatas[SceneIndex].dialogueStrings[lineIndex].DownEmojiLocation.GetComponent<SpriteRenderer>().sprite = textDatas[SceneIndex].dialogueStrings[lineIndex].DownEmoji;
                }
                else
                {
                    textDatas[SceneIndex].dialogueStrings[lineIndex].DownEmojiLocation.SetActive(false);
                }

                // sing
                if (textDatas[SceneIndex].dialogueStrings[lineIndex].IsSinging)
                {
                    dialogAudioSource.clip = textDatas[SceneIndex].dialogueStrings[lineIndex].SongClip;
                    dialogAudioSource.Play();
                }

                // write text
                dialogText.text = textDatas[SceneIndex].dialogueStrings[lineIndex].DialogueText.ToString();

                lineIndex++;
            }
            else if (lineIndex == textDatas[SceneIndex].dialogueStrings.Count)
            {
                dialogBox.SetActive(false);

                textDatas[SceneIndex].dialogueStrings[lineIndex - 1].UpEmojiLocation.SetActive(false);
                textDatas[SceneIndex].dialogueStrings[lineIndex - 1].DownEmojiLocation.SetActive(false);

                if(textDatas[SceneIndex].dialogueStrings[lineIndex - 1].LastText)
                {
                    lastLine = true;
                }

                lineIndex++;
            }
        }
    }
}
