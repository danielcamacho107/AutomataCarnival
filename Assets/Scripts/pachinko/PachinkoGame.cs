using UnityEngine;
using TMPro;

//v1.0
//desco

public class PachinkoGame : MonoBehaviour
{
    //attr
    int score=0;
    public int ticketsPerScore=10;
    //import
    MiniMenu menu;
    public TMP_Text[] scoreTxs;
    public TMP_Text rewardsTx;
    ResourceManager resxmgr;
    PegSpawner pegmgr;



    //exe
    void Start(){
        score=0;
        resxmgr=FindAnyObjectByType<ResourceManager>();
        pegmgr=FindAnyObjectByType<PegSpawner>();
        menu=FindAnyObjectByType<MiniMenu>();
        menu.HideAllMenus();
        UpdateScoreUI();
        pegmgr.SpawnPegs();
    }
    //funx
    public void AddScore(int amount){
        score+=amount;
        UpdateScoreUI();
    }
    public void OnGameEnd(){
        UpdateScoreUI();
        Debug.Log("Out of balls!");
        rewardsTx.text="tickets +"+(ticketsPerScore*score)+"!";
        if(resxmgr!=null){
            resxmgr.AddResource("Tickets", ticketsPerScore*score);
        }
        menu.ShowMenu(0, true);
    }
    //helper
    void UpdateScoreUI(){
        foreach(TMP_Text scoreTx in scoreTxs){
            scoreTx.text="Score: "+score;
        }
    }
}
