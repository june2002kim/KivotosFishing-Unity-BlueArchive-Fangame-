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
    [SerializeField] private CharacterData characterinfo;
    public CharacterData CharacterInfo {get {return characterinfo;}}

    [SerializeField] private int profileindex;
    public int profileIndex {get {return profileindex;}}

    [SerializeField] [TextArea(3, 5)]private string dialoguetext;
    public string DialogueText {get {return dialoguetext;}}

    [SerializeField] private bool lasttext;
    public bool LastText {get {return lasttext;}}

    [SerializeField] private bool isUnknown;
    public bool IsUnknown {get {return isUnknown;}}

    [SerializeField] private bool isUpEmoji;
    public bool IsUpEmoji {get {return isUpEmoji;}}

    [SerializeField] private Sprite upEmoji;
    public Sprite UpEmoji {get {return upEmoji;}}

    [SerializeField] private GameObject upEmojiLocation;
    public GameObject UpEmojiLocation {get {return upEmojiLocation;}}

    [SerializeField] private bool isDownEmoji;
    public bool IsDownEmoji {get {return isDownEmoji;}}

    [SerializeField] private Sprite downEmoji;
    public Sprite DownEmoji {get {return downEmoji;}}

    [SerializeField] private GameObject downEmojiLocation;
    public GameObject DownEmojiLocation {get {return downEmojiLocation;}}
}