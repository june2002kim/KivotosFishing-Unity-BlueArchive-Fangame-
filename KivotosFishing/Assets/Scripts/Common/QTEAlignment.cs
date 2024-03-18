using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QTEAlignment : MonoBehaviour
{
    [SerializeField] public GameObject QTEKeyPrefab;
    [SerializeField] public List<QTEKey> QTEKeys;

    GridLayoutGroup gridLayoutGroup;

    // Start is called before the first frame update
    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    public void gridAlignment(int keyCount)
    {
        for(int i = 0; i < keyCount; i++)
        {
            var newKey = Instantiate(QTEKeyPrefab, this.transform).GetComponent<QTEKey>();
            QTEKeys.Add(newKey);
        }
    }

    public void gridReset()
    {
        QTEKeys.Clear();
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
