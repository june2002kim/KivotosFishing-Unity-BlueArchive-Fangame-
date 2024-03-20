using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class PenguinSpawner : MonoBehaviour
{
    [SerializeField] private FisingManager fishingManager;
    [SerializeField] private IceManager iceManager;
    [SerializeField] private GameObject penguinPrefab;
    [SerializeField] private GameObject peroroPrefab;
    [SerializeField] private GameObject amy;
    [SerializeField] public List<GameObject> penguins = new List<GameObject>();
    [SerializeField] public int penguinIdx = 0;
    [SerializeField] private GameObject[] Lifes;
    public bool isPenguin = false;
    [SerializeField] private int MaxPenguinCnt;
    public static int HappyPenguinCnt = 0;
    public static int AngryPenguinCnt = 0;
    private bool penguinHulk = false;

    void Start()
    {
        SetVariables();
        SpawnPenguin();
    }

    void Update()
    {
        if (AngryPenguinCnt > 0 && AngryPenguinCnt <= 3)
        {
            Lifes[AngryPenguinCnt - 1].SetActive(true);
        }

        if (AngryPenguinCnt == 3 && !penguinHulk)
        {
            penguinHulk = true;
            StartCoroutine(iceManager.ShowRetryPanel());
        }
    }

    private void SetVariables()
    {
        HappyPenguinCnt = 0;
        AngryPenguinCnt = 0;
    }

    public void SpawnPenguin()
    {
        if (HappyPenguinCnt < MaxPenguinCnt)
        {
            GameObject result;
            penguinIdx++;

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                // Spawn Penguin
                result = Instantiate(penguinPrefab, this.transform.position, Quaternion.identity);
                isPenguin = true;
            }
            else
            {
                // Spawn Peroro
                result = Instantiate(peroroPrefab, this.transform.position, Quaternion.identity);
                isPenguin = false;
            }

            penguins.Add(result);
        }
        else
        {
            // Spawn Amy
            fishingManager.shirokoPhase = fishingPhase.BLOCKCASTING;

            amy.SetActive(true);
        }

    }
}
