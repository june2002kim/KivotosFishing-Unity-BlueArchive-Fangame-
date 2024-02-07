using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DBDManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private DBDAlignment dbdAlignment;

    [Header("UGUI")]
    [SerializeField] private GameObject DBDCanvas;
    [SerializeField] private GameObject DBDcakePrefab;
    [SerializeField] private GameObject DBDforeground;
    [SerializeField] private clockHand clockHand;
    
    [Header("Variables")]
    [SerializeField] private int cakeCount;
    [SerializeField] private float cakeRot;
    [SerializeField] private bool stopTimer;
    [SerializeField] private int arrowDirection;
    [SerializeField] private bool reverseTimer;
    [SerializeField] private int index;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationWeight = 40f;
    [SerializeField] public bool isIn;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            GameObject[] spawnedCake = GameObject.FindGameObjectsWithTag("Cake");
            if(spawnedCake.Length != 0)
            {
                dbdAlignment.dbdReset();
            }

            Setting();

            StartCoroutine(Play());
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(clockHand.isIn)
            {
                reverseTimer = true;
                isIn = true;
            }
            else
            {
                stopTimer = true;
            }
        }
    }

    public void Setting()
    {
        cakeCount = gachaManager.fish.fishData.FishRarity.ToString().Length + 2;
        
        dbdAlignment.dbdAlignment(cakeCount);

        rotationAngle = 0f;

        reverseTimer = false;

        index = 0;

        arrowDirection = qteManager.arrowDirection[index] - 1;

        stopTimer = false;

        isIn = false;
    }

    private IEnumerator Play()
    {
        while(!stopTimer)
        {
            
            yield return new WaitForSeconds(math.EPSILON);

            if(reverseTimer)
            {
                index++;
                arrowDirection = qteManager.arrowDirection[index] - 1;

                if(index == cakeCount)
                {
                    stopTimer = true;
                    break;
                }

                reverseTimer = false;
            }

            if(!stopTimer)
            {
                rotationAngle += Time.deltaTime * arrowDirection;
                clockHand.transform.rotation = Quaternion.Euler(0, 0, rotationAngle * rotationWeight * cakeCount);
            }
        }

        if(stopTimer)
        {
            if(reverseTimer)
            {
                Debug.Log("GOOD!");
            }
            else
            {
                Debug.Log("BAD!");
            }
        }
    }
}
