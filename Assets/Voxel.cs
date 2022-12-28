using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    const float resolution = 16f;
    static float res{
        get => (1f / resolution);
    }

    public enum Type{
        Air, Dirt, Grass, Stone, Trunk, Leaves
    }

    public enum Face{
        Top, Side, Bottom
    }

    Dictionary<Face, int[]> uvs;
    

    public Voxel(int[] top, int[] side, int[] bottom){
        uvs = new Dictionary<Face, int[]>(){
            {Face.Top, top},
            {Face.Side, side},
            {Face.Bottom, bottom}
        };
    }

    public static Dictionary<Voxel.Type, Voxel> voxels = new Dictionary<Type, Voxel>(){
        {Voxel.Type.Dirt, new Voxel(new int[]{0,0}, new int[]{0,0}, new int[]{0,0})},
        {Voxel.Type.Grass, new Voxel(new int[]{1,0}, new int[]{0,1}, new int[]{0,0})},
        {Voxel.Type.Stone, new Voxel(new int[]{0,2}, new int[]{0,2}, new int[]{0,2})},
        {Voxel.Type.Trunk, new Voxel(new int[]{0,3}, new int[]{0,4}, new int[]{0,3})},
        {Voxel.Type.Leaves, new Voxel(new int[]{0,5}, new int[]{0,5}, new int[]{0,5})},
    };

    public static Vector2[] GetUVs(int x, int y){
        Vector2[] uvs = new Vector2[]{
            new Vector2((float)x * res, (float)y * res),
            new Vector2((float)x * res, (float)(y + 1) * res),
            new Vector2((float)(x + 1) * res, (float)(y + 1) * res),
            new Vector2((float)(x + 1) * res, (float)(y) * res),
        };
        return uvs;
    }

    public static Vector2[] GetUVs(Voxel.Type voxelType, Voxel.Face voxelFace){
        int[] coordinates = voxels[voxelType].uvs[voxelFace];
        return GetUVs(coordinates[0], coordinates[1]);
    }


    #region Vertices
    public static Vector3[] topVerts
    {
        get => new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0)
        };
    }
    public static Vector3[] bottomVerts
    {
        get => new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };
    }
    public static Vector3[] frontVerts
    {
        get => new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)
        };
    }
    public static Vector3[] rightVerts
    {
        get => new Vector3[] {
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1)
        };
    }
    public static Vector3[] backVerts
    {
        get => new Vector3[] {
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 1)
        };
    }
    public static Vector3[] leftVerts
    {
        get => new Vector3[] {
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0)
        };
    }
    #endregion
}
