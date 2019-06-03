using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsManager : MonoBehaviour
{
	[SerializeField]
	private SoundContainer soundContainer = new SoundContainer();

    // Start is called before the first frame update
    void Start()
    {
		soundContainer.Start();
    }

    // Update is called once per frame
    void Update()
    {
		soundContainer.UpdateTick();
    }
}
