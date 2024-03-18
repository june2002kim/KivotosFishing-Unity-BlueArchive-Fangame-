using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBDcake : MonoBehaviour
{
    [SerializeField] private DBDManager dbdManager;
    [SerializeField] public bool isIn;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isIn = true;
    }

    private void OnTriggerExit2D()
    {
        isIn = false;
    }

    void Awake()
    {
        dbdManager = GameObject.FindObjectOfType<DBDManager>().GetComponent<DBDManager>();
    }

    void Update()
    {
        if(isIn && dbdManager.isIn)
        {
            dbdManager.isIn = false;
            Destroy(gameObject);
        }
    }
}
