using UnityEngine;

//v2.0
//a car in race gamemode

public class Car : MonoBehaviour
{
    //attr
    [Header("Speed")]
    //initial+current speed
    public float maxSpeed=5f;
    public float minSpeed=3f;
    protected float speed=0f;
    public float currentSpeed=0f;
    //friction/inertia
    public float frictionCD=0.1f;
    protected float frictionICD=0f;
    public float frictionScale=0.1f;
    //draft
    public float draftCD=0.1f;
    protected float draftICD=0f;
    public float draftScale=0f;
    public float iDraftScale=0.2f; //threshold/increment
    //crash
    public float crashCD=2f;
    protected float crashICD=0f;
    [Header("Lanes")]
    //following next waypoint
    public Waypoint currentWaypoint;
    public int nextWaypoint;
    //random change lane
    public bool laneLock=false;
    public float maxImpulsivity=75f;
    public float minImpulsivity=25f;
    protected float impulsivity=50f;
    public float changeLaneCD=2f;
    protected float changeLaneICD=0f;
    //import
    protected UIMsg uimsg;



    //exe
    void Start(){
        frictionICD=0f;
        draftICD=0f;
        changeLaneICD=changeLaneCD;
        speed=Random.Range(maxSpeed, minSpeed);
        impulsivity=Random.Range(maxImpulsivity, minImpulsivity);
        uimsg=FindAnyObjectByType<UIMsg>();
    }
    void Update(){
        if(Time.time>crashICD){
            SoftClampSpeed();
            Move();
        }
    }
    //crash
    void OnCollisionEnter(Collision col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            if(draftScale>0f){
                Crash(car);
            }
        }
    }
    protected void Crash(Car other){
        Debug.Log("Crash! "+gameObject.name+" v "+other.gameObject.name);
        uimsg.AddLog("Crash! "+gameObject.name+" v "+other.gameObject.name);
        draftScale=0f;
        other.currentSpeed+=currentSpeed;
        currentSpeed=0f;
        crashICD=Time.time+crashCD;
        ChangeLane();
    }
    public void Crash(){
        Debug.Log("Crash! "+gameObject.name);
        uimsg.AddLog("Crash! "+gameObject.name);
        draftScale=0f;
        currentSpeed=0f;
        crashICD=Time.time+crashCD;
        ChangeLane();
    }
    //funx
    protected void Move(){
        Debug.LogWarning(""+gameObject.name+" to "+currentWaypoint.name+"["+nextWaypoint+"]");
        transform.position = Vector3.MoveTowards(transform.position,
            currentWaypoint.nextWaypoints[nextWaypoint].gameObject.transform.position,
            (currentSpeed+draftScale)*Time.deltaTime);
    }
    public void ChangeLane(){
        nextWaypoint=0;//Random.Range(1, currentWaypoint.nextWaypoints.Length);
    }
    public virtual void ReachWaypoint(Waypoint waypoint){
        laneLock=waypoint.lockLane;
        SpriteRenderer rend=GetComponentInChildren<SpriteRenderer>();
        rend.flipX=waypoint.faceRight;
        if(Time.time>changeLaneICD && Random.Range(0f,100f)<=impulsivity){
            nextWaypoint=Random.Range(1, 2);
        }else{
            nextWaypoint=0;
        }
        currentWaypoint=waypoint;
        Draft draft=GetComponentInChildren<Draft>();
        draft.gameObject.transform.rotation=waypoint.gameObject.transform.rotation;
    }
    //helper
    public void Draft(){
        if(Time.time>draftICD){
            draftICD=Time.time+draftCD;
            if(draftScale<iDraftScale){
                draftScale+=iDraftScale;
            }else{
                draftScale+=draftScale;
            }
        }
    }
    protected void SoftClampSpeed(){
        if(Time.time>frictionICD){
            frictionICD=Time.time+frictionCD;
            if(currentSpeed>speed){
                currentSpeed-=frictionScale;
            }else if(currentSpeed<speed){
                currentSpeed+=frictionScale;
            }
        }
    }
    protected void ClampSpeed(){
        if(currentSpeed<minSpeed){
            currentSpeed=minSpeed;
        }else if(currentSpeed>maxSpeed){
            currentSpeed=maxSpeed;
        }
    }
}
