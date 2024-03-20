using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentPopup : MonoBehaviour
{
    [SerializeField] public CommentData commentData;
    [SerializeField] private GameObject commentPrefab;
    public int commentIdx = 0;
    public bool lastComment = false;

    void Start()
    {
        StartCoroutine(Chatting());
    }

    private IEnumerator Chatting()
    {
        while(commentIdx < commentData.commentStrings.Count)
        {
            Instantiate(commentPrefab, this.transform);

            yield return new WaitForSeconds(2f);

            commentIdx++;
        }

        lastComment = true;
    }
}
