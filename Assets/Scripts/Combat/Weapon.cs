////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor;

public class Weapon : MonoBehaviour, IInteractable
{
    //[Header("WWISE")]
    //public AK.Wwise.Switch WeaponTypeSwitch = new AK.Wwise.Switch();

    //[Header("Combo Actions")]
    //public AK.Wwise.Event ComboEvent = new AK.Wwise.Event();
    //public AK.Wwise.State WeaponCombo1 = new AK.Wwise.State();
    //public AK.Wwise.State WeaponCombo2 = new AK.Wwise.State();
    //public AK.Wwise.State WeaponCombo3 = new AK.Wwise.State();
    //[Space(20f)]

    public WeaponTypes weaponType = WeaponTypes.Dagger;
    public WeaponAnimationTypes weaponAnimationType = WeaponAnimationTypes.OneHanded;

    public bool equipped = false;
    public bool playerWeapon = false;

    [Header("Weapon Objects")]
    public GameObject prefab;
    public Collider hitbox;
    public GameObject hitEffect;

    [Header("Weapon Stats")]
    public float BaseDamage;
    public float attackCooldown;
    public float attackFrame;
    public float animationSpeedMultiplier = 1;
    public float knockbackStrength = 1f;
    public bool PickupEventOnPickup = true;

    [Header("Combo Info")]
    public float comboCompletionBonusDamage = 0;
    public int maxComboHits;
    public float postComboCooldown;
    public bool swingDash;
    public float dashAmount;
    public bool TriggerDamageAllowed = false;

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;

    private List<GameObject> alreadyHitObjects = new List<GameObject>();

    #region hidden public variables
    [HideInInspector]
    public bool applyBonusDamage = false;
    #endregion

    // Sandra
    private AudioSource audioSource;

    private const uint numAudios = 3;

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

    public void EnableHitbox()
    {
        //Reset list of already hit GameObjects (since this is a new swing)
        alreadyHitObjects.Clear();
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    //This method is called by the <Pickup> script
    public void OnInteract()
    {
        if (playerWeapon && !equipped)
        {
            if (!InteractionManager.inConversation)
            {
                // Add to picked up weapons
                if (!PlayerManager.Instance.pickedUpWeapons.Contains(weaponType))
                {
                    if (PickupEventOnPickup)
                    {
                        PlayerManager.Instance.StartPickupEvent();
                    }
                    PlayerManager.Instance.pickedUpWeapons.Add(weaponType);
                    PlayerManager.Instance.pickedUpWeaponObjects.Add(gameObject);
                }

                EquipWeapon();
            }
        }
    }

    private void Awake()
    {
        // Sandra
        audioSource = GetComponent<AudioSource>();

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
    }

    void Start()
    {
        prefab = gameObject;
        if (!playerWeapon)
        {
            BaseDamage = GetComponentInParent<Creature>().AttackDamage;
        }
        else
        {
            Physics.IgnoreCollision(hitbox, PlayerManager.Instance.playerCollider);
        }
    }

    public void EquipWeapon()
    {
        // destroy the pickup script 
        SetPickupEnabled(false);

        prefab.transform.position = PlayerManager.Instance.weaponSlot.transform.position;
        prefab.transform.rotation = PlayerManager.Instance.weaponSlot.transform.rotation;
        PlayerManager.Instance.Inventory_EquipWeapon(gameObject);

        PlayerManager.Instance.equippedWeaponInfo = this;
        PlayerManager.Instance.equippedWeapon = prefab;
        prefab.transform.parent = PlayerManager.Instance.weaponSlot.transform;
        Utility.StripGameObjectFromComponents(gameObject, typeof(Pickup));
        equipped = true;
    }

    public void UnequipWeapon()
    {
        PlayerManager.Instance.Inventory_UnequipWeapon(gameObject);
    }

    void SetPickupEnabled(bool enabled)
    {
        var pickupScript = gameObject.GetComponent<Pickup>();
        if (pickupScript != null)
        {
            pickupScript.enabled = enabled;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (TriggerDamageAllowed && !playerWeapon)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Attack attack = new Attack(BaseDamage, transform.position - PlayerManager.Instance.player.transform.position, knockbackStrength);

                if (!alreadyHitObjects.Contains(col.gameObject))
                {
                    SetAndPlayWeaponImpact(col.gameObject);
                    GameManager.DamageObject(col.gameObject, attack);
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (playerWeapon)
        {
            if (equipped && col.gameObject.tag != "Player")
            {
                Vector3 playerToHitPoint = col.contacts[0].point - PlayerManager.Instance.player.transform.position;
                Attack attack = new Attack(BaseDamage, playerToHitPoint, knockbackStrength, SwingTypes.None, weaponType, col.contacts[0].point);

                AnimatorStateInfo currentAnimation = PlayerManager.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0);
                if (currentAnimation.IsName("Player_RightSwing"))
                {
                    attack.swingType = SwingTypes.Right;
                    // HINT: Weapon combo state 1, you may want to take this into account when playing the weapon swing sound                   

                    // Sandra
                    audioSource.PlayOneShot(swingSounds[0]);
                }
                else if (currentAnimation.IsName("Player_LeftSwing"))
                {
                    attack.swingType = SwingTypes.Left;
                    // HINT: Weapon combo state 2, you may want to take this into account when playing the weapon swing sound

                    // Sandra
                    audioSource.PlayOneShot(swingSounds[1]);
                }
                else if (currentAnimation.IsName("Player_TopSwing"))
                {
                    attack.swingType = SwingTypes.Top;
                    // HINT: Weapon combo state 3, you may want to take this into account when playing the weapon swing sound

                    // Sandra
                    audioSource.PlayOneShot(swingSounds[2]);
                }

                if (!alreadyHitObjects.Contains(col.gameObject))
                {
                    // HINT: Get material of the contact point
                    SoundMaterial sm = col.gameObject.GetComponent<SoundMaterial>();
                    if (sm != null) {

                        uint thisSwitch = 0;
                        //print("Current Switch: "+ thisSwitch +", New: "+ sm.material.ID);

                        // HINT: Update impact material
                        /*if (thisSwitch != (uint)sm.material.ID)
                        {
                            sm.material.SetValue(transform.parent.gameObject); // Set Impact Material
                                                                               //print("New Impact Material: "+ sm.gameObject.name);
                        }*/
                    }

                    SetAndPlayWeaponImpact(col.gameObject);
                    GameManager.DamageObject(col.gameObject, attack);

                    GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject; //TODO: Pool hit effects
                    Destroy(hit, 5f);

                    if (col.gameObject.layer == LayerMask.NameToLayer("Agent"))
                    {
                        //ComboEvent.Post(transform.parent.gameObject);
                        attack.damage += applyBonusDamage ? comboCompletionBonusDamage : 0;

                        float newTimeScale = applyBonusDamage ? 0.2f : 0.5f;
                        float transitionTime = 0.1f;
                        float holdTime = applyBonusDamage ? 0.2f : 0.1f;
                        float shakeDuration = applyBonusDamage ? 0.3f : 0.15f;
                        float shakeScale = applyBonusDamage ? 0.2f : 0.1f;
                        
                        GameManager.Instance.gameSpeedHandler.SetGameSpeed(gameObject.GetInstanceID(), newTimeScale, transitionTime, transitionTime, holdTime);
                        PlayerManager.Instance.CamShake(new PlayerCamera.CameraShake(shakeScale, shakeDuration));
                    }
                }
            }
        }
        else
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Attack attack = new Attack(BaseDamage, col.contacts[0].point - PlayerManager.Instance.player.transform.position, BaseDamage);
                GameManager.DamageObject(col.gameObject, attack);
                // HINT: Play weapon impact event here, weapon type = transform.parent.gameObject

                // Sandra
                //Weapon weapon = transform.parent.gameObject.GetComponent<Weapon>();
                SoundMaterial soundMaterial = col.gameObject.GetComponent<SoundMaterial>();

                if (soundMaterial != null)
                    PlayWeaponImpact(weaponType, (SoundMaterial.SoundMaterialType)soundMaterial.material);
            }
        }
    }
    void SetAndPlayWeaponImpact(GameObject HitObj){
        //print("Impact");
        alreadyHitObjects.Add(HitObj);
        // HINT: Play weapon impact event here, weapon type = transform.parent.gameObject

        // Sandra
        //Weapon weapon = transform.parent.gameObject.GetComponent<Weapon>();
        SoundMaterial soundMaterial = HitObj.GetComponent<SoundMaterial>();

        if (soundMaterial != null)
            PlayWeaponImpact(weaponType, (SoundMaterial.SoundMaterialType)soundMaterial.material);
    }

    // Sandra
    private void PlayWeaponImpact(WeaponTypes weaponType, SoundMaterial.SoundMaterialType soundMaterialType)
    {
        switch (weaponType)
        {
            case WeaponTypes.Dagger:

                switch (soundMaterialType)
                {
                    case SoundMaterial.SoundMaterialType.dirt:
                        audioSource.PlayOneShot(impDaggerDirtSound);
                        break;
                    case SoundMaterial.SoundMaterialType.grass:
                        audioSource.PlayOneShot(impDaggerGrassSound);
                        break;
                    case SoundMaterial.SoundMaterialType.leaves:
                        audioSource.PlayOneShot(impDaggerLeavesSound);
                        break;
                    case SoundMaterial.SoundMaterialType.stone:
                        audioSource.PlayOneShot(impDaggerStoneSound);
                        break;
                    case SoundMaterial.SoundMaterialType.wood:
                        audioSource.PlayOneShot(impDaggerWoodSound);
                        break;
                }
                break;

            case WeaponTypes.Sword:

                switch (soundMaterialType)
                {
                    case SoundMaterial.SoundMaterialType.dirt:
                        audioSource.PlayOneShot(impSwordDirtSound);
                        break;
                    case SoundMaterial.SoundMaterialType.grass:
                        audioSource.PlayOneShot(impSwordGrassSound);
                        break;
                    case SoundMaterial.SoundMaterialType.leaves:
                        audioSource.PlayOneShot(impSwordLeavesSound);
                        break;
                    case SoundMaterial.SoundMaterialType.stone:
                        audioSource.PlayOneShot(impSwordStoneSound);
                        break;
                    case SoundMaterial.SoundMaterialType.wood:
                        audioSource.PlayOneShot(impSwordWoodSound);
                        break;
                }
                break;

            case WeaponTypes.Axe:

                switch (soundMaterialType)
                {
                    case SoundMaterial.SoundMaterialType.dirt:
                        audioSource.PlayOneShot(impAxeDirtSound);
                        break;
                    case SoundMaterial.SoundMaterialType.grass:
                        audioSource.PlayOneShot(impAxeGrassSound);
                        break;
                    case SoundMaterial.SoundMaterialType.leaves:
                        audioSource.PlayOneShot(impAxeLeavesSound);
                        break;
                    case SoundMaterial.SoundMaterialType.stone:
                        audioSource.PlayOneShot(impAxeStoneSound);
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
