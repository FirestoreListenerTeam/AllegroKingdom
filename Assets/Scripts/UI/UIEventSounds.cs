////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventSounds : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource btn_fx;
    public AudioClip hover_fx;
    public AudioClip open_fx;
    public AudioClip enter_fx;
    public AudioClip close_fx;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Play OnPointerDownSound sound
        btn_fx.PlayOneShot(enter_fx);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play OOnPointerEnterSound sound
        btn_fx.PlayOneShot(hover_fx);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Play OOnPointerExitSound sound
        btn_fx.PlayOneShot(close_fx);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Play OOnPointerUpSound sound
        btn_fx.PlayOneShot(close_fx);
    }
}
