using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextData textData;

    [Header("UGUI")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI schoolText;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("Variables")]
    [SerializeField] private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //profileImage.sprite = textData.dialogueStrings[index].CharacterInfo.ProfileImage[0];
            nameText.text = textData.dialogueStrings[index].CharacterInfo.name.ToString();
            schoolText.text = textData.dialogueStrings[index].CharacterInfo.CharacterSchool.ToString();
            dialogText.text = textData.dialogueStrings[index].DialogueText.ToString();

            index++;
        }
    }
}
