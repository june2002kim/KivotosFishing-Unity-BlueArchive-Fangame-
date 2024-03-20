using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [SerializeField] LakeManager lakeManager;
    [SerializeField] AmazonManager amazonManager;
    [SerializeField] FinManager finManager;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lake")
        {
            if (lakeManager.speedAudioUp && audioSource.pitch < 2f)
            {
                audioSource.pitch = 1.25f;
            }
            else if (!lakeManager.speedAudioUp && audioSource.pitch < 0f)
            {
                audioSource.pitch = 1.0f;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Amazon")
        {
            if (amazonManager.speedAudioUp && audioSource.pitch < 2f)
            {
                audioSource.pitch = 1.25f;
            }
            else if (!amazonManager.speedAudioUp && audioSource.pitch < 0f)
            {
                audioSource.pitch = 1.0f;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Fin")
        {
            if (finManager.speedAudioUp && audioSource.pitch < 2f)
            {
                audioSource.pitch = 1.25f;
            }
            else if (!finManager.speedAudioUp && audioSource.pitch < 0f)
            {
                audioSource.pitch = 1.0f;
            }
        }
    }
}
