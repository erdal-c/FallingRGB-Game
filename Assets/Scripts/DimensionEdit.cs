using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionEdit : MonoBehaviour
{
    MeshFilter meshFilter;
    BoxCollider boxCollider;

    Vector3[] orgMeshVert; 
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();

        orgMeshVert = meshFilter.mesh.vertices;
        MeshDimensionEdit();
    }

    public void MeshDimensionEdit()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        float randomValue = Random.Range(0.5f, 2f);

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i].x *= randomValue;
            vertices[i].y = (meshFilter.mesh.vertices[i].x * meshFilter.mesh.vertices[i].y) / vertices[i].x;
        }

        meshFilter.mesh.vertices = vertices;

        boxCollider.size = new Vector3(-meshFilter.mesh.vertices[1].x,
                                    meshFilter.mesh.vertices[1].y - 0.02f,
                                    -meshFilter.mesh.vertices[1].z) * 2;
    }

    public void MeshDimensionReset()
    {
        meshFilter.mesh.vertices = orgMeshVert;
    }
}
