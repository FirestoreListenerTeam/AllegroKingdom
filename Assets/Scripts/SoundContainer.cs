using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundContainer : System.Object
{
	public AudioSource audioSource;
	public List<AudioClip> waterDropsList = new List<AudioClip>();

	private float timeElapsed;
	private float timer = 0.0f;

	public float minRangeTime = 0.2f;
	public float maxRangeTime = 0.4f;

	public void Start()
	{
		PlayRandomSoundOnce();
		timeElapsed = Random.Range(minRangeTime, maxRangeTime);
	}

	public void UpdateTick()
	{
		if (!audioSource.isPlaying && timer >= timeElapsed) 
		{
			PlayRandomSoundOnce();
			float tmp = timer - timeElapsed;
			timer = 0.0f + tmp;
			timeElapsed = Random.Range(minRangeTime, maxRangeTime);
		}
		else
			timer += Time.deltaTime;
	}

	public void PlayRandomSoundOnce()
	{
		int randomWaterDrop = Random.Range(0, waterDropsList.Count);
		audioSource.PlayOneShot(waterDropsList[randomWaterDrop]);
	}
}