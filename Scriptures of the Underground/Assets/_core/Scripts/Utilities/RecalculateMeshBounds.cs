using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateMeshBounds : MonoBehaviour
{
    public float meshBound = 2000f;
    private void Start()
    {
        Mesh m = GetComponent<MeshFilter>().mesh;
        m.bounds = new Bounds(Vector3.zero, Vector3.one * meshBound);
        //mesh.bounds is in local/object space, so
        //setting a center of zero and extents of 2000 will
        //expand the bounds into a box 2000 units wide around
        //the center of the object
    }
}
