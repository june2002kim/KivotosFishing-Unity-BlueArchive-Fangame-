using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ioriSpawner : MonoBehaviour
{
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private GameObject runningIori;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Animator ioriAnimator;


    // Start is called before the first frame update
    void Start()
    {
        runningIori.transform.position = leftSpawnPoint.position;
        ioriAnimator.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFacingRight)
        {
            runningIori.transform.position += Vector3.right * Time.deltaTime * 2;

            if(runningIori.gameObject.transform.position.x > rightSpawnPoint.position.x)
            {
                isFacingRight = false;
                runningIori.transform.localScale = new Vector3(-1, 1, 0);
            }
        }
        else
        {
            runningIori.transform.position += Vector3.left * Time.deltaTime * 2;

            if(runningIori.gameObject.transform.position.x < leftSpawnPoint.position.x)
            {
                isFacingRight = true;
                runningIori.transform.localScale = new Vector3(1, 1, 0);
            }
        }
    }
}
