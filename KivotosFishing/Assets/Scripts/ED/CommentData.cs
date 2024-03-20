using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommentData : MonoBehaviour
{
    [SerializeField] public List<commentString> commentStrings = new List<commentString>();
}

[System.Serializable]
public class commentString
{
    [SerializeField] [TextArea(3, 5)]private string nicknametext;
    public string NicknameText {get {return nicknametext;}}

    [SerializeField] [TextArea(3, 5)]private string commenttext;
    public string CommentText {get {return commenttext;}}

    [SerializeField] private bool isstreamer;
    public bool IsStreamer {get {return isstreamer;}}
}