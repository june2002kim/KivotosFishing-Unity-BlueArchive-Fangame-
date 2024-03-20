using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("------References------")]
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private QTEAlignment qteAlignment;
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] public GameObject QTECanvas;

    [Header("------variables------")]
    [SerializeField] public int[] arrowDirection = new int[5];
    [SerializeField] private bool isWatingQTE;
    [SerializeField] private int keyCount;
    [SerializeField] private int leftRight;
    [SerializeField] private int index;
    [SerializeField] private bool isFailed;

    [Header("------UGUI------")]
    [SerializeField] public GameObject qteCanvas;
    [SerializeField] private GameObject KeyPanel;
    [SerializeField] private GameObject TimerPanel;
    [SerializeField] private GameObject QTECam;
    
    [Header("------MIRROR------")]
    [SerializeField] private GameObject QTECam_;
    [SerializeField] private RectTransform keyTransform_;
    [SerializeField] private RectTransform timerTransform_;

    [Header("------Timer------")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float sliderTime = 4f;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool stopTimer;

    [Header("------Audio------")]
    [SerializeField] private AudioClip goodClip;
    [SerializeField] private AudioClip badClip;

    private AudioSource qteAudioSource;

    // timeKeeper
    WaitForSeconds secEpsilon = new WaitForSeconds(Unity.Mathematics.math.EPSILON);


    void Start()
    {
        qteAlignment = KeyPanel.GetComponent<QTEAlignment>();
        qteAudioSource = GetComponent<AudioSource>();

        isWatingQTE = false;
        isFailed = false;
        index = 0;
    }

    void Update()
    {
        if (!isWatingQTE && fishingManager.shirokoPhase == fishingPhase.ENTERQTE)
        {
            if(SceneManager.GetActiveScene().name == "Ice")
            {
                if(fishingManager.isFacingRight)
                {
                    QTECam.SetActive(true);
                    
                    keyTransform_.offsetMax = Vector2.zero;
                    timerTransform_.offsetMax = Vector2.zero;

                    keyTransform_.offsetMin = new Vector2(300, 0);
                    timerTransform_.offsetMin = new Vector2(300, 0);
                }
                else
                {
                    QTECam_.SetActive(true);
                    
                    keyTransform_.offsetMin = Vector2.zero;
                    timerTransform_.offsetMin = Vector2.zero;

                    keyTransform_.offsetMax = new Vector2(-300, 0);
                    timerTransform_.offsetMax = new Vector2(-300, 0);
                }
            }
            else
            {
                QTECam.SetActive(true);
            }
            
            qteCanvas.SetActive(true);
            StartCoroutine(StartQTE());
            fishingManager.shirokoPhase = fishingPhase.BLOCKQTE;
        }

        if (isWatingQTE && Input.anyKeyDown)
        {
            if (leftRight == 0)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    isFailed = false;
                }
                else
                {
                    isFailed = true;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    isFailed = false;
                }
                else if (!Input.GetKey(KeyCode.LeftArrow))
                {
                    isFailed = true;
                }
            }

            if(isFailed)
            {
                qteAudioSource.clip = badClip;
            }
            else
            {
                qteAudioSource.clip = goodClip;
            }
            qteAudioSource.Play();

            StartCoroutine(PlayQTE());
        }
    }

    private int Setting()
    {
        string rarity;
        int keyCount = 3;
        int leftRight;

        rarity = gachaManager.fish.fishData.FishRarity.ToString();
        Debug.Log("Rarity : " + rarity);

        keyCount = rarity.Length + 2;
        Debug.Log(keyCount);

        QTEAlignment keyPanelCode = KeyPanel.GetComponent<QTEAlignment>();

        keyPanelCode.gridAlignment(keyCount);

        for (int i = 0; i < keyCount; i++)
        {
            leftRight = UnityEngine.Random.Range(0, 2);
            if (leftRight != 0)
            {
                leftRight++;
            }
            arrowDirection[i] = leftRight;

            keyPanelCode.QTEKeys[i].changeKey(leftRight);
        }

        return keyCount;
    }

    private IEnumerator StartQTE()
    {
        isWatingQTE = true;

        keyCount = Setting();

        resetTimer();
        StartCoroutine(Timer());

        leftRight = arrowDirection[index];

        qteAlignment.QTEKeys[index].changeKey(leftRight + 1);

        yield return null;
    }

    private IEnumerator PlayQTE()
    {
        if (!isFailed)
        {
            qteAlignment.QTEKeys[index].changeKey(leftRight);
            qteAlignment.QTEKeys[index].changeColor();

            index++;
            if (index == qteAlignment.QTEKeys.Count)
            {
                Debug.Log("PASS!");
                
                resetQTE();

                qteCanvas.SetActive(false);
                fishingManager.shirokoPhase = fishingPhase.ENTERDBD;

                yield break;
            }
            leftRight = arrowDirection[index];

            qteAlignment.QTEKeys[index].changeKey(leftRight + 1);
        }
        else
        {
            Debug.Log("FAIL!");
            
            resetQTE();
            fishingManager.resetPhase();
            qteCanvas.SetActive(false);
        }
    }

    public void resetQTE()
    {
        qteAlignment.gridReset();

        isWatingQTE = false;
        index = 0;

        stopTimer = true;
    }

    private IEnumerator Timer()
    {
        while(!stopTimer)
        {
            currentTime -= Time.deltaTime;

            //yield return new WaitForSeconds(math.EPSILON);
            yield return secEpsilon;

            if(currentTime <= 0)
            {
                stopTimer = true;
            }

            if(!stopTimer)
            {
                timerSlider.value = currentTime;
            }
        }

        if(stopTimer && currentTime<=0)
        {
            Debug.Log("TIME OUT!");

            resetQTE();
            fishingManager.resetPhase();
            qteCanvas.SetActive(false);
        }
    }

    private void resetTimer()
    {
        timerSlider.maxValue = sliderTime;
        timerSlider.value = sliderTime;

        currentTime = sliderTime;

        stopTimer = false;
    }
}
