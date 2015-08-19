using UnityEngine;
using System.Collections;

public class HandScrip : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Hello!");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			Debug.Log("Clicked!");
		}
	}
}
