using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBDManager : MonoBehaviour
{
    [Header("------Reference------")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private DBDAlignment dbdAlignment;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] public GameObject dbdCanvas;

    [Header("------UGUI------")]
    [SerializeField] private GameObject DBDCanvas;
    [SerializeField] private GameObject DBDcakePrefab;
    [SerializeField] private clockHand clockHand;
    [SerializeField] private GameObject DBDCam;

    [Header("------MIRROR------")]
    [SerializeField] private GameObject DBDCam_;
    [SerializeField] private RectTransform panelTransform_;

    [Header("------Audio------")]
    [SerializeField] private AudioClip goodClip;
    [SerializeField] private AudioClip badClip;
    
    [Header("------Variables------")]
    [SerializeField] private int cakeCount;
    [SerializeField] private float cakeRot;
    [SerializeField] private bool stopTimer;
    [SerializeField] private int arrowDirection;
    [SerializeField] private bool reverseTimer;
    [SerializeField] private int index;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationWeight = 40f;
    [SerializeField] public bool isIn;

    private AudioSource dbdAudioSource;

    void Start()
    {
        dbdAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fishingManager.shirokoPhase == fishingPhase.ENTERDBD)
        {
            if(SceneManager.GetActiveScene().name == "Ice")
            {
                if(fishingManager.isFacingRight)
                {
                    DBDCam.SetActive(true);
                    
                    panelTransform_.offsetMax = Vector2.zero;

                    panelTransform_.offsetMin = new Vector2(700, 0);
                }
                else
                {
                    DBDCam_.SetActive(true);
                    
                    panelTransform_.offsetMin = Vector2.zero;

                    panelTransform_.offsetMax = new Vector2(-700, 0);
                }
            }
            else
            {
                DBDCam.SetActive(true);
            }

            dbdCanvas.SetActive(true);

            GameObject[] spawnedCake = GameObject.FindGameObjectsWithTag("Cake");
            if(spawnedCake.Length != 0)
            {
                dbdAlignment.dbdReset();
            }

            Setting();

            StartCoroutine(PlayDBD());
            fishingManager.shirokoPhase = fishingPhase.BLOCKDBD;
        }

        if(Input.GetKeyDown(KeyCode.Space) && fishingManager.shirokoPhase == fishingPhase.BLOCKDBD)
        {
            if(clockHand.isIn)
            {
                reverseTimer = true;
                isIn = true;

                dbdAudioSource.clip = goodClip;
            }
            else
            {
                stopTimer = true;

                dbdAudioSource.clip = badClip;
            }

            dbdAudioSource.Play();
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

    private IEnumerator PlayDBD()
    {
        while(!stopTimer)
        {
            
            yield return new WaitForSeconds(math.EPSILON);

            if(reverseTimer)
            {
                index++;

                if(index == cakeCount)
                {
                    stopTimer = true;
                    break;
                }
                
                arrowDirection = qteManager.arrowDirection[index] - 1;

                reverseTimer = false;
            }

            if(!stopTimer)
            {
                rotationAngle += Time.deltaTime * arrowDirection;
                // [Difficulty] clock rotation speed for each rarity
                clockHand.transform.rotation = Quaternion.Euler(0, 0, rotationAngle * rotationWeight * cakeCount);
            }
        }

        if(stopTimer)
        {
            if(reverseTimer)
            {
                Debug.Log("GOOD!");
                fishingManager.shirokoPhase = fishingPhase.ENTERTURBO;
            }
            else
            {
                Debug.Log("BAD!");
                fishingManager.resetPhase();
            }
            dbdCanvas.SetActive(false);
        }
    }
}
