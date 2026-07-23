using UnityEngine;
using TMPro;

//v1.0
//Prints messages to dialogue textbox

//stick it to canvas
//triggerable prompt here is a parent of the text

public class DialogueMsg : MonoBehaviour
{
    //attr
    //import
    public GameObject dialoguebox;
    public TMP_Text dialogue;
    public TMP_Text speaker;
    public bool pauseNshowMouse=false;
    public GameObject triggerablePrompt;



    //exe
    void Start()
    {
        HideDialogue();
    }
    //funx
    public void Msg(string who, string msg){
        dialoguebox.SetActive(true);
        triggerablePrompt.SetActive(false);
        speaker.text=who;
        dialogue.text=msg;
        if(pauseNshowMouse){
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
        dialoguebox.SetActive(false);
        triggerablePrompt.SetActive(true);
        if(pauseNshowMouse){
            Time.timeScale=1f;
            Cursor.lockState=CursorLockMode.Locked;
        }
    }
    //helper
}