using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]

//v1.0
//Adds +1 lap when crossed

//Player must be contestant 0

public class FinishLine : MonoBehaviour
{
    //attr
    public int lapsToWin=5;
    public int ticketReward=100;
    int[] laps;
    int winner = -1;
    //import
    public Car[] contestants;
    public TMP_Text[] lapsTxs;
    MiniMenu menu;
    public TMP_Text outcomeTx;
    public TMP_Text rewardsTx;
    ResourceManager resxmgr;
    

    //exe
    void Start()
    {
        resxmgr=FindAnyObjectByType<ResourceManager>();
        if(resxmgr==null){
            Debug.LogError("Couldnt find Resource Manager!");
        }
        winner=-1;
        if(contestants.Length != lapsTxs.Length){
            Debug.LogError("Array lengths mismatch!");
        }
        laps=new int[contestants.Length];
        UpdateLapsUI();
        menu=FindFirstObjectByType<MiniMenu>();
        menu.HideAllMenus();
    }
    void OnTriggerEnter(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            for(int i=0; i<contestants.Length; i++){
                if(contestants[i]==car){
                    laps[i]++;
                    UpdateLapsUI();
                    if(laps[i]>lapsToWin && winner==-1){
                        winner=i;
                        if(winner == 0){
                            OnWin();
                        }else{
                            OnLose();
                        }
                    }
                    break;
                } //contestants[i]==car
            } //for
        } //car!=null
    } //OnTriggerEnter
    //funxs
    void UpdateLapsUI(){
        for(int i=0; i<lapsTxs.Length; i++){
            if(laps[i]-1<0){
                lapsTxs[i].text="Laps: "+0+"/"+lapsToWin;
            }else{
                lapsTxs[i].text="Laps: "+(laps[i]-1)+"/"+lapsToWin;
            }
        }
    }
    //helper
    void OnWin(){
        Debug.Log("You won this race!");
        outcomeTx.text="You won this race!";
        rewardsTx.text="tickets +"+(ticketReward*lapsToWin)+"!";
        if(resxmgr!=null){
            resxmgr.AddResource("Tickets", ticketReward*lapsToWin);
        }
        menu.ShowMenu(0, true);
    }
    public void OnLose(){
        Debug.Log("You lost this race...");
        outcomeTx.text="You lost this race...";
        rewardsTx.text="tickets +"+(ticketReward*laps[0]);
        if(resxmgr!=null){
            resxmgr.AddResource("Tickets", ticketReward*laps[0]);
        }
        menu.ShowMenu(0, true);
    }
}