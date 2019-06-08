////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerFoot : MonoBehaviour
{
    public MaterialChecker materialChecker;

    #region private variables
    private bool inWater;

    // Sandra
    private const uint numAudios = 6;

    /// Dirt
    private AudioClip[] dirtRunSound;
    private AudioClip[] dirtWalkSound;

    /// Grass
    private AudioClip[] grassRunSound;
    private AudioClip[] grassWalkSound;

    /// Rubble
    private AudioClip[] rubbleRunSound;
    private AudioClip[] rubbleWalkSound;

    /// Sand
    private AudioClip[] sandRunSound;
    private AudioClip[] sandWalkSound;

    /// Stone
    private AudioClip[] stoneRunSound;
    private AudioClip[] stoneWalkSound;

    /// Water
    private AudioClip[] waterRunSound;
    private AudioClip[] waterWalkSound;

    /// Wood
    private AudioClip[] woodRunSound;
    private AudioClip[] woodWalkSound;
    #endregion

    private void Awake()
    {
        // Sandra
        string basePath = "Assets/Audio Assets/SFX/Character/Footsteps/";
        string name = "BAS_Player_Footstep_";
        string extension = ".wav";

        string terrain = null;
        string movement = null;
        string number = "0";

        /// Dirt
        terrain = "dirt_";
        dirtRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            dirtRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        dirtWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            dirtWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Grass
        terrain = "grass_";
        grassRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            grassRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        grassWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            grassWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Rubble
        terrain = "rubble_";
        rubbleRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            rubbleRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        rubbleWalkSound = new AudioClip[numAudios - 1];
        movement = "walk_";
        for (uint i = 0; i < numAudios - 1; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 2) + extension;
            rubbleWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Sand
        terrain = "sand_";
        sandRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            sandRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        sandWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            sandWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Stone
        terrain = "stone_";
        stoneRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            stoneRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        stoneWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            stoneWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Water
        terrain = "water_";
        waterRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            waterRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        waterWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            waterWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// Wood
        terrain = "wood_";
        woodRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            woodRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        woodWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + (i + 1) + extension;
            woodWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
    }

    public AudioClip PlayFootstepSound()
    {
        bool isSprinting = PlayerManager.Instance.isSprinting;

        if (!inWater)
        {
            materialChecker.CheckMaterial(gameObject); //This also sets the material if a SoundMaterial is found!
        }
        else
        {
            // Sandra
            int randomNumber = Random.Range(0, (int)numAudios);
            return isSprinting ? waterRunSound[randomNumber] : waterWalkSound[randomNumber];
        }

        // HINT: Play footstep sound here

        // Sandra
        SoundMaterial.SoundMaterialType materialType = (SoundMaterial.SoundMaterialType)materialChecker.GetMaterial();
        switch (materialType)
        {
            case SoundMaterial.SoundMaterialType.dirt:
                {
                    int randomNumber = Random.Range(0, (int)numAudios);
                    return isSprinting ? dirtRunSound[randomNumber] : dirtWalkSound[randomNumber];
                }

            case SoundMaterial.SoundMaterialType.grass:
                {
                    int randomNumber = Random.Range(0, (int)numAudios);
                    return isSprinting ? grassRunSound[randomNumber] : grassWalkSound[randomNumber];
                }

            case SoundMaterial.SoundMaterialType.rubble:
                {
                    if (isSprinting)
                    {
                        int randomNumber = Random.Range(0, (int)numAudios);
                        return rubbleRunSound[randomNumber];
                    }
                    else
                    {
                        int randomNumber = Random.Range(0, (int)numAudios - 1);
                        return rubbleWalkSound[randomNumber];
                    }
                }

            case SoundMaterial.SoundMaterialType.sand:
                {
                    int randomNumber = Random.Range(0, (int)numAudios);
                    return isSprinting ? sandRunSound[randomNumber] : sandWalkSound[randomNumber];
                }

            case SoundMaterial.SoundMaterialType.stone:
                {
                    int randomNumber = Random.Range(0, (int)numAudios);
                    return isSprinting ? stoneRunSound[randomNumber] : stoneWalkSound[randomNumber];
                }

            case SoundMaterial.SoundMaterialType.wood:
                {
                    int randomNumber = Random.Range(0, (int)numAudios);
                    return isSprinting ? woodRunSound[randomNumber] : woodWalkSound[randomNumber];
                }
        }

        return null;
    }

    public void EnterWaterZone()
    {
        inWater = true;
    }

    public void ExitWaterZone()
    {
        inWater = false;
    }
}