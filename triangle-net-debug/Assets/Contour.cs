using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Contour : MonoBehaviour {

    public bool hole = false;

    public TriangleNet.Geometry.Contour GetContour()
    {
        return new TriangleNet.Geometry.Contour(GetPoints());
    }

    public TriangleNet.Geometry.Vertex[] GetPoints()
    {
        var v = GetVectors();
        var p = new List<TriangleNet.Geometry.Vertex>();

        for (int i = 0; i < v.Length; i++)
        {
            Vector3 vec = v[i];
            p.Add(new TriangleNet.Geometry.Vertex(vec.x, vec.z));
        }

        return p.ToArray();
    }

    public Vector3[] GetVectors()
    {
        var trans = GetComponentsInChildren<Transform>();
        var v = new List<Vector3>();

        for(int i = 0; i < trans.Length; i ++)
        {
            if (trans[i] == transform) continue;
            Vector3 vec = trans[i].localPosition;
            vec.y = 0;
            v.Add(vec);
        }

        return v.ToArray();
    }

    private void Update()
    {
        var trans = GetComponentsInChildren<Transform>();
        foreach (var t in trans)
        {
            Vector3 v = t.localPosition;
            v.y = 0;
            t.localPosition = v;
        }
        


        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        if (UnityEditor.Selection.Contains(gameObject.GetInstanceID()) && UnityEditor.Selection.gameObjects.Length > 1) {
            List<GameObject> selection = new List<GameObject>(UnityEditor.Selection.gameObjects);
            selection.Remove(gameObject);

            UnityEditor.Selection.objects = selection.ToArray();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = hole ? Color.red : Color.white;
        var vec = GetVectors();
        for(int i = 0; i < vec.Length; i++)
        {
            int nexti = (i + 1) % vec.Length;

            Vector3 thisvec = vec[i];
            Vector3 nextvec = vec[nexti];

            Gizmos.DrawLine(thisvec, nextvec);
        }
    }
}
