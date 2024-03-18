using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEKey : MonoBehaviour
{
    [SerializeField] public List<Sprite> QTEKeys;
    Color passKeyColor = new Color (0.5f, 0.5f, 0.5f, 0.5f);

    public void changeKey(int index)
    {
        Image image = GetComponent<Image>();
        image.sprite = QTEKeys[index];
    }

    public void changeColor()
    {
        Image image = GetComponent<Image>();
        image.color = passKeyColor;
    }
}
