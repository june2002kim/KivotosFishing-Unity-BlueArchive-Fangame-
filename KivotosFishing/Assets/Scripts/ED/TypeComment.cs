using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeComment : MonoBehaviour
{
    [SerializeField] private Image badge;
    [SerializeField] private TextMeshProUGUI headText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Sprite liveBadge;
    [SerializeField] private Sprite viewBadge;

    private CommentPopup commentPopup;

    void Start()
    {
        commentPopup = FindObjectOfType<CommentPopup>();

        if(commentPopup.commentData.commentStrings[commentPopup.commentIdx].IsStreamer)
        {
            badge.sprite = liveBadge;
        }
        else
        {
            badge.sprite = viewBadge;
        }

        headText.color = randomColor();
        headText.text = commentPopup.commentData.commentStrings[commentPopup.commentIdx].NicknameText;

        bodyText.text = commentPopup.commentData.commentStrings[commentPopup.commentIdx].CommentText;
    }

    private Color randomColor()
    {
        int colorPick = Random.Range(0, 7);
        Color ret;

        if(colorPick == 0)
        {
            ret = Color.blue;
        }
        else if(colorPick == 1)
        {
            ret = Color.cyan;
        }
        else if(colorPick == 2)
        {
            ret = Color.green;
        }
        else if(colorPick == 3)
        {
            ret = Color.magenta;
        }
        else if(colorPick == 4)
        {
            ret = Color.red;
        }
        else if(colorPick == 5)
        {
            ret = Color.white;
        }
        else if(colorPick == 6)
        {
            ret = Color.yellow;
        }
        else
        {
            ret = Color.gray;
        }

        return ret;
    }

}
