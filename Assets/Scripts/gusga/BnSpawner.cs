using UnityEngine;

//v1.0
//spawn GOs from a bn

public class BnSpawner : MonoBehaviour
{
    //attr
    public GameObject[] pool;
    public bool spawnInCanvas=false;
    public GameObject canvas;
    public Vector3 offset;
    public GameObject savedGO;
    //import



    //exe
    //funx
    public void SpawnGO(){
        if(spawnInCanvas){
            Instantiate(pool[Random.Range(0, pool.Length)], (canvas.transform.position+offset), Quaternion.identity, canvas.transform);
        }else{
            Instantiate(pool[Random.Range(0, pool.Length)], offset, Quaternion.identity);
        }
    }
    public void SaveSpawnGO(){
        if(spawnInCanvas){
            savedGO=Instantiate(pool[Random.Range(0, pool.Length)], (canvas.transform.position+offset), Quaternion.identity, canvas.transform);
        }else{
            savedGO=Instantiate(pool[Random.Range(0, pool.Length)], offset, Quaternion.identity);
        }
    }
    //helper
}
