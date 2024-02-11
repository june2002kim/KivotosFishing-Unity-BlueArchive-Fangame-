using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextData : MonoBehaviour
{
    [SerializeField] public List<dialogueString> dialogueStrings = new List<dialogueString>();
}

[System.Serializable]
public class dialogueString
{
    [SerializeField] private CharacterData characterInfo;
    public CharacterData CharacterInfo {get {return characterInfo;}}

    [SerializeField] [TextArea(3, 5)]private string dialogueText;
    public string DialogueText {get {return dialogueText;}}

    [SerializeField] private bool lastText;
    public bool LastText {get {return lastText;}}
}