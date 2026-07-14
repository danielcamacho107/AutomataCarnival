using UnityEngine;
using UnityEngine.UI;
using TMPro;

//v2.0
//player's car in race gamemode

public class PlayerCar : Car
{
    //attr
    [Header("Player Car")]
    public float acceleration=1f;
    public TMP_Text speedTx;
    public RawImage speedImg;
    public Texture2D[] speedSprites;
    int idealLane=0;
    //import
    MiniMenu menu;



    //exe
    void Start(){
        rend=GetComponentInChildren<SpriteRenderer>();
        draft=GetComponentInChildren<Draft>();
        currentSpeed=minSpeed;
        frictionICD=frictionCD;
        idealLane=lane;
        menu=FindAnyObjectByType<MiniMenu>();
        uimsg=FindAnyObjectByType<UIMsg>();
        uimsg.AddLog("Press [Space] or Click [RMB] quickly to accelerate.");
        uimsg.AddLog("Click [LMB] to change lanes.");
        uimsg.AddLog("Press [P] to give up.", 5f);
    }
    void Update(){
        if(Time.timeScale!=0f){
            DecaySpeed();
            PlayerChangeLane();
            Accelerate();
            UpdateSpeedUI();
            Move();
            GiveUp();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(Time.timeScale==0f){
                menu.HideAllMenus();
            }else{
                menu.ShowMenu(1, true);
            }
        }
    }
    //funx
    void Accelerate(){
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){
            currentSpeed+=acceleration;
            //ClampSpeed();
            Prompt();
        }
    }
    public void PlayerChangeLane(){
        if(Input.GetKeyDown(KeyCode.W) && !laneLock /*&& Time.time>changeLaneICD*/){
            idealLane=currentWaypoint.straightLane;
        }else if(Input.GetKeyDown(KeyCode.E) && !laneLock /*&& Time.time>changeLaneICD*/){
            idealLane++;
            if(idealLane>=5){
                idealLane=4;
            }else{
                uimsg.AddLog("Changing lane right!");
            }
        }else if(Input.GetKeyDown(KeyCode.Q) && !laneLock /*&& Time.time>changeLaneICD*/){
            idealLane--;
            if(idealLane<0){
                idealLane=0;
            }else{
                uimsg.AddLog("Changing lane left!");
            }
        }
    }
    public override void ReachWaypoint(Waypoint waypoint){
        laneLock=waypoint.lockLane;
        SpriteRenderer rend=GetComponentInChildren<SpriteRenderer>();
        rend.flipX=waypoint.faceRight;
        currentWaypoint=waypoint;
        Draft draft=GetComponentInChildren<Draft>();
        draft.gameObject.transform.rotation=waypoint.gameObject.transform.rotation;
    }
    new protected void Crash(Car other){
        Debug.Log("Crash! "+gameObject.name+" v "+other.gameObject.name);
        draftScale=0f;
        other.currentSpeed+=currentSpeed;
        currentSpeed=0f;
        crashICD=Time.time+crashCD;
    }
    //helper
    void UpdateSpeedUI(){
        speedTx.text=""+Mathf.FloorToInt(currentSpeed*10f)+" km/h";
        int i=Mathf.FloorToInt(currentSpeed-1);
        if(i<0){
            i=0;
        }else if(i>4){
            i=4;
        }
        speedImg.texture=speedSprites[i];
    }
    void DecaySpeed(){
        if(Time.time>frictionICD){
            frictionICD=Time.time+frictionCD;
            if(currentSpeed>minSpeed){
                currentSpeed*=frictionScale;
            }
            Prompt();
            //ClampSpeed();
        }
    }
    void Prompt(){
        if(currentSpeed<minSpeed+maxSpeed/2){
            uimsg.ReplacePrompt("Tap [Space] or [LMB] quickly to accelerate.");
        }else if(currentSpeed>minSpeed+maxSpeed/2){
            uimsg.ClearPrompt();
        } 
    }
    void GiveUp(){
        if(Input.GetKeyDown(KeyCode.P) && Time.timeScale!=0){
            FinishLine fl = FindAnyObjectByType<FinishLine>();
            fl.OnLose();
        }
    }
    protected override int GetLane(){
        switch(currentWaypoint.nextWaypoints.Length)
        {
            case 1:
                lane=0;
                break;
            case 2:
                if(idealLane<3){
                    lane=0;
                }else{
                    lane=1;
                }
                break;
            case 3:
                if(idealLane<2){
                    lane=0;
                }else if(idealLane==2){
                    lane=1;
                }else{
                    lane=2;
                }
                break;
            case 4:
                if(idealLane==0){
                    lane=0;
                }else if(idealLane<=2){
                    lane=1;
                }else if(idealLane==3){
                    lane=2;
                }else if(idealLane>3){
                    lane=3;
                }
                break;
            case 5:
                lane=idealLane;
                break;
            default:
                Debug.LogError("PlayerCar: GetLane: Lane error");
                lane=0;
                break;
        }
        return lane;
    }
}
