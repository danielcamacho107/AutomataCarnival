using UnityEngine;

[RequireComponent(typeof(Collider2D))]

//v1.0
//detects, deletes, and scores balls that land inside of it

public class ScoringArea : MonoBehaviour
{
    //attr
    public int score=10;
    //import
    UIMsg uimsg;

    //exe
    void Start(){
        
        uimsg=FindAnyObjectByType<UIMsg>();
    }
    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("entered @ "+gameObject.name+": "+col.gameObject.name);
        PachinkoBall ball=col.gameObject.GetComponent<PachinkoBall>();
        if(ball!=null){
            PachinkoGame gmgr=FindAnyObjectByType<PachinkoGame>();
            gmgr.AddScore(ball.score+score);
            uimsg.AddLog("Scored! +"+(ball.score+score));
            //play sounds etc
            Destroy(ball.gameObject);
        }
    }
    //funx
    //helper

}
