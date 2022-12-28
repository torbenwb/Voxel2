using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    public const int width = 16;
    public const int height = 64;
    
    public Vector3Int chunkCell;
    public Voxel.Type[,,] blocks = new Voxel.Type[width,height,width];

    public static Dictionary<Vector3Int, Chunk> allChunks = new Dictionary<Vector3Int, Chunk>();

    public static void TryRebuildMesh(Vector3Int targetCell){
        if (!allChunks.ContainsKey(targetCell)) return;
        allChunks[targetCell].BuildMesh();
    }

    public Dictionary<Vector3Int, Voxel.Type> deltas = new Dictionary<Vector3Int, Voxel.Type>();

    private void Awake()
    {
        chunkCell = new Vector3Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(transform.position.z)
        );

        allChunks.Add(chunkCell, this);

        GenerateTest();
    }

    private void Start()
    {
        BuildMesh();
    }

    void GenerateTest(){
        for(int x = 0; x < width; x++)
            for(int z = 0; z < width; z++)
                blocks[x,0,z] = Voxel.Type.Dirt;
    }

    static void GlobalToLocal(ref Vector3Int targetCell, ref int x, ref int y, ref int z){
        int tempX = x / width;
        int tempZ = z / width;
        if (x < 0) tempX -= 1;
        if (z < 0) tempZ -= 1;
        
        targetCell.x = tempX * width;
        targetCell.y = 0;
        targetCell.z = tempZ * width;

        x -= targetCell.x;
        z -= targetCell.z;
    }

    public static void SetVoxelType(int x, int y, int z, Voxel.Type newVoxelType){
        Vector3Int targetCell = Vector3Int.zero;
        GlobalToLocal(ref targetCell,ref x,ref y,ref z);
        if (allChunks.ContainsKey(targetCell)){
            if (allChunks[targetCell].deltas.ContainsKey(new Vector3Int(x,y,z))){
                allChunks[targetCell].deltas[new Vector3Int(x,y,z)] = newVoxelType;
            }
            else{
                allChunks[targetCell].deltas.Add(new Vector3Int(x,y,z), newVoxelType);
            }
            

            TryRebuildMesh(targetCell);

            if (x == 0) TryRebuildMesh(targetCell + new Vector3Int(-width,0,0));
            if (x == width - 1) TryRebuildMesh(targetCell + new Vector3Int(width,0,0));
            if (z == 0) TryRebuildMesh(targetCell + new Vector3Int(0,0,-width));
            if (z == width - 1) TryRebuildMesh(targetCell + new Vector3Int(0,0,width));
        }
    }

    Voxel.Type GetVoxelType(int x, int y, int z){
        Vector3Int targetCell = chunkCell; 
        if (x < 0) {
            targetCell.x -= width;
            x += width;
        }
        else if (x >= width){
            targetCell.x += width;
            x -= width;
        }
        if (z < 0){
            targetCell.z -= width;
            z += width;
        }
        else if (z >= width){
            targetCell.z += width;
            z -= width;
        }

        if (!allChunks.ContainsKey(targetCell)) return Voxel.Type.Air;
        if (allChunks[targetCell].deltas.ContainsKey(new Vector3Int(x,y,z))) return allChunks[targetCell].deltas[new Vector3Int(x,y,z)];
        return allChunks[targetCell].blocks[x, y, z];
    }

    void BuildMesh(){
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int x = 0; x <= width; x++)
            for(int z = 0; z <= width; z++)
                for(int y = 0; y < height; y++)
                {
                    Voxel.Type voxelType = GetVoxelType(x,y,z);
                    if (voxelType == Voxel.Type.Air) continue;

                    Vector3 blockPosition = new Vector3(x,y,z);
                    blockPosition -= Vector3.one * 0.5f;
                    int faceCount = 0;

                    // Top 
                    if (y < height - 1 && GetVoxelType(x, y + 1, z) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.topVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType,Voxel.Face.Top));
                    }

                    // Bottom
                    if (y > 0 && GetVoxelType(x,y - 1, z) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.bottomVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Bottom));
                    }

                    // Front
                    if (GetVoxelType(x, y, z - 1) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.frontVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));
                    }

                    // Right
                    if (GetVoxelType(x + 1, y, z) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.rightVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));
                    }

                    // Back
                    if (GetVoxelType(x, y, z + 1) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.backVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));
                    }

                    // Left
                    if (GetVoxelType(x - 1, y, z) == Voxel.Type.Air){
                        foreach(Vector3 v in Voxel.leftVerts) vertices.Add(blockPosition + v);
                        faceCount++;
                        uvs.AddRange(Voxel.GetUVs(voxelType, Voxel.Face.Side));
                    }

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
                }
    
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
