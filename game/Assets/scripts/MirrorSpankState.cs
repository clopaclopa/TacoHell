using UnityEngine;
using System.Collections;

public class MirrorSpankState : MonoBehaviour
{
	public GameObject spankControllerObject;
	private SpankScript spankScript;

	public Material normalMaterial;
	public Material spankMaterial;

	private MeshRenderer thisMeshRenderer;

	// Use this for initialization
	void Start ()
	{
		if (spankControllerObject)
		{
			this.spankScript = spankControllerObject.GetComponent<SpankScript>();
		}
		this.thisMeshRenderer = this.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.spankScript.spankOn)
		{
			this.thisMeshRenderer.material = spankMaterial;
		}
		else
		{
			this.thisMeshRenderer.material = normalMaterial;
		}
	}
}
