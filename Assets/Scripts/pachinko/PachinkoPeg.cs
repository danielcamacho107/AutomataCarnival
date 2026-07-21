using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]

//v1.0
//desco

public class PachinkoPeg : MonoBehaviour
{
    //attr
    enum PegType{
        None,
        Double,
        Add
    }
    PegType pegtype;
    bool isBouncy;
    //import
    Collider2D coll;
    SpriteRenderer rend;
    public PhysicsMaterial2D bouncy;
    public PhysicsMaterial2D unbouncy;
    PachinkoPlayer player;
    UIMsg uimsg;



    //exe
    void Start(){
        coll=GetComponent<Collider2D>();
        rend=GetComponent<SpriteRenderer>();
        player=FindAnyObjectByType<PachinkoPlayer>();
        uimsg=FindAnyObjectByType<UIMsg>();
    }
    void Update(){
        
    }
    void OnCollisionEnter2D(Collision2D col){
        PachinkoBall ball=col.gameObject.GetComponent<PachinkoBall>();
        if(ball!=null && !ball.repaid){
            switch(pegtype){
                case PegType.Add:
                    player.AddAmmo(1);
                    ball.repaid=true;
                    uimsg.AddLog("+1 ball!");
                    pegtype=PegType.None;
                    UpdatePegAttr();
                    break;
                case PegType.Double:
                    pegtype=PegType.None;
                    UpdatePegAttr();
                    Instantiate(col.gameObject, col.transform.position, transform.rotation);
                    uimsg.AddLog("Ball cloned!");
                    break;
                default:
                    break;
            }
        }
    }
    //funx
    //helper
    public void SetPegActive(bool state){
        SetPegAttr();
        if(coll!=null){
            coll.enabled=state;
            rend.enabled=state;
        }else{
            coll=GetComponent<Collider2D>();
            rend=GetComponent<SpriteRenderer>();
            coll.enabled=state;
            rend.enabled=state;
        }
    }
    void SetPegAttr(){
        float r=1f;
        float g=1f;
        float b=1f;
        switch(Random.Range(0, 3)){
            case 0:
                pegtype=PegType.None;
                break;
            case 1:
                pegtype=PegType.Double;
                r=0f;
                break;
            case 2:
                pegtype=PegType.Add;
                g=0f;
                break;
            default:
                pegtype=PegType.None;
                r=0f;
                g=0f;
                break;
        }
        isBouncy=Random.Range(0,2)==0;
        if(isBouncy){
            b=0f;
            coll.sharedMaterial=bouncy;
        }else{
            coll.sharedMaterial=unbouncy;
        }
        rend.color=new Color(r,g,b);
    }
    void UpdatePegAttr(){
        float r=1f;
        float g=1f;
        float b=1f;
        switch(pegtype){
            case PegType.None:
                break;
            case PegType.Double:
                r=0f;
                break;
            case PegType.Add:
                g=0f;
                break;
            default:
                r=0f;
                g=0f;
                break;
        }
        if(isBouncy){
            b=0f;
            coll.sharedMaterial=bouncy;
        }else{
            coll.sharedMaterial=unbouncy;
        }
        rend.color=new Color(r,g,b);
    }
}
