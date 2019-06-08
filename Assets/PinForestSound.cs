using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinForestSound : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> clipList = new List<AudioClip>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = clipList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource)
        {
            //Debug.Log(GameManager.TimeOfDay);

            if (GameManager.TimeOfDay >= 6.0f && GameManager.TimeOfDay <= 18.0f &&
                audioSource.clip != clipList[0])
            {
                audioSource.clip = clipList[0];
                audioSource.Play();
            }
            else if (((GameManager.TimeOfDay >= 0.0f && GameManager.TimeOfDay < 6.0f) ||
                      (GameManager.TimeOfDay > 18.0f && GameManager.TimeOfDay <= 24.0f))
                      && audioSource.clip != clipList[1])
            {
                audioSource.clip = clipList[1];
                audioSource.Play();
            }

        }
    }
}
