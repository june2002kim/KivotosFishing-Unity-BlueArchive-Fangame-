using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Penguin : MonoBehaviour
{
    [SerializeField] private GameObject happyEmoji;
    [SerializeField] private GameObject angryEmoji;
    [SerializeField] private Sprite[] emotions;
    [SerializeField] private float penguinMaxTime;
    [SerializeField] private GameObject emotionTimer;
    [SerializeField] private Image timerHandle;

    private bool stopTimer = false;
    private float currentTime;
    public bool isAngry = false;

    private Animator animator;
    private Slider slider;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        slider = emotionTimer.GetComponent<Slider>();

        ResetTimer();
        StartCoroutine(WalkIn());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            PenguinSpawner.AngryPenguinCnt++;
        }
    }

    private IEnumerator WalkIn()
    {
        animator.SetBool("isWalking", true);

        while(this.transform.position.x > 0f)
        {
            this.transform.position += Vector3.left * Time.deltaTime;

            yield return new WaitForSeconds(math.EPSILON);
        }

        animator.SetBool("isWalking", false);

        if(!isAngry)
        {
            emotionTimer.SetActive(true);
        }

        emotionTimer.SetActive(true);
        StartCoroutine(Timer());
    }

    public IEnumerator HappyOut()
    {
        stopTimer = true;
        emotionTimer.SetActive(false);

        PenguinSpawner.HappyPenguinCnt++;
        happyEmoji.SetActive(true);
        animator.SetBool("isDancing", true);

        while(this.transform.position.x > -10.5f)
        {
            this.transform.position += Vector3.left * Time.deltaTime;

            yield return new WaitForSeconds(math.EPSILON);
        }

        this.gameObject.SetActive(false);
    }

    public IEnumerator AngryOut()
    {
        stopTimer = true;
        isAngry = true;
        emotionTimer.SetActive(false);

        PenguinSpawner.AngryPenguinCnt++;
        angryEmoji.SetActive(true);
        animator.SetBool("isWalking", true);

        while(this.transform.position.x > -10.5f)
        {
            this.transform.position += Vector3.left * Time.deltaTime;

            yield return new WaitForSeconds(math.EPSILON);
        }

        this.gameObject.SetActive(false);
    }

    private void ResetTimer()
    {
        penguinMaxTime = 16f;
        currentTime = penguinMaxTime;
        slider.maxValue = currentTime;
        stopTimer = false;
    }

    private IEnumerator Timer()
    {
        while (!stopTimer)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForSeconds(math.EPSILON);

            if (currentTime <= penguinMaxTime * 0.2f)
            {
                timerHandle.sprite = emotions[2];
                slider.fillRect.GetComponent<Image>().color = Color.red;
            }
            else if (currentTime <= penguinMaxTime * 0.5f)
            {
                timerHandle.sprite = emotions[1];
                slider.fillRect.GetComponent<Image>().color = Color.yellow;
            }

            if (currentTime <= 0)
            {
                stopTimer = true;
            }

            if (!stopTimer)
            {
                slider.value = currentTime;
            }
        }

        if (stopTimer && currentTime <= 0)
        {
            Debug.Log("Penguin Angry!");
            StartCoroutine(AngryOut());
        }
    }
}
