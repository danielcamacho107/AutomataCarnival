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
    GusgeriaUI gsui;



    //exe
    void Start(){
        gsui=FindAnyObjectByType<GusgeriaUI>();
    }
    //funx
    public void SpawnGO(){
        if(spawnInCanvas){
            Instantiate(pool[Random.Range(0, pool.Length)], (canvas.transform.position+offset), Quaternion.identity, canvas.transform);
        }else{
            Instantiate(pool[Random.Range(0, pool.Length)], offset, Quaternion.identity);
        }
    }
    public void SpawnGOAt(){
        savedGO=Instantiate(pool[Random.Range(0, pool.Length)], (canvas.transform.position+offset), Quaternion.identity);
        switch(gsui.currentArea){
            case 0:
                gsui.GOsInCounter.Add(savedGO);
                break;
            case 1:
                gsui.GOsInKitchen.Add(savedGO);
                break;
            case 2:
                gsui.GOsInFurnace.Add(savedGO);
                break;
            default:
                break;
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
