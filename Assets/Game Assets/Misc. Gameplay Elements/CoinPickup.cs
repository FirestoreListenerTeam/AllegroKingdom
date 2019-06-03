////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CoinPickup : MonoBehaviour {

    public bool playSpawnSoundAtSpawn = true;

    // Sandra
    private AudioSource audioSource;

    private const uint numAudios = 4;

    private AudioClip[] pickupCoinSounds;

    private void Awake()
    {
        // Sandra
        audioSource = GetComponent<AudioSource>();

        pickupCoinSounds = new AudioClip[numAudios];
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = "Assets/Audio Assets/SFX/Objects/Pickups/BAS_Pickup_Coin_0" + (i + 1) + ".wav";
            pickupCoinSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (pickupCoinSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }
    }

    void Start(){
        if (playSpawnSoundAtSpawn){
            // HINT: You might want to play the Coin pickup sound here
            int randomNumber = Random.Range(0, (int)numAudios);
            audioSource.PlayOneShot(pickupCoinSounds[randomNumber]);
        }
	}

	public void AddCoinToCoinHandler(){
		InteractionManager.SetCanInteract(this.gameObject, false);
		GameManager.Instance.coinHandler.AddCoin ();
		//Destroy (gameObject, 0.1f); //TODO: Pool instead?
	}
}
