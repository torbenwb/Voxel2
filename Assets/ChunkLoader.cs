using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int areaRadius = 3;
    List<Chunk> chunks;

    private void Start()
    {
        StartCoroutine(InstantiateChunks());
    }

    IEnumerator InstantiateChunks(){
        chunks = new List<Chunk>();
        for(int x = -areaRadius; x <= areaRadius; x++){
            for(int y = -areaRadius; y <= areaRadius; y++){
                GameObject newChunk = Instantiate(chunkPrefab,new Vector3(x * Chunk.width, 0, y * Chunk.width),Quaternion.identity);
                newChunk.transform.SetParent(transform);
                Chunk c = newChunk.GetComponent<Chunk>();
                chunks.Add(c);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

}
