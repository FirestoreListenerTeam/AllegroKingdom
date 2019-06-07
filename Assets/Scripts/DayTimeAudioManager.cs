using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> timeClipList = new List<AudioClip>();


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = timeClipList[0];
    }
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource)
        {
            Debug.Log(GameManager.TimeOfDay);

            if (GameManager.TimeOfDay >= 6.0f && GameManager.TimeOfDay <= 18.0f && 
                audioSource.clip != timeClipList[0])
            {
                audioSource.clip = timeClipList[0];
                audioSource.Play();
            }
            else if (((GameManager.TimeOfDay >= 0.0f && GameManager.TimeOfDay < 6.0f) ||
                      (GameManager.TimeOfDay > 18.0f && GameManager.TimeOfDay <= 24.0f))
                      && audioSource.clip != timeClipList[1])
            {
                audioSource.clip = timeClipList[1];
                audioSource.Play();
            }
                
        }
    }
}