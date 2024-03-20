using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pointTrophy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject trophy;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        trophy.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        trophy.SetActive(false);
    }
}
