using System.Collections.Generic;
using UnityEngine;

public class MeshCutter : MonoBehaviour
{
    public Transform cuttingPlane;
    bool edgeSet = false;

    public void Cut(GameObject originalObject)
    {
        Mesh originalMesh = originalObject.GetComponent<MeshFilter>().mesh;
        originalMesh.RecalculateBounds();

        //List<PartMesh> parts = new List<PartMesh>();
        List<PartMesh> subParts = new List<PartMesh>();

        PartMesh mainPart = new PartMesh();
        mainPart.Initialize(originalMesh);
        //parts.Add(mainPart);

        Plane plane = new Plane(cuttingPlane.transform.up, cuttingPlane.transform.position);

        subParts.Add(GenerateMesh(mainPart, plane, true));
        subParts.Add(GenerateMesh(mainPart, plane, false));

        foreach (PartMesh part in subParts)
        {
            part.CreateGameObject(originalObject);
        }
        Destroy(originalObject);
    }

    private PartMesh GenerateMesh(PartMesh original, Plane plane, bool left)
    {
        PartMesh partMesh = new PartMesh();
        Ray ray1 = new Ray();
        Ray ray2 = new Ray();

        for (int i = 0; i < original.triangles.Count; i++)
        {
            int[] triangles = original.triangles[i].ToArray();
            edgeSet = false;

            for (int j = 0; j < triangles.Length; j+=3)
            {
                bool sideA = plane.GetSide(original.vertices[triangles[j]]) == left;
                bool sideB = plane.GetSide(original.vertices[triangles[j+1]]) == left;
                bool sideC = plane.GetSide(original.vertices[triangles[j+2]]) == left;

                int sideCount = (sideA ? 1 : 0) + (sideB ? 1 : 0) + (sideC ? 1 : 0);
                Debug.Log(sideCount);

                if (sideCount == 0)
                {
                    continue;
                }
                else if(sideCount == 3)
                {
                    Triangle triangle = GetTriangle(triangles, i, j, original);
                    partMesh.AddTriangle(triangle);
                    continue;
                }

                int singleIndex = (sideB == sideC ? 0 : sideA == sideC ? 1 : 2);

                ray1.origin = original.vertices[triangles[j+singleIndex]];
                var dir1 = original.vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
            }
        }

        return partMesh;
    }

    Triangle GetTriangle(int[] submeshTriangles, int submesh, int triangleIndex, PartMesh originalMesh)
    {
        Vector3[] verts = {
            originalMesh.vertices[submeshTriangles[triangleIndex]],
            originalMesh.vertices[submeshTriangles[triangleIndex + 1]],
            originalMesh.vertices[submeshTriangles[triangleIndex + 2]]
        };

        Vector3[] normals =
        {
            originalMesh.normals[submeshTriangles[triangleIndex]],
            originalMesh.normals[submeshTriangles[triangleIndex + 1]],
            originalMesh.normals[submeshTriangles[triangleIndex + 2]]
        };

        Vector2[] uvs =
        {
            originalMesh.uvs[submeshTriangles[triangleIndex]],
            originalMesh.uvs[submeshTriangles[triangleIndex + 1]],
            originalMesh.uvs[submeshTriangles[triangleIndex + 2]]
        };
        
        Triangle t = new Triangle(submesh, verts, normals, uvs);
        return t;
    }
}
