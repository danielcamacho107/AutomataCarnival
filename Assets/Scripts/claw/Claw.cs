using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]

//v1.0
//claw in claw minigame

public class Claw : MonoBehaviour
{
    //attr
    [Header("Grab")]
    public float movSpeed=1f;
    bool descend=true;
    [HideInInspector] public bool fired=false;
    bool isOpen=true;
    Vector3 originalPos;
    [HideInInspector] public List<ClawPrize> grabbedPrizes=new List<ClawPrize>();
    bool imput=false;
    //import
    SpriteRenderer rend;
    public Sprite openClaw;
    public Sprite closedClaw;
    public Collider2D trigger;
    UIMsg uimsg;
    
    
    
    //exe
    void Start(){
        originalPos=transform.position;
        rend=GetComponent<SpriteRenderer>();
        uimsg=FindAnyObjectByType<UIMsg>();
        fired=false;
        imput=false;
        SetClawOpen(true);
    }
    void Update(){
        if(Time.timeScale!=0f){
            if(fired){
                Move(descend);
                ToggleClaw();
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        ClawPrize prize=col.gameObject.GetComponent<ClawPrize>();
        if(prize!=null || col.gameObject.CompareTag("Finish")){
            descend=false;
        }
    }
    void OnTriggerStay2D(Collider2D col){
        ClawPrize prize=col.gameObject.GetComponent<ClawPrize>();
        if(imput && prize!=null){
            prize.Grab(this);
        }
    }
    /*void OnTriggerExit2D(Collider2D col){
        ClawPrize prize=col.gameObject.GetComponent<ClawPrize>();
        if(prize!=null){
            prize.Release();
        }
    }*/
    
    //funx
    void Move(bool down){
        float mov=movSpeed*Time.deltaTime;
        if(down){
            mov*=-1f;
        }else if(transform.position.y>=originalPos.y){
            descend=true;
            fired=false;
            SetClawOpen(true);
        }
        transform.Translate(0f, mov, 0f);
    }
    void ToggleClaw(){
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){
            imput=true;
            StartCoroutine(ToggleClawCR());
        }
    }
    //helper
    void SetClawOpen(bool open){
        if(open){
            rend.sprite=openClaw;
            foreach (ClawPrize prize in grabbedPrizes){
                prize.Release();
            }
            grabbedPrizes.Clear();
            trigger.enabled=true;
            uimsg.ReplacePrompt("Trigger ON");
            isOpen=true;
        }else{
            rend.sprite=closedClaw;
            trigger.enabled=false;
            uimsg.ReplacePrompt("Trigger OFF");
            isOpen=false;
        }
    }
    IEnumerator ToggleClawCR(){
        yield return new WaitForSeconds(0.1f);
        isOpen=!isOpen;
        SetClawOpen(isOpen);
        imput=false;
    }
}