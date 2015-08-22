using UnityEngine;
using System.Collections;

public class SpankScript : MonoBehaviour
{

	public AudioSource audioSourceToPlayOnClick;

	//private bool mouseWasDown = false;

	public SpriteRenderer normalFrame;
	public SpriteRenderer spankFrame;

	public GameObject underwearObject;
	MeshRenderer underwearMeshRenderer;
	public Material underwearNormalFrame;
	public Material underwearSpankFrame;


	public float spankDuration = 1.0f;
	private float spankEndTime = 0.0f;

	public Camera cam; 

	private bool spankOn = false;

	// Use this for initialization
	void Start ()
	{
		//Debug.Log("VR Mission: " + GlobalInfo.VRMission);
		if (GlobalInfo.VRMission)
		{
			cam.backgroundColor = Color.yellow;
		}

		underwearMeshRenderer = underwearObject.GetComponent<MeshRenderer>();
	}

	public void Spank()
	{
		audioSourceToPlayOnClick.PlayOneShot(audioSourceToPlayOnClick.clip);
		
		spankEndTime = Time.time + spankDuration;
		spankFrame.sortingOrder = 2;
		normalFrame.sortingOrder = 0;
		underwearMeshRenderer.material = underwearSpankFrame;
		spankOn = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (spankOn && Time.time > spankEndTime)
		{
			spankFrame.sortingOrder = 0;
			normalFrame.sortingOrder = 2;
			underwearMeshRenderer.material = underwearNormalFrame;
			spankOn = false;
		}

		//if (Input.GetMouseButtonDown(0))
		//{
		//	Spank();
		//}
	}
}
