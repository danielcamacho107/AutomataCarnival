using UnityEngine;
using System.Collections;
using TMPro;

//v1.0
//desco

public class GusgeriaGlobalTimer : MonoBehaviour
{
    //attr
    public float waitTime=600f;
    float secs;
    float mins;
    //import
    public TMP_Text timerTx;



    //exe
    void Start(){
        StartCoroutine(BeginTimer());
    }
    //funx
    void UpdateTimerUI(){
        mins=Mathf.FloorToInt(waitTime/60f);
        secs=Mathf.FloorToInt(waitTime-mins*60f);
        if(mins<10){
            timerTx.text="0"+mins+":";
        }else{
            timerTx.text=""+mins+":";
        }
        if(secs<10){
            timerTx.text+="0"+secs;
        }else{
            timerTx.text+=""+secs;
        }
    }
    IEnumerator BeginTimer(){
        UpdateTimerUI();
        while(waitTime>0f){
            yield return new WaitForSeconds(1f);
            waitTime-=1f;
            UpdateTimerUI();
        }
        GusgeriaWaitListMgr gwlmgr=FindAnyObjectByType<GusgeriaWaitListMgr>();
        gwlmgr.OnGameEnd();
        //call end game
    }
    //helper
}
