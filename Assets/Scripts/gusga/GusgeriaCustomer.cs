using UnityEngine;
using UnityEngine.UI;

//v1.0
//customer in kitchen game, holds his own order data and dialogue

public class GusgeriaCustomer : MonoBehaviour
{
    //attr
    int dish=0;
    public string[] dishNames;
    public int dishNamesPer=1;
    /*
    0 (none)
    1 espiropapa → (papa+(cook))                      + (spice)
    2 esquite    → (elote+(cook))                     + (spice)
    3 churros    → (masa+(cook))     + {sal/azucar}   + (spice)
    4 taco       → (tortilla+(cook)) + (carne+(cook)) + (spice)
    5 tostilocos → (papa+(cook))     + (salch+(cook)) + (spice)
    6 hotdog     → (pan+(cook))      + (salch+(cook)) + (spice)
    7 hamburgesa → (pan+(cook))      + (carne+(cook)) + (spice) + (pan+(cook))
    8 tejuino    → tejuino           + sal            + (spice) + nieve
    */
    int drink=0;
    public string[] drinkNames;
    public int drinkNamesPer=1;
    /*
    0 (none)
    1 agua
    2 rfrsc claro
    3 rfrsc oscuro
    */
    bool[] spices=new bool[12];
    public string[] spiceNames;
    public int spiceNamesPer=1;
    /*
    0 sal
    1 limon
    2 catsup
    3 mayonesa
    4 mostaza
    5 salsa picante
    6 pink sauce
    7 queso
    8 cebolla
    9 jitomate
    X elote
    Ɛ azucar
    */
    public string[] dialogue;
    int cDiag=-1;
    public int dialoguePer=1;
    string csmerName;
    public string[] csmerNames;
    public int csmerNamesPer=1;
    public Sprite[] csmerImgs;
    public Image csmerImg;
    public float waitTime=60f;
    //import
    DialogueMsg diag;



    //exe
    void Start(){
        SetupRandomData();
        diag=FindAnyObjectByType<DialogueMsg>();
        NextMsg();
    }
    void Update(){
        //timer
    }
    //funx
    //helper
    void SetupRandomData(){
        dish=Random.Range(0,9);
        drink=Random.Range(0,4);
        for(int i=0; i<spices.Length; i++){
            spices[i]=Random.Range(0, 2)==0;
        }
        int chosenCsmer=Random.Range(0,csmerImgs.Length);
        csmerImg.sprite=csmerImgs[chosenCsmer];
        csmerImg.color=Random.ColorHSV(0f, 1f, 0f, 1f, 0.75f, 1f, 1f, 1f);
        csmerName=csmerNames[Random.Range(0,csmerNamesPer)+(chosenCsmer*csmerNamesPer)];
        waitTime=Random.Range(waitTime/2f, 2*waitTime);
    }
    public void NextMsg(){
        cDiag++;
        if(dialogue[cDiag]=="(close)"){
            diag.HideDialogue();
            HideCsmer();
        }else{
            dialogue[cDiag]=dialogue[cDiag].Replace("(dish)",dishNames[Random.Range(0,dishNamesPer)+(dish*dishNamesPer)]);
            dialogue[cDiag]=dialogue[cDiag].Replace("(drink)",drinkNames[Random.Range(0,drinkNamesPer)+(drink*drinkNamesPer)]);
            dialogue[cDiag]=dialogue[cDiag].Replace("(spices)","(spice0), (spice1), (spice2), (spice3), (spice4), (spice5), (spice6), (spice7), (spice8), (spice9), (spice10), (spice11)");
            for(int i=0; i<spices.Length; i++){
                if(spices[i]){
                    dialogue[cDiag]=dialogue[cDiag].Replace("(spice"+i+")", spiceNames[Random.Range(0,spiceNamesPer)+(i*spiceNamesPer)]);
                }else{
                    dialogue[cDiag]=dialogue[cDiag].Replace(" (spice"+i+"),", "");
                    dialogue[cDiag]=dialogue[cDiag].Replace(", (spice"+i+").", ".");
                }
            }
            diag.Msg(csmerName, dialogue[cDiag]);
        }
    }
    void HideCsmer(){
        csmerImg.enabled=false;
        GusgeriaWaitListMgr gwlmgr=FindAnyObjectByType<GusgeriaWaitListMgr>();
        gwlmgr.AddCsmer(dish, drink, spices, Time.time+waitTime, csmerImg.sprite, csmerImg.color);
        //cast to button & set timer
    }
}
