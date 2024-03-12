using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum fishingPhase{
    BEFORECASTING,
    WAITING,
    CATCHING,
    ENTERGACHA,
    BLOCKGACHA,
    ENTERQTE,
    BLOCKQTE,
    ENTERDBD,
    BLOCKDBD,
    ENTERTURBO,
    BLOCKTURBO,
    ENTERRECORD,
    BLOCKRECORD,
    TALKING
}

public class FisingManager : MonoBehaviour
{
    [Header("Variables")]
    public fishingPhase shirokoPhase;

    [SerializeField] private float gachaRate;
    public float GachaRate {get {return gachaRate;}}

    [SerializeField] private float minGachaRate;
    public float MinGachaRate {get {return minGachaRate;}}

    [SerializeField] private float maxGachaRate;
    public float MaxGachaRate {get {return maxGachaRate;}}

    [SerializeField] private float reactionTime;
    public float ReactionTime {get {return reactionTime;}}

    private float currTime;
    private float timerTime;
    private bool stopTimer;

    private Animator shirokoAnimator;

    private void Awake()
    {
        shirokoAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        resetPhase();
    }

    private void Update()
    {
        if(shirokoPhase == fishingPhase.WAITING)
        {
            currTime += Time.deltaTime;

            if(currTime >= gachaRate)
            {
                Debug.Log("!!!");
                shirokoPhase = fishingPhase.CATCHING;
                shirokoAnimator.SetBool("isReeling", true);

                StartCoroutine(Timer());
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(shirokoPhase == fishingPhase.BEFORECASTING)
            {
                shirokoAnimator.SetBool("isCasting", true);
                shirokoPhase = fishingPhase.WAITING;
                shirokoAnimator.SetBool("isIdle", false);
                shirokoAnimator.SetBool("isWaiting", true);
            }
            else if(shirokoPhase == fishingPhase.WAITING)
            {
                resetPhase();
            }
            else if(shirokoPhase == fishingPhase.CATCHING)
            {
                Debug.Log("Catch!");
                stopTimer = true;
                shirokoPhase = fishingPhase.ENTERGACHA;
            }
        }
    }

    public float SetGachaRate()
    {
        return Random.Range(minGachaRate, maxGachaRate);
    }

    public void resetPhase()
    {
        shirokoPhase = fishingPhase.BEFORECASTING;
        resetAnimBool();
        shirokoAnimator.SetBool("isIdle", true);

        gachaRate = SetGachaRate();
        currTime = 0;
        resetTimer();
    }

    public void resetAnimBool()
    {
        shirokoAnimator.SetBool("isCasting", false);
        shirokoAnimator.SetBool("isWaiting", false);
        shirokoAnimator.SetBool("isReeling", false);
        shirokoAnimator.SetBool("isJumping", false);
        shirokoAnimator.SetBool("isCatching", false);
    }

    private IEnumerator Timer()
    {
        while(!stopTimer)
        {
            timerTime -= Time.deltaTime;
            yield return new WaitForSeconds(Unity.Mathematics.math.EPSILON);

            if(timerTime <= 0)
            {
                stopTimer = true;
            }
        }

        if(stopTimer && timerTime<=0)
        {
            Debug.Log("TIME OUT!");
            resetPhase();
        }
    }

    private void resetTimer()
    {
        timerTime = reactionTime;
        stopTimer = false;
    }
}
