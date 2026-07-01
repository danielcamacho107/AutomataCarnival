using UnityEngine;
[RequireComponent(typeof(Collider))]

//v1.0
//parent interactable class

public abstract class Interactable : MonoBehaviour
{
    //attr
    bool interactable=false;
    public bool showPrompt=true;
    public string promptMsg="Press [E] to interact";
    //import
    UIMsg uimsg;



    //exe
    void Awake(){
        uimsg=FindAnyObjectByType<UIMsg>();
    }
    void Update(){
        if(interactable && Input.GetKeyDown(KeyCode.E)){
            OnInteracted();
        }
    }
    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            interactable=true;
            if(showPrompt){
                uimsg.ReplacePrompt(promptMsg);
            }
        }
    }
    void OnTriggerExit(Collider col){
        if(col.gameObject.CompareTag("Player")){
            interactable=false;
            if(showPrompt && uimsg.promptTx.text==promptMsg){
                uimsg.ClearPrompt();
            }
        }
    }
    //funx
    protected abstract void OnInteracted();
    //helper
}
