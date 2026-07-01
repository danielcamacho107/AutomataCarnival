using UnityEngine;
using System.Collections;

//v1.0
//dismantles and builds an object

public class Interactable_Build : Interactable
{
    //attr
    //import
    public Collider coll;
    public bool building=false;
    bool buildingLock=false;
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;
    public Sprite builtSprite;
    public Sprite construxion;
    public float buildingWait=5f;
    public string buildMsg="Press [E] to build";
    public string construxionMsg="This building is in construction...";
    public string dismantleMsg="Press [E] to dismantle";



    //exe
    protected void Start(){
        buildingLock=false;
        if(building){
            promptMsg=dismantleMsg;
        }else{
            promptMsg=buildMsg;
        }
    }
    //funx
    protected override void OnInteracted(){
        if(coll!=null && !buildingLock){
            buildingLock=true;
            promptMsg=construxionMsg;
            building=!GetComponent<Collider>().enabled;
            if(spriteRenderer!=null){
                spriteRenderer.enabled=true;
                spriteRenderer.sprite=construxion;
            }
            StartCoroutine(BuildOrDismantle());
        }
        
    }
    //helper
    IEnumerator BuildOrDismantle(){
        yield return new WaitForSeconds(buildingWait);
        if(meshRenderer!=null){
            meshRenderer.enabled=building;
        }
        if(spriteRenderer!=null){
            if(building){
                spriteRenderer.sprite=builtSprite;
                promptMsg=dismantleMsg;
            }else{
                spriteRenderer.enabled=false;
                promptMsg=buildMsg;
            }
        }
        coll.enabled=building;
        buildingLock=false;
    }
}