using UnityEngine;
using TMPro;

//v1.0
//Ending of shooting game

public class ShootingEnd : MonoBehaviour
{
    //attr
    public int ticketsPerScore=10;
    //import
    ResourceManager resxmgr;
    MiniMenu menu;
    Gunner gunner;
    public TMP_Text outcomeTx;
    public TMP_Text rewardsTx;

    //exe
    void Start(){
        resxmgr = FindAnyObjectByType<ResourceManager>();
        if(resxmgr==null){
            Debug.LogError("Couldnt find Resource Manager!");
        }
        menu = FindAnyObjectByType<MiniMenu>();
        if(menu==null){
            Debug.LogError("Couldnt find MiniMenu!");
        }
        gunner = FindAnyObjectByType<Gunner>();
        if(gunner==null){
            Debug.LogError("Couldnt find Gunner!");
        }else{
            gunner.SubscribeToGunDataChanged(OnGunDataChanged);
        }
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0f){
            menu.ShowMenu(1, true);
        }else if(Input.GetKeyDown(KeyCode.Escape)){
            menu.HideAllMenus(false, true);
        }
        if(Input.GetKeyDown(KeyCode.P) && Time.timeScale != 0f){
            OnGameEnd();
        }
    }
    private void OnGunDataChanged(uint score, uint ammoInMagazine, uint magazineCapacity, uint ammoInReserve)
    {
        if(ammoInMagazine == ammoInReserve && ammoInReserve == 0)
        {
            OnGameEnd();
        }
    }
    //funx
    void OnGameEnd(){
        gunner.UnsubscribeFromGunDataChanged(OnGunDataChanged);
        Debug.Log("Out of bullets!");
        outcomeTx.text="Out of bullets!";
        long notLong = ticketsPerScore*gunner.score;
        if(notLong>int.MaxValue){
            notLong=int.MaxValue;
        }else if(notLong<int.MinValue){
            notLong=int.MinValue;
        }
        rewardsTx.text="tickets +"+(int)notLong+"!";
        resxmgr.AddResource("Tickets", (int)notLong);
        menu.ShowMenu(0, true);
    }
    //helper
}
