using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

//v1.0
//claw prizes

public class ClawPrize : MonoBehaviour
{
    //attr
    public int score=10;
    //import
    Rigidbody2D rb;
    Collider2D coll;
    
    
    
    //exe
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        //coll=GetComponent<Collider2D>();
    }
    //funx
    public void Grab(Claw claw){
        claw.grabbedPrizes.Add(this);
        gameObject.transform.parent=claw.gameObject.transform;
        rb.gravityScale=0f;
        rb.linearVelocity=Vector2.zero;
        //coll.enabled=false;
    }
    public void Release(){
        gameObject.transform.parent=null;
        rb.gravityScale=1f;
        //coll.enabled=true;
    }
    //helper
	
}