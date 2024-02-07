using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class clockHand : MonoBehaviour
{
    [SerializeField] public bool isIn;

    private void OnTriggerEnter2D()
    {
        isIn = true;
    }

    private void OnTriggerExit2D()
    {
        isIn = false;
    }
}
