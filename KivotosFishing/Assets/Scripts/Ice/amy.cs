using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class amy : MonoBehaviour
{
    [SerializeField] private Transform standbyPos;
    [SerializeField] private IceManager iceManager;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(WalkIn());
    }

    private IEnumerator WalkIn()
    {
        while(this.transform.position.x > standbyPos.position.x)
        {
            this.transform.position += Vector3.left * Time.deltaTime;

            yield return new WaitForSeconds(math.EPSILON);
        }

        audioSource.Play();

        yield return new WaitForSeconds(2f);

        StartCoroutine(iceManager.EndIce());
    }
}
