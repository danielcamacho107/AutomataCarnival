using UnityEngine;
using System.Collections;
using TMPro;

//v1.0
//Prints messages to UI, optionally clear them after a set time

//Set two textboxes, one for logs(timed messages) and one for prompts (permanent messages)

//Stick this script to canvas

public class UIMsg : MonoBehaviour
{
    //attr
    public float defaultLifetime=3f;
    //import
    public TMP_Text logTx;
    public TMP_Text promptTx;
    


    //exe
    void Start()
    {
        ClearLog();
        ClearPrompt();
    }
    //funx
    public void ClearLog(){
        logTx.text="";
    }
    public void ClearPrompt(){
    	promptTx.text="";
    }
    public void AddLog(string msg){
        if(msg!="(none)"){
            ResetTimer(defaultLifetime);
    	    logTx.text=logTx.text+"\n"+msg;
        }
    }
    public void AddLog(string msg, float lifetime){
        if(msg!="(none)"){
            ResetTimer(lifetime);
    	    logTx.text=logTx.text+"\n"+msg;
        }
    }
    public void AddPrompt(string msg){
        if(msg!="(none)"){
            promptTx.text=promptTx.text+"\n"+msg;
        }
    }
    public void ReplaceLog(string msg){
        if(msg!="(none)"){
            ResetTimer(defaultLifetime);
    	    logTx.text=msg;
        }
    }
    public void ReplaceLog(string msg, float lifetime){
        if(msg!="(none)"){
            ResetTimer(lifetime);
    	    logTx.text=msg;
        }
    }
    public void ReplacePrompt(string msg){
        if(msg!="(none)"){
            promptTx.text=msg;
        }
    }
    //helper
    void ResetTimer(float newTime){
    	StopAllCoroutines();
    	StartCoroutine(TimedClear(newTime));
    }
    IEnumerator TimedClear(float waittime){
    	yield return new WaitForSeconds(waittime);
    	ClearLog();
    }
}