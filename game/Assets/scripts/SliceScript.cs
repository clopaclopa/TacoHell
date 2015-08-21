using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;

public class TriSlicer
{
	public class Tri
	{
		public Vector2 v1;// = new Vector3[3]();
		public Vector2 v2;
		public Vector2 v3;
		public Vector2 uv1;
		public Vector2 uv2;
		public Vector2 uv3;
		public Vector2 vel;
	}

	Mesh mesh;

	//Vector2[] verts;
	//List<int[]> tris = new List<int[]>();
	public List<Tri> tris = null;

	//List<Vector2> trivels = new List<Vector2>();

	//Constructor, must initialize with a sacrificial mesh
	public TriSlicer(Mesh _mesh)
	{
		mesh = _mesh;
		//For now, this doesn't load the mesh into the 2d structure (could project or something later)
	}

	public override string ToString()
	{
		string ret = "TriSlicer! Tri count: " + this.tris.Count + " \n";
		foreach (Tri tri in this.tris)
		{
			ret += "Tri: " + tri.v1 + " " + tri.v2 + " " + tri.v3 + " vel: " + tri.vel + "\n";
		}
		return ret;
	}

	//Rebuilds actual mesh from the 2d structure within TriSlicer
	public void RefreshMesh()
	{
		if (this.mesh == null || this.tris == null || this.tris.Count == 0) return;

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

		Vector3[] verts3 = new Vector3[tris.Count * 3];
		int[] tris3 = new int[tris.Count * 3];
		Vector2[] uvs3 = new Vector2[tris.Count * 3];
		int i = 0;
		foreach (Tri tri in tris)
		{
			verts3[i*3+0] = new Vector3(tri.v1[0],0,tri.v1[1]);
			verts3[i*3+1] = new Vector3(tri.v2[0],0,tri.v2[1]);
			verts3[i*3+2] = new Vector3(tri.v3[0],0,tri.v3[1]);
			
			tris3[i*3+0] = i*3+0;
			tris3[i*3+1] = i*3+1;
			tris3[i*3+2] = i*3+2;

			uvs3[i*3+0] = tri.uv1;
			uvs3[i*3+1] = tri.uv2;
			uvs3[i*3+2] = tri.uv3;

			i++;
		}

		mesh.Clear();
		mesh.vertices = verts3;
		mesh.triangles = tris3;
		mesh.uv = uvs3;
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
	public void InitPlane(float scale)
	{
		Tri t1 = new Tri();// = Tri;
		Tri t2 = new Tri();// = Tri;

		t1.v1 = new Vector3(-scale,-scale);
		t1.v2 = new Vector3(-scale,scale);
		t1.v3 = new Vector3(scale,scale);
		t1.uv1 = new Vector2(0,0);
		t1.uv2 = new Vector2(0,1);
		t1.uv3 = new Vector2(1,1);

		t2.v1 = new Vector3(scale,scale);
		t2.v2 = new Vector3(scale,-scale);
		t2.v3 = new Vector3(-scale,-scale);
		t2.uv1 = new Vector2(1,1);
		t2.uv2 = new Vector2(1,0);
		t2.uv3 = new Vector2(0,0);

		//t1.vel = new Vector2(-0.1f,0.1f);
		//t2.vel = new Vector2(0.1f,-0.1f);
		t1.vel = new Vector2(0,0);
		t2.vel = new Vector2(0,0);

		this.tris = new List<Tri>();
		tris.Add(t1);
		tris.Add(t2);


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
		if (this.mesh == null || this.tris == null || this.tris.Count == 0) return;
		int i = 0;
		for (i = 0; i < tris.Count; i++)
		{
			Tri tri = tris[i];
			tri.v1 += deltaTime * tri.vel;
			tri.v2 += deltaTime * tri.vel;
			tri.v3 += deltaTime * tri.vel;
			tris[i] = tri;
		}
	}

	//returns t from A1, scaled to S1
	//A1 and A2 are starting points for a ray, S1 and S2 are the slopes
	public static float LineLineIntersectReturnT(Vector2 A1, Vector2 S1, Vector2 A2, Vector2 S2)
	{
		return (S2.y * (A2.x - A1.x) + S2.x * (A1.y - A2.y)) / ((S1.x * S2.y) - (S2.x * S1.y));
	}

	public void Slice(Vector2 a, Vector2 b)
	{
		if (this.tris.Count > 5000) return;

		//Debug.Log("Slice begin. Tris len: " + tris.Count);

		Vector2 s = b-a;
		List<Tri> removethese = new List<Tri>();
		List<Tri> addthese = new List<Tri>();
		foreach (Tri tri in tris)
		{
			//first do broadband test line-circle or line-square maybe

			//slice this triangle
			//intersect edge 1
			Vector2 t1a = tri.v1;
			Vector2 t1s = tri.v2-tri.v1;
			float t1 = LineLineIntersectReturnT(t1a, t1s, a, s);
			bool isect1valid = t1 > 0 && t1 < 1;
			//intersect edge 2
			Vector2 t2a = tri.v2;
			Vector2 t2s = tri.v3-tri.v2;
			float t2 = LineLineIntersectReturnT(t2a, t2s, a, s);
			bool isect2valid = t2 > 0 && t2 < 1;
			//intersect edge 3
			Vector2 t3a = tri.v3;
			Vector2 t3s = tri.v1-tri.v3;
			float t3 = LineLineIntersectReturnT(t3a, t3s, a, s);
			bool isect3valid = t3 > 0 && t3 < 1;

			int numvalid = (isect1valid?1:0) + (isect3valid?1:0) + (isect2valid?1:0);
			if (numvalid == 2)
			{
				Vector2 isect1 = t1a + t1 * t1s;
				Vector2 isect2 = t2a + t2 * t2s;
				Vector2 isect3 = t3a + t3 * t3s;

				//figure out which vertex of the triangle is shared by the two intersected edges
				Vector2 common;
				Vector2 p1, p2, o1, o2;
				if (isect1valid && isect2valid)
				{
					common = tri.v2;
					p1 = isect1;
					p2 = isect2;
					o1 = tri.v1;
					o2 = tri.v3;
				}
				else if (isect2valid && isect3valid)
				{
					common = tri.v3;
					p1 = isect2;
					p2 = isect3;
					o1 = tri.v2;
					o2 = tri.v1;
				}
				else //if (isect3valid && isect1valid)
				{
					common = tri.v1;
					p1 = isect3;
					p2 = isect1;
					o1 = tri.v3;
					o2 = tri.v2;
				}

				//create new triangles
				Tri nt1 = new Tri();
				Tri nt2 = new Tri();
				Tri nt3 = new Tri();

				nt1.v1 = p1;
				nt1.v2 = common;
				nt1.v3 = p2;

				nt2.v1 = o1;
				nt2.v2 = p1;
				nt2.v3 = p2;

				nt3.v1 = p2;
				nt3.v2 = o2;
				nt3.v3 = o1;

				//Give them velocities
				Vector2 velcommon = (common - (p1 + (p2-p1) * 0.5f));
				nt1.vel = tri.vel + 0.1f * velcommon;// + (Vector2)Random.onUnitSphere*0.01f;
				nt2.vel = tri.vel + 0.1f * -velcommon;// + (Vector2)Random.onUnitSphere*0.01f;
				nt3.vel = tri.vel + 0.1f * -velcommon;// + (Vector2)Random.onUnitSphere*0.01f;

				addthese.Add(nt1);
				addthese.Add(nt2);
				addthese.Add(nt3);

				//tag old ones for removal
				removethese.Add(tri);

			}//if there was a valid slice made
		}//loop over all triangles to check slices

		//Remove tris tagged for removal
		foreach (Tri tri in removethese)
		{
			this.tris.Remove(tri);
		}

		//Add tris created by slice
		foreach (Tri tri in addthese)
		{
			this.tris.Add(tri);
		}

		//Debug.Log("Slice end. Tris len: " + tris.Count + " Removed: " + removethese.Count + " Added: " + addthese.Count);
		//Debug.Log(this.ToString());
	}//Slice function
}

public class SliceScript : MonoBehaviour
{
	MeshFilter meshfilter;
	TriSlicer trislicer;

	public GameObject debugtext;

	// Use this for initialization
	void Start ()
	{
		meshfilter = this.GetComponent<MeshFilter>();
		trislicer = new TriSlicer(meshfilter.mesh);

		trislicer.InitPlane(Mathf.Abs(this.transform.lossyScale.x)*5f);
		this.transform.localScale = new Vector3(1,1,1);

		//float t = 123.0f;
		//t = trislicer.LineLineIntersectReturnT(new Vector2(0,0), new Vector2(1,0), new Vector2(0.5f,-1), new Vector2(0f,1));
		//Debug.Log("t:" + t);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 clickpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			trislicer.Slice(new Vector2(-1,clickpoint.y),new Vector2(1,clickpoint.y));
		}

		//Update moving slices
		trislicer.UpdateMovements(Time.deltaTime);
		//Slice?

		//Rebuild actual mesh from 2d slice structure 
		trislicer.RefreshMesh();

		this.debugtext.GetComponent<Text>().text = "Zangeki: " + trislicer.tris.Count;
	}
}
