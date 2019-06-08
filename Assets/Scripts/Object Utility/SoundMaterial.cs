////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class SoundMaterial : MonoBehaviour
{
    // HINT: You can identify your material here, so you can play the appropiate footstep sound

    // Sandra
    public enum SoundMaterialType
    {
        none = 0,
        dirt = 1,
        grass = 2,
        leaves = 3,
        rubble = 4,
        sand = 5,
        stone = 6,
        wood = 7,
        water = 8
    };
    public int material = 0;
}