using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//v1.0
//menu library for minigames

public class MiniMenu : MonoBehaviour
{
    //attr
    public GameObject[] menus;



    //funx
    public void ShowMenu(int menuIdx, bool pauseState){
        HideAllMenus();
        menus[menuIdx].SetActive(true);
        SetPauseState(pauseState, pauseState);
    }
    public void ReloadScene(){
        Time.timeScale=1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadScene(string sceneName){
        Time.timeScale=1f;
        SceneManager.LoadScene(sceneName);
    }
    //helper
    public void HideAllMenus(){
        foreach(GameObject menu in menus){
           menu.SetActive(false);
        }
        SetPauseState(false, false);
    }
    public void HideAllMenus(bool hasCursor){
        foreach(GameObject menu in menus){
           menu.SetActive(false);
        }
        SetPauseState(false, hasCursor);
    }
    public void HideAllMenus(bool paused, bool hasCursor){
        foreach(GameObject menu in menus){
           menu.SetActive(false);
        }
        SetPauseState(paused, hasCursor);
    }
    public void UpdateTextUI(TMP_Text uiText, string msg){
        uiText.text=msg;
    }
    void SetPauseState(bool paused, bool hasCursor){
        if(paused){
            Time.timeScale=0f;
        }else{
            Time.timeScale=1f;
        }
        if(hasCursor){
            Cursor.lockState=CursorLockMode.None;
            Cursor.visible=true;
        }else{
            Cursor.lockState=CursorLockMode.Locked;
            Cursor.visible=false;
        }
    }
}
