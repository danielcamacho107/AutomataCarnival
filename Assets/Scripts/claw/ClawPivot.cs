using UnityEngine;

//v1.0
//movable pivot on claw minigame

public class ClawPivot : MonoBehaviour
{
    //attr
    public float movSpeed=2f;
    //import
    Claw claw;
    
    
    
    //exe
    void Start(){
        claw=FindAnyObjectByType<Claw>();
    }
    void Update(){
        if(Time.timeScale!=0f){
            Move();
            Move2();
            FireClaw();
        }
    }
    //funx
    void Move(){
        float movX=Input.GetAxis("Horizontal");
        transform.Translate(movX*Time.deltaTime*movSpeed, 0f, 0f);
    }
    void Move2(){
        float movX=Input.GetAxis("Vertical");
        transform.Translate(movX*Time.deltaTime*(movSpeed/10), 0f, 0f);
    }
    void FireClaw(){
        if(!claw.fired && Input.GetKeyDown(KeyCode.Mouse1) ){
            claw.fired=true;
        }
    }
    //helper
}