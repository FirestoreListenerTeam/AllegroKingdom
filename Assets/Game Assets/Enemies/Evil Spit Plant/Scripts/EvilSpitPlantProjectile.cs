////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EvilSpitPlantProjectile : MonoBehaviour
{
    [Header("Prefab Links")]
    public GameObject explodeParticles;
    public GameObject deflectParticles;

    [Header("Projectile Settings")]
    public float speed = 5;
    public float duration = 3;
    public float damage = 40f;

    public bool ignoreCollisionWithWwizard = false;

    [HideInInspector]
    public GameObject parent;

    #region private variables
    private Rigidbody rb;
    private float time = 0;
    private bool isExploding;
    private IEnumerator movementRoutine;

    // Sandra
    private AudioSource audioSource;

    private const uint numAudios = 3;

    private AudioClip[] chargeSound;
    private AudioClip[] deathSound;
    private AudioClip[] deathHeadfallSound;
    private AudioClip[] hurtSound;
    private AudioClip[] hurtImpactSound;
    private AudioClip[] impactPlayerSound;
    private AudioClip[] shootSound;
    private AudioClip shootLPSound;
    #endregion

    private void Awake()
    {
        // Sandra
        audioSource = GetComponent<AudioSource>();

        string basePath = "Assets/Audio Assets/SFX/Creatures/";
        string name = "BAS_Evil_SpitPlant_";
        string extension = ".wav";
        string number = "_0";

        string type = null;
        string path = null;

        chargeSound = new AudioClip[numAudios];
        type = "Charge";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            chargeSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (chargeSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        deathSound = new AudioClip[numAudios];
        type = "Death";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 4) + extension;
            deathSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (deathSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        deathHeadfallSound = new AudioClip[numAudios];
        type = "Death_Headfall";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            deathHeadfallSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (deathHeadfallSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        hurtSound = new AudioClip[numAudios * 2];
        type = "Hurt";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            hurtSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (hurtSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        hurtImpactSound = new AudioClip[numAudios];
        type = "HurtImpact";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            hurtImpactSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (hurtImpactSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        impactPlayerSound = new AudioClip[numAudios];
        type = "ImpactPlayer";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            impactPlayerSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (impactPlayerSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        shootSound = new AudioClip[numAudios];
        type = "Shoot";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            shootSound[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (shootSound[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        path = basePath + name + "Shoot_LP" + number + 1 + extension;
        shootLPSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (shootLPSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        PlayerCamera.OnCameraEventStart += ForceExplode;

        movementRoutine = MoveSpitBullet();
        StartCoroutine(movementRoutine);
    }

    private void OnDisable()
    {
        PlayerCamera.OnCameraEventStart -= ForceExplode;
    }

    IEnumerator MoveSpitBullet()
    {
        // HINT: Spit bullet started moving, you might want to start playing its continuous sound here
        while (time < duration)
        {
            rb.velocity = transform.forward * speed;
            time += Time.deltaTime;
            yield return null;
        }

        if (!isExploding)
        {
            //SpitBullet explodes because it didn't hit something within the set duration.
            Explode(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject != parent && !col.isTrigger && (ignoreCollisionWithWwizard ? !col.CompareTag("Wwizard") : true))
        {
            //If player deflects the spit ball
            if (col.gameObject.CompareTag("Player") || col.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                //Only deflect if the player is somewhat facing the spitbullet
                bool allowToReflect = Vector3.Dot(col.transform.forward, rb.velocity.normalized) < 0;

                if (allowToReflect && PlayerManager.Instance.isDashing)
                {
                    Deflect(col.gameObject);
                    return; // don't explode yet!
                }
            }

            Explode();

            Vector3 dir = (transform.position - parent.transform.position).normalized;
            GameManager.DamageObject(col.gameObject, new Attack(damage, dir, 5f, SwingTypes.Top, WeaponTypes.EvilSpitPlant));
        }
    }

    void Deflect(GameObject deflector)
    {
        //Reset timer
        time = 0;

        //flip the rotation, effectively sending the spitbullet right back to its shooter
        transform.rotation = Quaternion.LookRotation(-rb.velocity);

        GameObject deflect = Instantiate(deflectParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(deflect, 5f);

        //make sure the weapon doesn't accidentally explode the bullet just after deflecting
        if (deflector.layer == LayerMask.NameToLayer("Weapon"))
        {
            parent = deflector;
        }
        else
        {
            parent = PlayerManager.Instance.equippedWeapon;
        }
    }

    private void ForceExplode()
    {
        Explode(false);
    }

    void Explode(bool hitSomething = true)
    {
        if (!isExploding)
        {
            isExploding = true;

            // HINT: Spit bullet stopped, you might want to stop playing its continuous sound here

            GetComponent<Collider>().enabled = false;
            time = duration;
            rb.velocity = Vector3.zero;

            //Spawn explodeParticles
            GameObject go = Instantiate(explodeParticles, transform.position, Quaternion.identity) as GameObject; //TODO: Pool explode particles

            if (hitSomething)
            {
                // HINT: Explosion did hit something, you may want to play the explosion hit sound here
            }
            else
            {
                // HINT: Explosion didn't hit something, you may want to play the explosion miss sound here
            }

            Destroy(go, 5f);

            //Stop all currently active particles
            foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            Destroy(gameObject, 5f);
        }
    }
}
