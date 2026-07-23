using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]

//v1.0
//detects, deletes, and scores prizes that land inside of it
//also gmgr for claw game

public class ClawScoring : MonoBehaviour
{
    //attr
    public float gameTime=300;
    int score=0;
    //import
    UIMsg uimsg;
    public TMP_Text timeTx;
    public TMP_Text[] scoreTxs;
    public TMP_Text rewardsTx;
    MiniMenu menu;
    ResourceManager resxmgr;


    //exe
    void Start(){
        uimsg=FindAnyObjectByType<UIMsg>();
        menu=FindAnyObjectByType<MiniMenu>();
        resxmgr=FindAnyObjectByType<ResourceManager>();
        if(resxmgr==null){
            Debug.LogError("Couldnt find Resource Manager!");
        }
        score=0;
        UpdateScoreUI();
    }
    void Update(){
        if(Time.timeScale!=0f){
            if(Time.time>gameTime || Input.GetKeyDown(KeyCode.P)){
                EndGame();
            }else{
                DisplayTime();
            }
            if(Input.GetKeyDown(KeyCode.Escape)){
                menu.ShowMenu(1, true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("entered @ "+gameObject.name+": "+col.gameObject.name);
        ClawPrize prize=col.gameObject.GetComponent<ClawPrize>();
        if(prize!=null){
            AddScore(prize.score);
            uimsg.AddLog("Scored! +"+(prize.score));
            //play sounds etc
            Destroy(prize.gameObject);
        }
    }
    //funx
    void EndGame(){
        Time.timeScale=0f;
        rewardsTx.text="+"+score+" Tickets!";
        menu.ShowMenu(0, true);
        resxmgr.AddResource("Tickets", score);
    }
    void AddScore(int amount){
        score+=amount;
        UpdateScoreUI();
    }
    //helper
    float cTime;
    int mins;
    int secs;
    void DisplayTime(){
        cTime=gameTime-Time.time;
        mins=Mathf.FloorToInt(cTime)/60;
        cTime-=mins*60;
        secs=Mathf.FloorToInt(cTime);
        timeTx.text="Time: ";
        if(mins>=10){
            timeTx.text=timeTx.text+mins;
        }else{
            timeTx.text=timeTx.text+"0"+mins;
        }
        timeTx.text+=":";
        if(secs>=10){
            timeTx.text=timeTx.text+secs;
        }else{
            timeTx.text=timeTx.text+"0"+secs;
        }
    }
    void UpdateScoreUI(){
        foreach (TMP_Text scoreTx in scoreTxs)
        {
            scoreTx.text="Score: "+score;
        }
    }
}
