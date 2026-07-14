using UnityEngine;

//v1.0
//spawns pegs in pachinko

public class PegSpawner : MonoBehaviour
{
    //attr
    bool cPegEven=true;
    [Range(0f, 100f)] public float skipPegChance=25f;
    //import
    public PachinkoPeg[] pegs;
    public int[] pegRowCounts;



    //exe
    void Start(){
        ValidatePegs();
    }
    void Update(){
        
    }
    //funx
    public void SpawnPegs(){
        //start even😢 — or odd😊 ‽‽‽
        cPegEven=Random.Range(0, 2)==0;
        foreach(PachinkoPeg peg in pegs){
            //activate odd pegs that are also higher than the set chance🥶
            peg.SetPegActive(!cPegEven && Random.Range(0f, 100f)>skipPegChance);
            //in any case flip the even pegs to create room💀
            cPegEven=!cPegEven;
        }
    }
    //helper
    void ValidatePegs(){
        int totalPegs=0;
        int totalCounts=0;
        foreach(PachinkoPeg peg in pegs){
            totalPegs++;
        }
        foreach(int count in pegRowCounts){
            totalCounts+=count;
        }
        if(totalPegs!=totalCounts){
            Debug.LogError("Peg count mismatch!");
        }
    }
}
