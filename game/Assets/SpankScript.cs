using UnityEngine;
using System.Collections;

public class SpankScript : MonoBehaviour
{

	public AudioSource audioSourceToPlayOnClick;

	//private bool mouseWasDown = false;

	public SpriteRenderer normalFrame;
	public SpriteRenderer spankFrame;

	public float spankDuration = 1.0f;
	private float spankEndTime = 0.0f;

	// Use this for initialization
	void Start ()
	{

	}

	public void Spank()
	{
		audioSourceToPlayOnClick.PlayOneShot(audioSourceToPlayOnClick.clip);
		
		spankEndTime = Time.time + spankDuration;
		spankFrame.sortingOrder = 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > spankEndTime)
		{
			spankFrame.sortingOrder = 0;
		}

		//if (Input.GetMouseButtonDown(0))
		//{
		//	Spank();
		//}
	}
}
