using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBDAlignment : MonoBehaviour
{
    [SerializeField] private GameObject DBDcakePrefab;
    [SerializeField] private GameObject DBDforeground;

    public void dbdAlignment(int cakeCount)
    {
        for(int i = 0; i < cakeCount; i++)
        {
            float cakeRot = (360 / cakeCount - 20) * i + UnityEngine.Random.Range(0, 17);
            var newCake = GameObject.Instantiate(DBDcakePrefab, this.transform);
            newCake.transform.rotation = Quaternion.Euler(0, 0, cakeRot);
        }

        var lastCake = GameObject.Instantiate(DBDforeground, this.transform);
    }

    public void dbdReset()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
