////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class AdventuressAnimationEventHandler : MonoBehaviour
{
    public AudioClip leftFootStep;
    public AudioClip rightFootStep;

    [Header("Object Links")]
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private GameObject runParticles;

    private PlayerFoot foot_L;
    private PlayerFoot foot_R;

    #region private variables
    private bool hasPausedMovement;
    private readonly int canShootMagicHash = Animator.StringToHash("CanShootMagic");
    private readonly int isAttackingHash = Animator.StringToHash("IsAttacking");

    // Sandra
    private const uint numAudios = 3;

    /// WEAPON
    private AudioClip[] swingSounds;

    /// Axe
    private AudioClip impAxeDirtSound;
    private AudioClip impAxeGrassSound;
    private AudioClip impAxeLeavesSound;
    private AudioClip impAxeStoneSound;

    /// Dagger
    private AudioClip impDaggerDirtSound;
    private AudioClip impDaggerGrassSound;
    private AudioClip impDaggerLeavesSound;
    private AudioClip impDaggerStoneSound;
    private AudioClip impDaggerWoodSound;

    /// Sword
    private AudioClip impSwordDirtSound;
    private AudioClip impSwordGrassSound;
    private AudioClip impSwordLeavesSound;
    private AudioClip impSwordStoneSound;
    private AudioClip impSwordWoodSound;

    /// PICKUP
    private AudioClip pickupGenericSound;
    private AudioClip pickupBookSound;
    private AudioClip[] pickupCrystalsSounds;
    private AudioClip pickupEvilEssenceSound;
    private AudioClip pickupKeySound;
    private AudioClip pickupMushroomSound;
    private AudioClip pickupPineConeSound;
    private AudioClip pickupAxeSound;
    private AudioClip pickupDaggerSound;
    private AudioClip pickupHammerSound;
    private AudioClip pickupPickaxeSound;
    private AudioClip pickupSwordSound;
    private AudioClip[] pickupCoinSounds;
    private AudioClip pickupWeaponSound;
    #endregion

    private void Awake()
    {
        GameObject L = GameObject.Find("toe_left");
        GameObject R = GameObject.Find("toe_right");
        if (L != null)
        {
            foot_L = L.GetComponent<PlayerFoot>();
        }
        else {
            print("Left foot missing");
        }
        if (R != null)
        {
            foot_R = R.GetComponent<PlayerFoot>();
        }
        else
        {
            print("Right foot missing");
        }

        // Sandra

        /// WEAPON
        string basePath = "Assets/Audio Assets/SFX/Character/Weapons/";
        string name = "BAS_imp_";
        string extension = ".wav";

        string path = null;

        swingSounds = new AudioClip[numAudios];
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + "Swing/" + "Builder_Game_Weapon_Heavy_Swing_" + (i + 1) + extension;
            swingSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (swingSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        string weaponType = null;
        string dirtMaterial = "dirt";
        string grassMaterial = "grass";
        string leavesMaterial = "leaves";
        string stoneMaterial = "stone";
        string woodMaterial = "wood";

        /// Axe
        weaponType = "axe_";

        path = basePath + name + weaponType + dirtMaterial + extension;
        impAxeDirtSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impAxeDirtSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + grassMaterial + extension;
        impAxeGrassSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impAxeGrassSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + leavesMaterial + "_2" + extension;
        impAxeLeavesSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impAxeLeavesSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + stoneMaterial + "_2" + extension;
        impAxeStoneSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impAxeStoneSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Dagger
        weaponType = "dagger_";

        path = basePath + name + weaponType + dirtMaterial + extension;
        impDaggerDirtSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impDaggerDirtSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + grassMaterial + "_2" + extension;
        impDaggerGrassSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impDaggerGrassSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + leavesMaterial + extension;
        impDaggerLeavesSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impDaggerLeavesSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + stoneMaterial + "_2" + extension;
        impDaggerStoneSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impDaggerStoneSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + woodMaterial + extension;
        impDaggerWoodSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impDaggerWoodSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Sword
        weaponType = "sword_";

        path = basePath + name + weaponType + dirtMaterial + extension;
        impSwordDirtSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impSwordDirtSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + grassMaterial + extension;
        impSwordGrassSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impSwordGrassSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + leavesMaterial + extension;
        impSwordLeavesSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impSwordLeavesSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + stoneMaterial + "_2" + extension;
        impSwordStoneSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impSwordStoneSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
        path = basePath + name + weaponType + woodMaterial + "_2" + extension;
        impSwordWoodSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (impSwordWoodSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// PICKUP
        basePath = "Assets/Audio Assets/SFX/Objects/Pickups/";
        name = "BAS_Pickup_";
        string number = "_0";

        /// Generic
        path = basePath + name + "Generic" + number + "5" + extension;
        pickupGenericSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupGenericSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Book
        path = basePath + name + "Book" + number + "1" + extension;
        pickupBookSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupBookSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Crystals
        pickupCrystalsSounds = new AudioClip[numAudios];
        for (uint i = 0; i < numAudios; ++i)
        {
            path = basePath + name + "Crystals" + number + (i + 1) + extension;
            pickupCrystalsSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (pickupCrystalsSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        /// Evil essence
        path = basePath + name + "EvilEssence" + extension;
        pickupEvilEssenceSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupEvilEssenceSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Key
        path = basePath + name + "Key" + number + "1" + extension;
        pickupKeySound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupKeySound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Mushroom
        path = basePath + name + "Mushroom" + number + "1" + extension;
        pickupMushroomSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupMushroomSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Pine cone
        path = basePath + name + "PineCone" + number + "1" + extension;
        pickupPineConeSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupPineConeSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Axe
        path = basePath + name + "WeaponType_" + "Axe" + number + "1" + extension;
        pickupAxeSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupAxeSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Dagger
        path = basePath + name + "WeaponType_" + "Dagger" + extension;
        pickupDaggerSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupDaggerSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Hammer
        path = basePath + name + "WeaponType_" + "Hammer" + number + "1" + extension;
        pickupHammerSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupHammerSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Pickaxe
        path = basePath + name + "WeaponType_" + "Pickaxe" + number + "1" + extension;
        pickupPickaxeSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupPickaxeSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Sword
        path = basePath + name + "WeaponType_" + "Sword" + number + "1" + extension;
        pickupSwordSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupSwordSound == null)
            Debug.LogError("Invalid audio clip path: " + path);

        /// Coin
        pickupCoinSounds = new AudioClip[numAudios + 1];
        for (uint i = 0; i < numAudios + 1; ++i)
        {
            path = basePath + name + "Coin" + number + (i + 1) + extension;
            pickupCoinSounds[i] = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            if (pickupCoinSounds[i] == null)
                Debug.LogError("Invalid audio clip path: " + path);
        }

        /// Weapon
        path = "Assets/Audio Assets/SFX/Character/Weapons/BAS_pickUpWeapon_001" + extension;
        pickupWeaponSound = (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        if (pickupWeaponSound == null)
            Debug.LogError("Invalid audio clip path: " + path);
    }


    void enableWeaponCollider()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.equippedWeaponInfo != null)
        {
            PlayerManager.Instance.equippedWeaponInfo.EnableHitbox();
        }
    }

    void disableWeaponCollider()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.equippedWeaponInfo != null)
        {
            PlayerManager.Instance.equippedWeaponInfo.DisableHitbox();
        }

    }

    void ScreenShake()
    {
        PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(0.4f, 0.7f));
    }

    bool onCooldown = false;
    public enum FootSide { left, right };
    public void TakeFootstep(FootSide side)
    {
        if (foot_L != null && foot_R != null) {
            if (!PlayerManager.Instance.inAir && !onCooldown)
            {
                Vector3 particlePosition;

                if (side == FootSide.left )
                {
                    //if (foot_L.FootstepSound.Validate())
                    {
                        // HINT: Play left footstep sound

                        // Sandra
                        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                        if (playerMovement != null)
                            leftFootStep = foot_L.PlayFootstepSound();

                        particlePosition = foot_L.transform.position;
                        FootstepParticles(particlePosition);
                        AudioSource audioSource = GetComponent<AudioSource>();
                        audioSource.PlayOneShot(leftFootStep, 0.7F);
                    }
                }
                else
                {
                    //if (foot_R.FootstepSound.Validate())
                    {
                        // HINT: Play right footstep sound

                        // Sandra
                        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                        if (playerMovement != null)
                            rightFootStep = foot_R.PlayFootstepSound();

                        particlePosition = foot_R.transform.position;
                        FootstepParticles(particlePosition);
                        AudioSource audioSource = GetComponent<AudioSource>();
                        audioSource.PlayOneShot(rightFootStep, 0.7F);
                    }
                }
            }
        }
    }

    void FootstepParticles(Vector3 particlePosition) {
        GameObject p = Instantiate(runParticles, particlePosition + Vector3.up * 0.1f, Quaternion.identity) as GameObject;
        p.transform.parent = SceneStructure.Instance.TemporaryInstantiations.transform;
        Destroy(p, 5f);
        StartCoroutine(FootstepCooldown());
    }

    IEnumerator FootstepCooldown()
    {
        onCooldown = true;
        yield return new WaitForSecondsRealtime(0.2f);
        onCooldown = false;
    }

    void ReadyToShootMagic()
    {
        PlayerManager.Instance.playerAnimator.SetBool(canShootMagicHash, true);
    }

    public enum AttackState { NotAttacking, Attacking };
    void SetIsAttacking(AttackState state)
    {
        if (state == AttackState.NotAttacking)
        {
            playerAnimator.SetBool(isAttackingHash, false);
        }
        else
        {
            playerAnimator.SetBool(isAttackingHash, true);
        }
    }

    public void Weapon_SwingEvent(SwingTypes swingType)
    {
        // PLAY SOUND
        Weapon W = PlayerManager.Instance.equippedWeaponInfo;
        // HINT: PlayerManager.Instance.weaponSlot contains the selected weapon;
        // HINT: This is a good place to play the weapon swing sounds

        // Sandra
        AudioSource playerAudioSource = PlayerManager.Instance.playerAudioSource;
        if (playerAudioSource != null)
        {
            switch (swingType)
            {
                case SwingTypes.Right:
                    playerAudioSource.PlayOneShot(swingSounds[0]);
                    break;

                case SwingTypes.Left:
                    playerAudioSource.PlayOneShot(swingSounds[1]);
                    break;

                case SwingTypes.Top:
                    playerAudioSource.PlayOneShot(swingSounds[2]);
                    break;
            }
        }
    }

    public void PauseMovement()
    {
        if (!hasPausedMovement)
        {
            hasPausedMovement = true;
            PlayerManager.Instance.motor.SlowMovement();
        }
    }

    public void ResumeMovement()
    {
        if (hasPausedMovement)
        {
            hasPausedMovement = false;
            PlayerManager.Instance.motor.UnslowMovement();
        }
    }

    public void FreezeMotor()
    {
        StartCoroutine(PickupEvent());
    }

    private IEnumerator PickupEvent()
    {
        PlayerManager.Instance.PauseMovement(gameObject);
        yield return new WaitForSeconds(2f);
        PlayerManager.Instance.ResumeMovement(gameObject);
    }

    public void PickUpItem(Pickup.PickupType pickupType)
    {
        //PlayerManager.Instance.PickUpEvent();
        // HINT: This is a good place to play the Get item sound and stinger

        // Sandra
        AudioSource playerAudioSource = PlayerManager.Instance.playerAudioSource;
        if (playerAudioSource != null)
        {
            switch (pickupType)
            {
                case Pickup.PickupType.Book:
                    playerAudioSource.PlayOneShot(pickupBookSound);
                    break;

                case Pickup.PickupType.Crystals:
                    {
                        int randomNumber = Random.Range(0, (int)numAudios);
                        playerAudioSource.PlayOneShot(pickupCrystalsSounds[randomNumber]);
                    }
                    break;

                case Pickup.PickupType.EvilEssence:
                    playerAudioSource.PlayOneShot(pickupEvilEssenceSound);
                    break;

                case Pickup.PickupType.Key:
                    playerAudioSource.PlayOneShot(pickupKeySound);
                    break;

                case Pickup.PickupType.Mushroom:
                    playerAudioSource.PlayOneShot(pickupMushroomSound);
                    break;

                case Pickup.PickupType.Pinecone:
                    playerAudioSource.PlayOneShot(pickupPineConeSound);
                    break;

                case Pickup.PickupType.Axe:
                    playerAudioSource.PlayOneShot(pickupAxeSound);
                    playerAudioSource.PlayOneShot(pickupWeaponSound);
                    break;

                case Pickup.PickupType.Dagger:
                    playerAudioSource.PlayOneShot(pickupDaggerSound);
                    playerAudioSource.PlayOneShot(pickupWeaponSound);
                    break;

                case Pickup.PickupType.Hammer:
                    playerAudioSource.PlayOneShot(pickupHammerSound);
                    playerAudioSource.PlayOneShot(pickupWeaponSound);
                    break;

                case Pickup.PickupType.Pickaxe:
                    playerAudioSource.PlayOneShot(pickupPickaxeSound);
                    playerAudioSource.PlayOneShot(pickupWeaponSound);
                    break;

                case Pickup.PickupType.Sword:
                    playerAudioSource.PlayOneShot(pickupSwordSound);
                    playerAudioSource.PlayOneShot(pickupWeaponSound);
                    break;

                case Pickup.PickupType.Coin:
                    {
                        int randomNumber = Random.Range(0, (int)numAudios);
                        playerAudioSource.PlayOneShot(pickupCoinSounds[randomNumber]);
                    }
                    break;

                case Pickup.PickupType.Generic:
                default:
                    playerAudioSource.PlayOneShot(pickupGenericSound);
                    break;
            }
        }
    }

    public void WeaponSound(SoundMaterial.SoundMaterialType materialType)
    {
        Weapon EquippedWeapon = PlayerManager.Instance.equippedWeaponInfo;
        // HINT: This is a good place to play equipped weapon impact sound

        // Sandra
        AudioSource playerAudioSource = PlayerManager.Instance.playerAudioSource;
        if (playerAudioSource != null)
        {
            switch (EquippedWeapon.weaponType)
            {
                case WeaponTypes.Dagger:

                    switch (materialType)
                    {
                        case SoundMaterial.SoundMaterialType.dirt:
                            playerAudioSource.PlayOneShot(impDaggerDirtSound);
                            break;
                        case SoundMaterial.SoundMaterialType.grass:
                            playerAudioSource.PlayOneShot(impDaggerGrassSound);
                            break;
                        case SoundMaterial.SoundMaterialType.leaves:
                            playerAudioSource.PlayOneShot(impDaggerLeavesSound);
                            break;
                        case SoundMaterial.SoundMaterialType.stone:
                            playerAudioSource.PlayOneShot(impDaggerStoneSound);
                            break;
                        case SoundMaterial.SoundMaterialType.wood:
                            playerAudioSource.PlayOneShot(impDaggerWoodSound);
                            break;
                    }
                    break;

                case WeaponTypes.Sword:

                    switch (materialType)
                    {
                        case SoundMaterial.SoundMaterialType.dirt:
                            playerAudioSource.PlayOneShot(impSwordDirtSound);
                            break;
                        case SoundMaterial.SoundMaterialType.grass:
                            playerAudioSource.PlayOneShot(impSwordGrassSound);
                            break;
                        case SoundMaterial.SoundMaterialType.leaves:
                            playerAudioSource.PlayOneShot(impSwordLeavesSound);
                            break;
                        case SoundMaterial.SoundMaterialType.stone:
                            playerAudioSource.PlayOneShot(impSwordStoneSound);
                            break;
                        case SoundMaterial.SoundMaterialType.wood:
                            playerAudioSource.PlayOneShot(impSwordWoodSound);
                            break;
                    }
                    break;

                case WeaponTypes.Axe:

                    switch (materialType)
                    {
                        case SoundMaterial.SoundMaterialType.dirt:
                            playerAudioSource.PlayOneShot(impAxeDirtSound);
                            break;
                        case SoundMaterial.SoundMaterialType.grass:
                            playerAudioSource.PlayOneShot(impAxeGrassSound);
                            break;
                        case SoundMaterial.SoundMaterialType.leaves:
                            playerAudioSource.PlayOneShot(impAxeLeavesSound);
                            break;
                        case SoundMaterial.SoundMaterialType.stone:
                            playerAudioSource.PlayOneShot(impAxeStoneSound);
                            break;
                    }
                    break;

                case WeaponTypes.PickAxe:
                    break;

                case WeaponTypes.Hammer:
                    break;
            }
        }
    }
}
