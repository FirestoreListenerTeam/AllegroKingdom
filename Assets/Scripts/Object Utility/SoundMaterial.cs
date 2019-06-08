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
    public enum SoundMaterialType { none, dirt, grass, leaves, rubble, sand, stone, wood, water };
    public SoundMaterialType soundMaterialType = SoundMaterialType.none;
}