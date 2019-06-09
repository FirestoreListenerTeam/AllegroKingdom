////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEditor;

public class EvilSpitPlantAI : Creature
{
    [Header("Custom Creature Options:")]
    public GameObject bulletPrefab;
    public GameObject chargeParticles;
    public GameObject shootParticles;
    public GameObject spitBulletSpawnPoint;

    #region private variables
    private bool hasSpawned = false;
    private bool lockRotation = false;
    private readonly int spawnHash = Animator.StringToHash("Spawn");
    private readonly int despawnHash = Animator.StringToHash("Despawn");
    private readonly int isAliveHash = Animator.StringToHash("IsAlive");

    // Sandra
    private AudioSource audioSource;

    private const uint numAudios = 3;

    private AudioClip[] chargeSounds;
    private AudioClip[] deathSounds;
    private AudioClip[] deathHeadfallSounds;
    private AudioClip[] hurtSounds;
    private AudioClip[] hurt2Sounds;
    private AudioClip[] shootSounds;
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

        chargeSounds = new AudioClip[numAudios];
        type = "Charge";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            chargeSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (chargeSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        deathSounds = new AudioClip[numAudios];
        type = "Death";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 4) + extension;
            deathSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (deathSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        deathHeadfallSounds = new AudioClip[numAudios];
        type = "Death_Headfall";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            deathHeadfallSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (deathHeadfallSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        hurtSounds = new AudioClip[numAudios];
        hurt2Sounds = new AudioClip[numAudios];
        type = "Hurt";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            hurtSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (hurtSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);

            path = basePath + name + type + number + (i + 4) + extension;
            hurt2Sounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (hurt2Sounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        shootSounds = new AudioClip[numAudios];
        type = "Shoot";
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + type + number + (i + 1) + extension;
            shootSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (shootSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }
    }

    public override void OnSpotting()
    {
        base.OnSpotting();

        if (!hasSpawned)
        {
            anim.SetTrigger(spawnHash);
            hasSpawned = true;
        }
    }

    public override void OffSpotting()
    {
        base.OffSpotting();

        if (hasSpawned)
        {
            anim.SetTrigger(despawnHash);
            hasSpawned = false;
        }
    }

    /// <summary>
    /// Called from Animation Event. This shoots the projectile!
    /// </summary>
    public void Shoot()
    {
        if (targetOfNPC != null && !GameManager.Instance.AIPaused)
        {
            // HINT: Plant will launch a bullet, time to play the spit sound here

            // Sandra
            int randomNumber = Random.Range(0, (int)numAudios);
            audioSource.PlayOneShot(shootSounds[randomNumber]);

            GameObject bullet = Instantiate(bulletPrefab, spitBulletSpawnPoint.transform.position, Quaternion.LookRotation(transform.forward)) as GameObject; //TODO: Pool spitbullets
            bullet.GetComponent<EvilSpitPlantProjectile>().parent = gameObject;
            bullet.GetComponent<EvilSpitPlantProjectile>().damage = this.AttackDamage;

            GameObject bulletSpawnFX = Instantiate(shootParticles, spitBulletSpawnPoint.transform.position, Quaternion.identity, spitBulletSpawnPoint.transform) as GameObject; //TODO: Pool spitbullet spawn particles
            Destroy(bulletSpawnFX, 5f);
        }
    }

    public void PlayChargeSound()
    {
        // HINT: Plant is charging a bullet, time to play the spit charging sound here

        // Sandra
        int randomNumber = Random.Range(0, (int)numAudios);
        audioSource.PlayOneShot(chargeSounds[randomNumber]);
    }

    /// <summary>
    /// Called from Animation Event. This happens when the Evil Spit Plant telegraphs its attack!
    /// </summary>
    public void ChargeUp()
    {
        if (chargeParticles != null)
        {
            GameObject chargeFX = Instantiate(chargeParticles, spitBulletSpawnPoint.transform.position, Quaternion.identity, spitBulletSpawnPoint.transform) as GameObject; //TODO: Pool charge particles.
            Destroy(chargeFX, 5f);
        }
    }

    public override void Move(Vector3 yourPosition, Vector3 targetPosition)
    {
        if (!lockRotation)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetOfNPC.transform.position - transform.position);
            RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * NoNavRotationSpeed);
        }
    }

    public override void OnDamageReset()
    {
        base.OnDamageReset();
        lockRotation = false;

        // Sandra
        int randomNumber = Random.Range(0, (int)numAudios);
        audioSource.PlayOneShot(hurtSounds[randomNumber]);
        randomNumber = Random.Range(0, (int)numAudios);
        audioSource.PlayOneShot(hurt2Sounds[randomNumber]);
    }

    // Sandra
    protected override void PlayCreatureDeathSound()
    {
        base.PlayCreatureDeathSound();

        AudioSource playerAudioSource = PlayerManager.Instance.playerAudioSource;
        if (playerAudioSource != null)
        {
            int randomNumber = Random.Range(0, (int)numAudios);
            playerAudioSource.PlayOneShot(deathSounds[randomNumber]);
        }
    }

    /// <summary>
    /// Called from Animation Event.
    /// </summary>
    public void LockRotation()
    {
        lockRotation = true;
    }

    /// <summary>
    /// Called from Animation Event. This happens when the Evil Head telegraphs its attack!
    /// </summary>
    public void UnlockRotation()
    {
        lockRotation = false;
    }

    public override void OnDeathAnimation()
    {
        base.OnDeathAnimation();

        anim.SetBool(isAliveHash, false);

        float angle = Vector3.Angle(RotationObject.transform.forward, LastAttack.attackDir);
        if (Mathf.Abs(angle) > 90)
        {
            anim.SetTrigger(DeathAnimations.FrontTrigger);
        }
        else
        {
            anim.SetTrigger(DeathAnimations.BehindTrigger);
        }
        LockRotation();
    }

    public void OnDeathHeadFall()
    {
        // HINT: Plant is dead, you might want to play the death head fall sound here

        // Sandra
        AudioSource playerAudioSource = PlayerManager.Instance.playerAudioSource;
        if (playerAudioSource != null)
        {
            int randomNumber = Random.Range(0, (int)numAudios);
            audioSource.PlayOneShot(deathHeadfallSounds[randomNumber]);
        }
    }
}