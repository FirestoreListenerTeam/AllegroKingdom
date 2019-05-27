////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class SoundMaterial : MonoBehaviour
{
    // Sandra
    public enum SoundMaterialType { dirt, grass, rubble, sand, stone, wood, water };

    // HINT: You can identify your material here, so you can play the appropiate footstep sound
    public int material;
}