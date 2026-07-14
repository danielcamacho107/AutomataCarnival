using UnityEngine;
using TMPro;

//v1.0
//the player in pachinko minigame, can move L/R and drop balls

public class PachinkoPlayer : MonoBehaviour
{
    //attr
    public float moveSpeed=1f;
    public int maxAmmo=5;
    int ammo;
    public GameObject ball;
    public float placeBallCD=0.5f;
    float placeBallICD=0f;
    public Transform firepoint;
    //import
    PachinkoGame gmgr;
    MiniMenu menu;
    public TMP_Text ammoTx;



    //exe
    void Start(){
        ammo=maxAmmo;
        placeBallICD=0f;
        menu=FindAnyObjectByType<MiniMenu>();
        gmgr=FindAnyObjectByType<PachinkoGame>();
        UpdateAmmoUI();
    }
    void Update(){
        Move();
        Move2();
        DropBall();
        Pause();
        GiveUp();
    }
    //funx
    void DropBall(){
        if( (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Mouse0)) && Time.time>placeBallICD){
            if(ammo>0){
                placeBallICD=Time.time+placeBallCD;
                ammo--;
                UpdateAmmoUI();
                Instantiate(ball, firepoint.position, firepoint.rotation);
            }else{
                gmgr.OnGameEnd();
            }
        }
    }
    void Move(){
        float movX=Input.GetAxis("Horizontal");
        Vector3 axis=new Vector3(-movX, 0f, 0f);
        transform.Translate(axis.normalized*moveSpeed*Time.deltaTime);
    }
    void Move2(){
        float movX=Input.GetAxis("Vertical");
        Vector3 axis=new Vector3(-movX, 0f, 0f);
        transform.Translate(axis.normalized*(moveSpeed/10f)*Time.deltaTime);
    }
    void Pause(){
        if(Time.timeScale!=0f && Input.GetKeyDown(KeyCode.Escape)){
            menu.ShowMenu(1, true);
        }
    }
    void GiveUp(){
        if(Time.timeScale!=0f && Input.GetKeyDown(KeyCode.P)){
            gmgr.OnGameEnd();
        }
    }
    //helper
    void UpdateAmmoUI(){
        ammoTx.text="Balls: "+ammo+"/"+maxAmmo;
    }
    public void AddAmmo(int amount){
        ammo+=amount;
        UpdateAmmoUI();
    }
}
