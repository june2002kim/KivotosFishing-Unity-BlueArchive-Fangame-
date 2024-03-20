using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUp : MonoBehaviour
{
    [SerializeField] CommentPopup commentPopup;
    [SerializeField] private float scrollSpeed = 2f;

    void Update()
    {
        if(!commentPopup.lastComment)
        {
            this.transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
        }
    }
}
