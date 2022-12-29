using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor{
    MeshGenerator meshGenerator;
    

    public override void OnInspectorGUI()
    {
        if (!meshGenerator) meshGenerator = (MeshGenerator)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Rectangle")) meshGenerator.GenerateRectangle();
        if (GUILayout.Button("Generate Plane")) meshGenerator.GeneratePlane();
        if (GUILayout.Button("Generate Cube")) meshGenerator.GenerateCube();
        if (GUILayout.Button("Generate Voxel")) meshGenerator.GenerateVoxel(meshGenerator.voxelType);
    }

    void OnSceneGUI(){
        if (!meshGenerator) meshGenerator = (MeshGenerator)target;

        for(int i = 0; i < meshGenerator.vertices.Count; i++){
            Handles.Label(meshGenerator.vertices[i], $"{i}");
        }
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    [HideInInspector] public List<Vector3> vertices;
    [HideInInspector] public List<int> triangles;
    [HideInInspector] public List<Vector2> uvs;

    public float rectangleWidth, rectangleHeight;
    public int planeWidth = 5;
    
    public Voxel.Type voxelType;
    

    public void GenerateRectangle(){
        meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>();
        vertices.Add(new Vector3(0,0,0));
        vertices.Add(new Vector3(rectangleWidth,0,0));
        vertices.Add(new Vector3(rectangleWidth,0,rectangleHeight));
        vertices.Add(new Vector3(0,0,rectangleHeight));
        int[] triangles = {0, 2, 1, 3, 2, 0};  
        Vector2[] uvs = {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    public void GeneratePlane(){
        meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        int width = planeWidth;

        for (int z = 0; z <= width; z++){
            for (int x = 0; x <= width; x++){
                vertices.Add(new Vector3(x, 0f, z));
                uvs.Add(new Vector2((float)x / (float)width, (float)z / (float)width));
            }
        }

        // r = row
        // c = column
        for (int r = 0; r < width; r++){
            for (int c = 0; c < width; c++){
                // s = start vert index
                int s = c + (width + 1) * r;
                triangles.AddRange(new int[]{
                    s,
                    s + (width + 2),
                    s + 1,
                    s,
                    s + (width + 1),
                    s + (width + 2),
                });
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    public void GenerateCube(){
        meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        // Top verts
        float x = 1f / 6f;
        vertices.AddRange(new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0,0),
            new Vector2(0, x),
            new Vector2(x, x),
            new Vector2(x, 0)
        });
        // Bottom Verts
        vertices.AddRange(new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0, x * 1),
            new Vector2(0, x * 2),
            new Vector2(x, x * 2),
            new Vector2(x, x * 1)
        });
        // Front
        vertices.AddRange(new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0, x * 2),
            new Vector2(0, x * 3),
            new Vector2(x, x * 3),
            new Vector2(x, x * 2)
        });
        // Right
        vertices.AddRange(new Vector3[] {
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0, x * 3),
            new Vector2(0, x * 4),
            new Vector2(x, x * 4),
            new Vector2(x, x * 3)
        });
        // Back
        vertices.AddRange(new Vector3[] {
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 1)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0, x * 4),
            new Vector2(0, x * 5),
            new Vector2(x, x * 5),
            new Vector2(x, x * 4)
        });
        // Left
        vertices.AddRange(new Vector3[] {
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0)
        });
        uvs.AddRange(new Vector2[]{
            new Vector2(0, x * 5),
            new Vector2(0, x * 6),
            new Vector2(x, x * 6),
            new Vector2(x, x * 5)
        });

        int faceCount = 6;
        int h = vertices.Count - 4 * faceCount;
        for (int i = 0; i < faceCount; i++){
            triangles.AddRange(new int[]{
                h + i * 4,
                h + i * 4 + 1,
                h + i * 4 + 2,
                h + i * 4,
                h + i * 4 + 2,
                h + i * 4 + 3
            });
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    public void GenerateVoxel(Voxel.Type voxelType){
        meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        vertices.AddRange(Voxel.topVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Top));

        vertices.AddRange(Voxel.bottomVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Bottom));

        vertices.AddRange(Voxel.frontVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));
        
        vertices.AddRange(Voxel.rightVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));

        vertices.AddRange(Voxel.backVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));

        vertices.AddRange(Voxel.leftVerts);
        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));

        int faceCount = 6;
        int h = vertices.Count - 4 * faceCount;
        for (int i = 0; i < faceCount; i++){
            triangles.AddRange(new int[]{
                h + i * 4,
                h + i * 4 + 1,
                h + i * 4 + 2,
                h + i * 4,
                h + i * 4 + 2,
                h + i * 4 + 3
            });
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
    
}
