using UnityEngine;
using TMPro;

//v1.0
//Prints messages to dialogue textbox

public class DialogueMsg : MonoBehaviour
{
    //attr
    //import
    public GameObject dialoguebox;
    public TMP_Text dialogue;
    public TMP_Text speaker;
    public bool pauseNShowMouse=false;
    public GameObject[] hideGOs;
    bool[] hideGOsStates;
    public BnSpawner bns;
    bool recorded=true;



    //exe
    void Start()
    {
        hideGOsStates=new bool[hideGOs.Length];
        RecordHideState();
        HideDialogue();
    }
    //funx
    public void Msg(string who, string msg){
        dialoguebox.SetActive(true);
        if(!recorded){
            RecordHideState();
        }
        for(int i=0; i<hideGOs.Length; i++){
            if(hideGOsStates[i]){
                hideGOs[i].SetActive(false);
            }
        }
        speaker.text=who;
        dialogue.text=msg;
        if(pauseNShowMouse){
            Time.timeScale=0f;
            Cursor.lockState=CursorLockMode.None;
        }
    }
    void ClearMsg(){
        speaker.text="";
        dialogue.text="";
    }
    public void HideDialogue(){
        ClearMsg();
        for(int i=0; i<hideGOs.Length; i++){
            if(hideGOsStates[i]){
                hideGOs[i].SetActive(true);
            }
        }
        if(pauseNShowMouse){
            Time.timeScale=1f;
            Cursor.lockState=CursorLockMode.Locked;
        }
        recorded=false;
        dialoguebox.SetActive(false);
    }
    public void NextMsg2(){
        GusgeriaCustomer csmer=bns.savedGO.GetComponent<GusgeriaCustomer>();
        if(csmer!=null){
            csmer.NextMsg();
        }
    }
    //helper
    void RecordHideState(){
        for(int i=0; i<hideGOs.Length; i++){
            if(hideGOs[i].activeSelf){
                hideGOsStates[i]=true;
            }else{
                hideGOsStates[i]=false;
            }
        }
        recorded=true;
    }
}