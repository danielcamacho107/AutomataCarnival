using UnityEngine;
using TMPro;

//v1.0
//manages minimenus and win condition on whackamole

public class WhackMenuMgr : MonoBehaviour
{
    //attr
    public int ticketsPerScore=10;
    //import
    MiniMenu menu;
    public TMP_Text scoreTx;
    public TMP_Text rewardTx;
    ResourceManager resxmgr;



    //exe
    void Start(){
        menu=FindAnyObjectByType<MiniMenu>();
        menu.HideAllMenus(true);
        resxmgr=FindAnyObjectByType<ResourceManager>();
    }
    void Update(){
        if(Time.timeScale!=0f){
            if(Input.GetKeyDown(KeyCode.Escape)){
                menu.ShowMenu(1, true);
            }
        }
    }
    //funx
    public void OnGameEnd(int score){
        scoreTx.text="Score: "+score;
        rewardTx.text="+"+(score*ticketsPerScore)+" Tickets";
        menu.ShowMenu(0, true);
        resxmgr.AddResource("Tickets", (score*ticketsPerScore));
    }
    //helper
}
