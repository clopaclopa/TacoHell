using UnityEngine;
using System.Collections;

public class SmackAnyKeyScript : MonoBehaviour
{
	public GameObject cont;
	public GameObject vr;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			cont.SetActive (true);
			vr.SetActive (true);
			this.gameObject.SetActive(false);
		}
	}

}
