using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amySwimming : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("isSwimming", true);
    }
}
