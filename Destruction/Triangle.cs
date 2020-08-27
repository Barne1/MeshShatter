using UnityEngine;

public struct Triangle
{
    public const int arraySize = 3;

    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] uvs;
    public int submesh;

    public Triangle(int submesh, Vector3[] vertices, Vector3[] normals, Vector2[] uvs)
    {
        this.vertices = new Vector3[arraySize];
        this.normals = new Vector3[arraySize];
        this.uvs = new Vector2[arraySize];
        this.submesh = submesh;

        for (int i = 0; i < arraySize; i++)
        {
            this.vertices[i] = vertices[i];
            this.normals[i] = normals[i];
            this.uvs[i] = uvs[i];
        }
    }
}
