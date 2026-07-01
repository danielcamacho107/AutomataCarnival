using UnityEngine;
using System.Collections;
using TMPro;

//v1.0
//A countdown that shows up before minigames,
// so that player has time to read instructions

public class Countdown : MonoBehaviour
{
    //attr
    public int countFrom=5;
    int count=5;
    public float countDelay=1f;
    //import
    public TMP_Text countTx;



    //exe
    void Start(){
        CountDown();
    }
    //funx
    //helper
    public void CountDown(){
        count=countFrom;
        Time.timeScale=0f;
        StartCoroutine(StartCountdown());
    }
    public void CountDown(int startFrom){
        count=startFrom;
        Time.timeScale=0f;
        StartCoroutine(StartCountdown());
    }
    IEnumerator StartCountdown(){
        while(count>0){
            countTx.text=""+count;
            yield return new WaitForSecondsRealtime(countDelay);
            count--;
        }
        countTx.text="GO!";
        yield return new WaitForSecondsRealtime(0.2f);
        countTx.text="";
        Time.timeScale=1f;
    }
}
