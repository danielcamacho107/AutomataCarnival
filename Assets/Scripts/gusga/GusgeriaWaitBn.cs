using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

//v1.0
//the buttons representing waiting csmers

public class GusgeriaWaitBn : MonoBehaviour
{
    //attr
    [HideInInspector] public int dish=0;
    [HideInInspector] public int drink=0;
    [HideInInspector] public bool[] spices=new bool[12];
    float waitTime=0f;
    int secs;
    int mins;
    //import
    public TMP_Text timerTx;
    public Image iconImg;
    GusgeriaWaitListMgr gwlmgr;



    //exe
    void Start(){
        gwlmgr=FindAnyObjectByType<GusgeriaWaitListMgr>();
    }
    //funx
    //helper
    public void SetData(int newDish, int newDrink, bool[] newSpices, float newTime, Sprite newImg, Color newColor){
        dish=newDish;
        drink=newDrink;
        spices=newSpices;
        waitTime=newTime;
        iconImg.sprite=newImg;
        iconImg.color=newColor;
        StartCoroutine(BeginTimer());
    }
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
        gwlmgr.ChainShift(this);
        //hidebn or call chain shift
    }
    public void CheckOrder(){
        gwlmgr.TallyPoints(this);
        gwlmgr.ChainShift(this);
        //find what is on the scene
        //foreach order break it up into single orders
        //foreach order break it up into components
        //foreach component check if match, add a point
        //calculate points and cast to scoremgr
        //hidebn or call chain shift
    }
}
