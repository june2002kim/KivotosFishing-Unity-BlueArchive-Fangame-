using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextData textData;
    [SerializeField] private FisingManager fishingManager;

    [Header("UGUI")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("Variables")]
    [SerializeField] private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && fishingManager.shirokoPhase == fishingPhase.TALKING)
        {
            // reset emoji
            textData.dialogueStrings[index].UpEmojiLocation.SetActive(false);
            textData.dialogueStrings[index].DownEmojiLocation.SetActive(false);

            // show dialog
            profileImage.sprite = textData.dialogueStrings[index].CharacterInfo.ProfileImage[textData.dialogueStrings[index].profileIndex];

            if(textData.dialogueStrings[index].IsUnknown)
            {
                nameText.text = "???";
            }
            else
            {
                nameText.text = textData.dialogueStrings[index].CharacterInfo.CharacterName.ToString() + " <#87CEFA><sub>" + textData.dialogueStrings[index].CharacterInfo.CharacterSchool.ToString() + "</color></sub>";
            }

            if(textData.dialogueStrings[index].IsUpEmoji)
            {
                textData.dialogueStrings[index].UpEmojiLocation.SetActive(true);
                textData.dialogueStrings[index].UpEmojiLocation.GetComponent<SpriteRenderer>().sprite = textData.dialogueStrings[index].UpEmoji;
            }
            else
            {
                textData.dialogueStrings[index].UpEmojiLocation.SetActive(false);
            }

            if(textData.dialogueStrings[index].IsDownEmoji)
            {
                textData.dialogueStrings[index].DownEmojiLocation.SetActive(true);
                textData.dialogueStrings[index].DownEmojiLocation.GetComponent<SpriteRenderer>().sprite = textData.dialogueStrings[index].DownEmoji;
            }
            else
            {
                textData.dialogueStrings[index].DownEmojiLocation.SetActive(false);
            }

            dialogText.text = textData.dialogueStrings[index].DialogueText.ToString();

            index++;
        }
    }
}
