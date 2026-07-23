using UnityEngine;
using System.Collections.Generic;

//v1.0
//the waitlist's manager

public class GusgeriaWaitListMgr : MonoBehaviour
{
    //attr
    struct CsmerNode{
        public int dish;
        public int drink;
        public bool[] spices;
        public float endTime;
        public Sprite img;
        public Color color;
    }
    List<CsmerNode> waitlist=new List<CsmerNode>();
    public GusgeriaWaitBn[] waitBns;
    //import



    //exe
    void Start(){
        HideBns();
    }
    void Update(){
        
    }
    //funx
    void HideBns(){
        foreach(GusgeriaWaitBn waitBn in waitBns){
            waitBn.gameObject.SetActive(false);
        }
    }
    public void AddCsmer(int newDish, int newDrink, bool[] newSpices, float endTime, Sprite newImg, Color newColor){
        //add to list
        CsmerNode newCsmer;
        newCsmer.dish=newDish;
        newCsmer.drink=newDrink;
        newCsmer.spices=newSpices;
        newCsmer.endTime=endTime;
        newCsmer.img=newImg;
        newCsmer.color=newColor;
        waitlist.Add(newCsmer);
        AssignBn(newCsmer);
    }
    public void ChainShift(GusgeriaWaitBn gwbn){
        //find csmer with same data as bn and pop him
        for(int i=0; i<waitlist.Count; i++){
            if(CompareSame(gwbn, waitlist[i])){
                waitlist.RemoveAt(i);
                break;
            }
        }
        //deactivate bn to mark as available
        gwbn.gameObject.SetActive(false);

        //if anyone was waiting add them to newly available bn
        int count=waitlist.Count;
        for(int i=0; i<count; i++){
            //also pop anyone w an expired timer
            if(waitlist[i].endTime<Time.time){
                waitlist.RemoveAt(i);
                i--;
                count--;
            }else{
                //if not already in a bn, add him
                for(int j=0; j<waitBns.Length; j++){
                    if( !(waitBns[j].gameObject.activeSelf && (CompareSame(waitBns[j], waitlist[i])) ) ){
                        AssignBn(waitlist[i]);
                    }
                }
            }
        }
    }
    void AssignBn(CsmerNode newCsmer){
        //check if a space is free
        for(int i=0; i<waitBns.Length; i++){
            if(!waitBns[i].gameObject.activeSelf){
                //available
                //if so cast to bn
                waitBns[i].gameObject.SetActive(true);
                waitBns[i].SetData(newCsmer.dish, newCsmer.drink, newCsmer.spices, newCsmer.endTime-Time.time, newCsmer.img, newCsmer.color);
                break;
            }
        }
    }
    bool CompareSame(GusgeriaWaitBn gwbn, CsmerNode csmer){
        if(gwbn.dish!=csmer.dish || gwbn.drink!=csmer.drink || gwbn.iconImg.sprite!=csmer.img){
            return false;
        }else{
            for(int i=0; i<gwbn.spices.Length; i++){
                if(gwbn.spices[i]!=csmer.spices[i]){
                    return false;
                }
            }
            return true;
        }
        
    }
    //helper
}
