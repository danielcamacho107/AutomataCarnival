using UnityEngine;
using UnityEngine.SceneManagement;

//v1.0
//opens a scene by name on interact

public class Interactable_SendToScene : Interactable
{
    //attr
    public string sceneName="World";
    //import



    //exe
    //funx
    protected override void OnInteracted(){
        if(sceneName=="Race"){
            sceneName=sceneName+"_"+Random.Range(0,2);
        }
        SceneManager.LoadScene(sceneName);
    }
    //helper
}