using UnityEngine;
using System.Collections;

public class SmackAnyKeyScript : MonoBehaviour
{
	public AudioSource audio1;
	public AudioSource audio2;

	private bool fadeVolumeAndTransition;
	private float volume1;
	private float volume2;
	//private float deltavol1;
	//private float deltavol2;

	public float FadeDelta = 0.1f;

	// Use this for initialization
	void Start ()
	{
		fadeVolumeAndTransition = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!fadeVolumeAndTransition && Input.GetMouseButtonDown(0))
		{
			fadeVolumeAndTransition = true;
			volume1 = audio1.volume;
			volume2 = audio2.volume;
		}

		if (fadeVolumeAndTransition)
		{
			audio1.volume -= FadeDelta * Time.deltaTime;
			audio2.volume -= FadeDelta * Time.deltaTime;
			if (audio1.volume < 0.01f)
			{
				audio1.volume = 0.0f;
			}
			if (audio2.volume < 0.01f)
			{
				audio2.volume = 0.0f;
			}
		}

		if (audio1.volume == 0.0f && audio2.volume == 0.0f)
		{
			Application.LoadLevel("gamescene");
			audio1.Stop();
			audio2.Stop();
			audio1.volume = volume1;
			audio2.volume = volume2;
		}
	}
}
