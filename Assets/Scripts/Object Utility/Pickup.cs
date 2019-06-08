////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEditor;

public class Pickup : MonoBehaviour, IInteractable
{
	public GameObject pickupParticles;
	public bool DestroyOnPickup = false;

	[Header("Animation Properties")]
	public bool pickupAnimationOnInteract = true;
	public bool hoverEffect = true;
	public float hoverScale = 0.1f;
	public bool rotation = true;
	public float rotationSpeed = 50f;
	public bool addedToInteractManager = false;
	public bool InteractionEnabled = true;

	public bool interactionSound = true;
	[HideInInspector]
	public SphereCollider trigger;

    // Sandra
    public enum PickupType
    {
        Generic,
        Book,
        Crystals,
        EvilEssence,
        Key,
        Mushroom,
        Pinecone,
        Axe,
        Dagger,
        Hammer,
        Pickaxe,
        Sword,
        Coin
    }
    public PickupType pickupType;

    #region private variables
    private float randomOffset;
	private bool playerInTrigger;
	private GameObject outline;
	private bool inConversation = false;
	private bool isFocus = false;
	private ObjectOutline objectOutline;

    // Sandra
    private AudioSource audioSource;

    private const uint numAudios = 3;

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
    #endregion

    //Events
    public UnityEvent OnBecameFocus;
	public UnityEvent OnInteraction;

    private void Awake()
    {
        // Sandra
        audioSource = GetComponent<AudioSource>();

        string basePath = "Assets/Audio Assets/SFX/Objects/Pickups/";
        string name = "BAS_Pickup_";
        string extension = ".wav";
        string number = "_0";

        string path = null;

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
    }

    void Start()
	{
		randomOffset = Random.Range(0, 2 * Mathf.PI);
	}

	void OnEnable()
	{
        // HINT: Good place to save the pickup type

        if (transform.childCount > 0)
		{
			Transform T = transform.Find("Outline");
			if (T != null)
			{
				outline = T.gameObject;
				objectOutline = outline.GetComponent<ObjectOutline>();
			}
		}

		if (trigger == null)
		{
			var sphereCollider = GetComponent<SphereCollider>();
			if (sphereCollider != null)
			{
				trigger = sphereCollider;
			}
			else
			{
				SphereCollider col = gameObject.AddComponent<SphereCollider>();
				trigger = col;
				col.isTrigger = true;
				col.radius = 1;
			}
		}
		if (trigger != null)
		{
			trigger.enabled = true;
		}
		else
		{
			print("You forgot a sphere trigger on object: " + this.gameObject.name);
		}

        if (!InteractionEnabled)
        {
            InteractionEnabled = true;
        }
	}

	void Update()
	{
		if (hoverEffect)
		{
			transform.position += new Vector3(0, Mathf.Sin(Time.time + randomOffset) * Time.deltaTime * hoverScale, 0);
		}
		if (rotation)
		{
			transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0));
		}

		if (InteractionManager.InteractableObjects.Count > 0 && InteractionManager.InteractableObjects[0] == gameObject)
		{
			if (!isFocus)
			{
				OnBecameFocus.Invoke();
			}
			isFocus = true;
			if (!inConversation)
			{
				if (objectOutline != null)
				{
					if (!objectOutline.isEnabled)
					{
						objectOutline.EnableOutline();
					}
				}
				else if (outline != null)
				{
					outline.SetActive(true);
				}
			}

		}
		else
		{
			isFocus = false;

			if (objectOutline != null)
			{
				if (objectOutline.isEnabled)
				{
					objectOutline.DisableOutline();
				}
			}
			else if (outline != null)
			{
				outline.SetActive(false);
			}
			inConversation = false;
		}
	}

	void OnDisable()
	{
		trigger.enabled = false;
		InteractionManager.SetCanInteract(gameObject, false);
		InteractionEnabled = false;
		addedToInteractManager = false;
		if (outline != null)
		{
			outline.SetActive(false);
			inConversation = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && !addedToInteractManager)
		{
			if (InteractionEnabled)
			{
				InteractionManager.SetCanInteract(gameObject, true);
				addedToInteractManager = true;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			if (InteractionEnabled)
			{
				InteractionManager.SetCanInteract(gameObject, false);
				addedToInteractManager = false;
				inConversation = false;
			}
		}
	}

	public void OnInteract()
	{
		if (InteractionEnabled && this.enabled)
		{
			OnInteraction.Invoke();

			if (interactionSound)
			{
                // HINT: Play the sound for this pickup

                // Sandra
                switch (pickupType)
                {
                    case PickupType.Book:
                        audioSource.PlayOneShot(pickupBookSound);
                        break;

                    case PickupType.Crystals:
                        {
                            int randomNumber = Random.Range(0, (int)numAudios);
                            audioSource.PlayOneShot(pickupCrystalsSounds[randomNumber]);
                        }
                        break;

                    case PickupType.EvilEssence:
                        audioSource.PlayOneShot(pickupEvilEssenceSound);
                        break;

                    case PickupType.Key:
                        audioSource.PlayOneShot(pickupKeySound);
                        break;

                    case PickupType.Mushroom:
                        audioSource.PlayOneShot(pickupMushroomSound);
                        break;

                    case PickupType.Pinecone:
                        audioSource.PlayOneShot(pickupPineConeSound);
                        break;

                    case PickupType.Axe:
                        audioSource.PlayOneShot(pickupAxeSound);
                        break;

                    case PickupType.Dagger:
                        audioSource.PlayOneShot(pickupDaggerSound);
                        break;

                    case PickupType.Hammer:
                        audioSource.PlayOneShot(pickupHammerSound);
                        break;

                    case PickupType.Pickaxe:
                        audioSource.PlayOneShot(pickupPickaxeSound);
                        break;

                    case PickupType.Sword:
                        audioSource.PlayOneShot(pickupSwordSound);
                        break;

                    case PickupType.Coin:
                        {
                            int randomNumber = Random.Range(0, (int)numAudios);
                            audioSource.PlayOneShot(pickupCoinSounds[randomNumber]);
                        }
                        break;

                    case PickupType.Generic:
                    default:
                        audioSource.PlayOneShot(pickupGenericSound);
                        break;
                }
			}
			if (pickupParticles != null)
			{
				GameObject p = Instantiate(pickupParticles, transform.position, Quaternion.identity) as GameObject;
				Destroy(p, 5f);
			}

			if (pickupAnimationOnInteract)
			{
				SetInteractionEnabled(false);
				if (trigger != null)
				{
					trigger.enabled = false;
				}
				InteractionManager.SetCanInteract(this.gameObject, false);
				StartCoroutine(PickupAnimation());
			}
		}
	}

	public void SetInteractionEnabled(bool enabled)
	{
		InteractionEnabled = enabled;

        if (!InteractionEnabled)
        {
            InteractionManager.SetCanInteract(gameObject, false);
            addedToInteractManager = false;
            inConversation = false;
        }
	}

    public void SetInteractionEnabledTrueWithDelay(float delay)
    {
        StartCoroutine(SetInteractionEnabledDelayed(delay));
    }

    private IEnumerator SetInteractionEnabledDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetInteractionEnabled(true);
    }

	public void SetInteractionSoundActive(bool enabled)
	{
		interactionSound = enabled;
	}

	IEnumerator PickupAnimation()
	{
		Transform target = PlayerManager.Instance.playerHead.transform;
		Vector3 origPos = transform.position;

		Vector3 origSize = transform.localScale;

		float speed = 1f;
		for (float t = 0; t < 1f; t += Time.deltaTime / speed)
		{
			float yOffsetValue = Curves.Instance.Hill.Evaluate(t) * 2f;
			transform.position = Vector3.Lerp(origPos, target.position + Vector3.zero.WithY(yOffsetValue), t);

			float scaleCurveValue = Curves.Instance.SmoothIn.Evaluate(t);
			transform.localScale = Vector3.Lerp(origSize, Vector3.zero, scaleCurveValue);
			yield return null;
		}
		transform.localScale = Vector3.zero;

		if (DestroyOnPickup)
		{
			Destroy(gameObject);
		}
	}

}
 