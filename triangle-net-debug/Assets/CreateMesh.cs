using UnityEngine;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;
public class CreateMesh : MonoBehaviour {

    Mesh mesh = null;

	// Update is called once per frame
	void Update() {
        var poly = new Polygon();
        var cont = FindObjectsOfType<Contour>();
		foreach(var c in cont)
        {
            poly.Add(c.GetContour(), c.hole);
        }

        var m = poly.Triangulate();
        var verts = ToVector3(m.Vertices);
        var tris = ToTriangles(m.Triangles);

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
    }

    Vector3[] ToVector3(ICollection<Vertex> vert)
    {
        List<Vertex> vertList = new List<Vertex>();
        foreach (var vt in vert) vertList.Add(vt);

        var v = new Vector3[vert.Count];
        for(int i = 0; i < vertList.Count; i ++)
        {
            v[i].x = (float)vertList[i].x;
            v[i].z = (float)vertList[i].y;
        }

        return v;
    }

    int[] ToTriangles(ICollection<Triangle> tris)
    {
        List<Triangle> triList = new List<Triangle>();
        foreach (var t in tris) triList.Add(t);


        int[] ts = new int[tris.Count * 3];
        for(int i = 0; i < triList.Count; i++)
        {
            Triangle tri = triList[i];
            ts[i * 3 + 2] = tri.vertices[0].ID;
            ts[i * 3 + 1] = tri.vertices[1].ID;
            ts[i * 3 + 0] = tri.vertices[2].ID;
        }

        return ts;
    }
}
