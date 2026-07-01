using UnityEngine;

//v1.0
//example interactable class, this one disappears and reappears an object

public class Interactable_Disappear : Interactable
{
    //attr
    //import
    public Collider coll;
    public MeshRenderer meshRenderer;
    public SpriteRenderer spriteRenderer;



    //exe
    //funx
    protected override void OnInteracted(){
        if(coll!=null){
            coll.enabled=!GetComponent<Collider>().enabled;
        }
        if(meshRenderer!=null){
            meshRenderer.enabled=!meshRenderer.enabled;
        }
        if(spriteRenderer!=null){
            spriteRenderer.enabled=!spriteRenderer.enabled;
        }
    }
    //helper
}