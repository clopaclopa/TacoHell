using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class TriSlicer
{
	public struct Tri
	{
		public Vector2 v1;// = new Vector3[3]();
		public Vector2 v2;
		public Vector2 v3;
		public Vector2 vel;
	}

	Mesh mesh;

	//Vector2[] verts;
	//List<int[]> tris = new List<int[]>();
	Tri[] tris = null;

	//List<Vector2> trivels = new List<Vector2>();

	//Constructor, must initialize with a mesh
	public TriSlicer(Mesh _mesh)
	{
		mesh = _mesh;
		//For now, this doesn't load the mesh into the 2d structure (could project or something later)
	}

	//Rebuilds actual mesh from the 2d structure within TriSlicer
	public void RefreshMesh()
	{
		if (this.mesh == null || this.tris == null || this.tris.Length == 0) return;

//		Vector3[] verts3 = new Vector3[verts.Length];
//		int i = 0;
//		foreach (Vector2 vert in verts)
//		{
//			verts3[i++] = new Vector3(vert[0],0,vert[1]);
//			//Debug.Log(verts3[i-1].ToString());
//		}
//		int[] tris3 = new int[tris.Count * 3];
//		i = 0;
//		foreach (int[] tri in tris)
//		{
//			tris3[i++] = tri[0];
//			tris3[i++] = tri[1];
//			tris3[i++] = tri[2];
//		}

		Vector3[] verts3 = new Vector3[tris.Length * 3];
		int[] tris3 = new int[tris.Length * 3];
		int i = 0;
		foreach (Tri tri in tris)
		{
			verts3[i*3+0] = new Vector3(tri.v1[0],0,tri.v1[1]);
			verts3[i*3+1] = new Vector3(tri.v2[0],0,tri.v2[1]);
			verts3[i*3+2] = new Vector3(tri.v3[0],0,tri.v3[1]);
			
			tris3[i*3+0] = i*3+0;
			tris3[i*3+1] = i*3+1;
			tris3[i*3+2] = i*3+2;

			i++;
		}

		mesh.Clear();
		mesh.vertices = verts3;
		mesh.triangles = tris3;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		//Debug.Log("verts len: " + mesh.vertices.Length);
		//Debug.Log("tris len: " + mesh.triangles.Length);
		//string aoeu = "";
		//foreach (Vector3 vec in mesh.vertices)
		//{
		//	aoeu += " - " + vec.ToString() + " - ";
		//}
		//Debug.Log(aoeu);
	}

	//Initialize the mesh with a plane
	public void InitPlane()
	{
		Tri t1 = new Tri();// = Tri;
		Tri t2 = new Tri();// = Tri;
		t1.v1 = new Vector3(-1,-1);
		t1.v2 = new Vector3(-1,1);
		t1.v3 = new Vector3(1,1);
		t2.v1 = new Vector3(1,1);
		t2.v2 = new Vector3(1,-1);
		t2.v3 = new Vector3(-1,-1);

		t1.vel = new Vector2(-0.1f,0.1f);
		t2.vel = new Vector2(0.1f,-0.1f);

		this.tris = new Tri[]{t1, t2};


		//Vector2[] verts2 = {new Vector3(-1,-1), new Vector3(-1,1), new Vector3(1,1), new Vector3(1,-1)};
		//int[] tris2 = {0,1,2,2,3,0};

//		tris.Clear();
//		tris.Add(new int[]{0,1,2});
//		tris.Add(new int[]{2,3,0});
//
//		verts = verts2;
//
//		trivels = new List<Vector2>();
//		for (int i = 0; i < tris.Count; i++)
//		{
//			trivels.Add(new Vector2(0,0));
//		}

		//Debug.Log("verts len: " + verts.Length);
		//Debug.Log("tris len: " + tris.Count);

		RefreshMesh();
	}

	public void UpdateMovements(float deltaTime)
	{
		if (this.mesh == null || this.tris == null || this.tris.Length == 0) return;
		int i = 0;
		for (i = 0; i < tris.Length; i++)
		{
			Tri tri = tris[i];
			tri.v1 += deltaTime * tri.vel;
			tri.v2 += deltaTime * tri.vel;
			tri.v3 += deltaTime * tri.vel;
			tris[i] = tri;
		}
	}

	public void Slice(Vector2 a, Vector2 b)
	{
		Vector2 ab = b-a;
		foreach (Tri tri in tris)
		{
			//first do broadband test line-circle or line-square maybe

			//slice this triangle

			//intersect edge 1

			//intersect edge 2

			//intersect edge 3

		}
	}
}

public class SliceScript : MonoBehaviour
{
	MeshFilter meshfilter;
	TriSlicer trislicer;

	// Use this for initialization
	void Start ()
	{
		meshfilter = this.GetComponent<MeshFilter>();
		trislicer = new TriSlicer(meshfilter.mesh);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			trislicer.InitPlane();
		}

		//Update moving slices
		trislicer.UpdateMovements(Time.deltaTime);
		//Slice?

		//Rebuild actual mesh from 2d slice structure 
		trislicer.RefreshMesh();
	}
}
