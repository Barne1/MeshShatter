using System.Collections.Generic;
using UnityEngine;

public class PartMesh
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    //triangles[x] where x will be submesh index
    public List<List<int>> triangles = new List<List<int>>();

    Bounds bounds = new Bounds();

    GameObject newGameObject;

    public void Initialize(Mesh originalMesh)
    {
        for (int i = 0; i < originalMesh.vertices.Length; i++)
        {
            this.vertices.Add(originalMesh.vertices[i]);
            this.normals.Add(originalMesh.normals[i]);
            this.uvs.Add(originalMesh.uv[i]);
        }
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            triangles.Add(new List<int>());
            int[] cachcedTriangleArray = originalMesh.GetTriangles(i);
            for (int j = 0; j < cachcedTriangleArray.Length; j++)
            {
                triangles[i].Add(cachcedTriangleArray[j]);
            }
        }
    }

    public void AddTriangle(Triangle triangle)
    {
        if (triangles.Count - 1 < triangle.submesh)
        {
            triangles.Add(new List<int>());
            
        }

        for (int i = 0; i < Triangle.arraySize; i++)
        {
            triangles[triangle.submesh].Add(vertices.Count);
            vertices.Add(triangle.vertices[i]);
            normals.Add(triangle.normals[i]);
            uvs.Add(triangle.uvs[i]);

            //bounds.min = Vector3.Min(bounds.min, vertices[i]);
            //bounds.max = Vector3.Min(bounds.max, vertices[i]);
        }
    }

    public void CreateGameObject(GameObject original)
    {
        newGameObject = new GameObject();
        newGameObject.transform.position = original.transform.position;
        newGameObject.transform.rotation = original.transform.rotation;
        newGameObject.transform.localScale = original.transform.localScale;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        for (int i = 0; i < triangles.Count; i++)
        {
            mesh.SetTriangles(triangles[i], i);
        }

        //bounds = mesh.bounds;
        mesh.RecalculateBounds();

        MeshRenderer mr = newGameObject.AddComponent<MeshRenderer>();
        mr.materials = original.GetComponent<MeshRenderer>().materials;

        MeshFilter mf = newGameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        mf.mesh.name = "test";

        MeshCollider collider = newGameObject.AddComponent<MeshCollider>();
        collider.convex = true;

        newGameObject.AddComponent<Rigidbody>();
    }

}
