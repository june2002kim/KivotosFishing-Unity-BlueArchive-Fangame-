using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class swimmingHanako : MonoBehaviour
{
    [SerializeField] private Transform goalPosition;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isSwimming", true);

        StartCoroutine(Swim());
    }

    private IEnumerator Swim()
    {
        while(this.gameObject.transform.position.x > goalPosition.position.x)
        {
            this.gameObject.transform.position += Vector3.left * Time.deltaTime;
            yield return new WaitForSeconds(math.EPSILON);
        }
    }
}
