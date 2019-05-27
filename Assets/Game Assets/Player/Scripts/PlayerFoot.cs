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
    #endregion

    // Sandra
    private const uint numAudios = 6;

    /// DIRT
    private AudioClip[] dirtRunSound;
    private AudioClip[] dirtWalkSound;

    /// GRASS
    private AudioClip[] grassRunSound;
    private AudioClip[] grassWalkSound;

    /// RUBBLE
    private AudioClip[] rubbleRunSound;
    //private AudioClip rubbleWalk1Sound; does not exist
    private AudioClip[] rubbleWalkSound;

    /// SAND
    private AudioClip[] sandRunSound;
    private AudioClip[] sandWalkSound;

    /// STONE
    private AudioClip[] stoneRunSound;
    private AudioClip[] stoneWalkSound;

    /// WATER
    private AudioClip[] waterRunSound;
    private AudioClip[] waterWalkSound;

    /// WOOD
    private AudioClip[] woodRunSound;
    private AudioClip[] woodWalkSound;

    private void Awake()
    {
        // Sandra
        string basePath = "Assets/Audio Assets/SFX/Character/Footsteps/";
        string name = "BAS_Player_Footstep_";
        string extension = ".wav";

        string terrain = null;
        string movement = null;
        string number = "0";

        /// DIRT
        terrain = "dirt_";
        dirtRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            dirtRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        dirtWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            dirtWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// GRASS
        terrain = "grass_";
        grassRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            grassRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        grassWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            grassWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// RUBBLE
        terrain = "rubble_";
        rubbleRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            rubbleRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        rubbleWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            rubbleWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// SAND
        terrain = "sand_";
        sandRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            sandRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        sandWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            sandWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// STONE
        terrain = "stone_";
        stoneRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            stoneRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        stoneWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            stoneWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// WATER
        terrain = "water_";
        waterRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            waterRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        waterWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            waterWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }

        /// WOOD
        terrain = "wood_";
        woodRunSound = new AudioClip[numAudios];
        movement = "run_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            woodRunSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
        woodWalkSound = new AudioClip[numAudios];
        movement = "walk_";
        for (uint i = 0; i < numAudios; ++i)
        {
            string path = basePath + name + terrain + movement + number + i + extension;
            woodWalkSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
    }

    public void PlayFootstepSound()
    {
        if (!inWater)
        {
            materialChecker.CheckMaterial(gameObject); //This also sets the material if a SoundMaterial is found!
        }

        // HINT: Play footstep sound here
        SoundMaterial.SoundMaterialType materialType = (SoundMaterial.SoundMaterialType)materialChecker.GetMaterial();
        switch (materialType)
        {
            case SoundMaterial.SoundMaterialType.dirt:

                break;

            case SoundMaterial.SoundMaterialType.grass:

                break;

            case SoundMaterial.SoundMaterialType.rubble:

                break;

            case SoundMaterial.SoundMaterialType.sand:

                break;

            case SoundMaterial.SoundMaterialType.stone:

                break;

            case SoundMaterial.SoundMaterialType.wood:

                break;
        }
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
