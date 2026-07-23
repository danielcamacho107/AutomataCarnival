using UnityEngine;

//v1.0
//any food item, finished or ingredient

public class GusgeriaFoodItem : MonoBehaviour
{
    //attr
    public int dish=0;
    public int drink=0; //water 2 white 3 black 5 (know what is included by prime factors)
    public bool isDrink=false;
    public bool[] ingredients=new bool[12];
    public float[] cookTime=new float[8];
    public bool[] spices=new bool[12];
    public Vector2[] spiceTargetColors=new Vector2[12]; //x color y sat
    public bool isSpice=false;
    //import
    [HideInInspector] public SpriteRenderer rend;
    public Sprite[] dishImgs;
    Collider2D coll;
    Rigidbody2D rb;
    UIMsg uimsg;
    public Sprite altIngImg;




    //exe
    void Start(){
        rend=GetComponent<SpriteRenderer>();
        coll=GetComponent<Collider2D>();
        rb=GetComponent<Rigidbody2D>();
        uimsg=FindAnyObjectByType<UIMsg>();
    }
    void Update(){
        
    }
    void OnCollisionEnter2D(Collision2D col){
        GusgeriaFoodItem colgfi=col.gameObject.GetComponent<GusgeriaFoodItem>();
        if(colgfi!=null){
            if((!isDrink&&!isSpice)){
                //merge spices
                for(int i=0; i<colgfi.spices.Length; i++){
                    if(colgfi.spices[i]){
                        float H, S, V;
                        spices[i]=true;
                        Color.RGBToHSV(rend.color, out H, out S, out V);
                        //change color ±1f/360f based on index and current color
                        if(spiceTargetColors[i].x>H){
                            //more t color
                            H+=1/360f;
                        }else{
                            //less t color
                            H-=1/360f;
                        }
                        //change pastel ±1f/100f based on index and current pastel
                        if(spiceTargetColors[i].y>S){
                            //more t sat
                            S+=1/100f;
                        }else{
                            //less t sat
                            S-=1/100f;
                        }
                        rend.color=Color.HSVToRGB(H, S, V);
                    }
                }
                //merge ingredients
                for(int i=0; i<colgfi.ingredients.Length; i++){
                    if(colgfi.ingredients[i]){
                        ingredients[i]=true;
                    }
                    if(i<=7 && colgfi.cookTime[i]>cookTime[i]){
                        cookTime[i]=colgfi.cookTime[i];
                    }
                }
                if(colgfi.isSpice){
                    Destroy(colgfi.gameObject);
                }else if(colgfi.isDrink){
                    //what happens to drinks colliding on nonspice nondrinks
                    //delete rigidbody and collider
                    if(colgfi.drink==0){
                        Debug.LogError("Did not set which drink!");
                    }else{
                        drink*=colgfi.drink;
                    }
                    colgfi.PruneAndParent(transform.GetChild(0));
                }else{
                    //delete tops rigidbody and bottoms script collider rigidbody if exists and children botm to top
                    //determine if this is top
                    if(gameObject.transform.position.y>colgfi.gameObject.transform.position.y){
                        //is top
                        colgfi.PruneAndParent(transform.GetChild(1));
                        if(!CraftFood()){
                            Destroy(rb);
                        }
                    }
                }
            }
        }
    }
    //funx
    bool CraftFood(){
        string foundDish="(none)";
        float cookmin=6f;
        float cookmax=10f;
        //check ing list
        //if theres a match for a dish:
            //delete all nondrink children
            //change sprite to found dish
            //tell on uimsg "(dish) crafted!"
        /*
        0 (none)
        1 espiropapa → 2 (papa+(cook))     + Ɛ sal            + (spice)
        2 esquite    → 9 (elote+(cook))    + Ɛ sal            + (spice)
        3 churros    → 7 (masa+(cook))     + XƐ {azucar/sal}  + (spice)
        4 taco       → 3 (tortilla+(cook)) + 0 (carne+(cook)) + (spice)
        5 tostilocos → 2 (papa+(cook))     + 1 (salch+(cook)) + (spice)
        6 hotdog     → 6 (pan+(cook))      + 1 (salch+(cook)) + (spice)
        7 hamburgesa → 4 (pan+(cook))      + 0 (carne+(cook)) + (spice) + 5 (pan+(cook))
        8 tejuino    → — tejuino           + Ɛ sal            + (spice) + 8 nieve
        */
        #region LookupCraft
            if(ingredients[2] && ingredients[11]){
                if(cookTime[2]>cookmin && cookTime[2]<cookmax){
                    foundDish="Espiropapa";
                    dish=1;
                }else if(cookTime[2]<cookmin){
                    foundDish="Espiropapa (underdone)";
                    dish=9;
                }else{
                    foundDish="Espiropapa (overdone)";
                    dish=10;
                }
            }else if(ingredients[9] && ingredients[11]){
                foundDish="Esquite";
                dish=2;
            }else if(ingredients[7] && ingredients[10]){
                if(cookTime[7]>cookmin && cookTime[7]<cookmax){
                    foundDish="Churros";
                    dish=3;
                }else if(cookTime[7]<cookmin){
                    foundDish="Churros (underdone)";
                    dish=11;
                }else{
                    foundDish="Churros (overdone)";
                    dish=12;
                }
            }else if(ingredients[3] && ingredients[0]){
                if(cookTime[3]>cookmin && cookTime[3]<cookmax){
                    if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                        foundDish="Taco";
                        dish=4;
                    }else if(cookTime[0]<cookmin){
                        foundDish="Taco (raw)";
                        dish=13;
                    }else{
                        foundDish="Taco (burnt)";
                        dish=14;
                    }
                }else if(cookTime[3]<cookmin){
                    if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                        foundDish="Taco (soft)";
                        dish=15;
                    }else if(cookTime[0]<cookmin){
                        foundDish="Taco (raw, soft)";
                        dish=16;
                    }else{
                        foundDish="Taco (burnt, soft)";
                        dish=17;
                    }
                }else{
                    if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                        foundDish="Taco (charred)";
                        dish=18;
                    }else if(cookTime[0]<cookmin){
                        foundDish="Taco (raw, charred)";
                        dish=19;
                    }else{
                        foundDish="Taco (burnt, charred)";
                        dish=20;
                    }
                }
            }else if(ingredients[2] && ingredients[1]){
                if(cookTime[2]>cookmin && cookTime[2]<cookmax){
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Tostilocos";
                        dish=5;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Tostilocos (raw)";
                        dish=21;
                    }else{
                        foundDish="Tostilocos (burnt)";
                        dish=22;
                    }
                }else if(cookTime[2]<cookmin){
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Tostilocos (underdone)";
                        dish=23;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Tostilocos (raw, underdone)";
                        dish=24;
                    }else{
                        foundDish="Tostilocos (burnt, underdone)";
                        dish=25;
                    }
                }else{
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Tostilocos (overdone)";
                        dish=26;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Tostilocos (raw, overdone)";
                        dish=27;
                    }else{
                        foundDish="Tostilocos (burnt, overdone)";
                        dish=28;
                    }
                }
            }else if(ingredients[6] && ingredients[1]){
                if(cookTime[6]>cookmin && cookTime[6]<cookmax){
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Hotdog";
                        dish=6;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Hotdog (raw)";
                        dish=29;
                    }else{
                        foundDish="Hotdog (burnt)";
                        dish=30;
                    }
                }else if(cookTime[6]<cookmin){
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Hotdog (underdone)";
                        dish=31;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Hotdog (raw, underdone)";
                        dish=32;
                    }else{
                        foundDish="Hotdog (burnt, underdone)";
                        dish=33;
                    }
                }else{
                    if(cookTime[1]>cookmin && cookTime[1]<cookmax){
                        foundDish="Hotdog (overdone)";
                        dish=34;
                    }else if(cookTime[1]<cookmin){
                        foundDish="Hotdog (raw, overdone)";
                        dish=35;
                    }else{
                        foundDish="Hotdog (burnt, overdone)";
                        dish=36;
                    }
                }
            }else if(ingredients[4] && ingredients[5] && ingredients[0]){
                if(cookTime[4]>cookmin && cookTime[4]<cookmax){
                    if(cookTime[5]>cookmin && cookTime[5]<cookmax){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger";
                            dish=7;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw)";
                            dish=37;
                        }else{
                            foundDish="Burger (burnt)";
                            dish=38;
                        }
                    }else if(cookTime[5]<cookmin){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top underdone)";
                            dish=39;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top underdone)";
                            dish=40;
                        }else{
                            foundDish="Burger (burnt, top underdone)";
                            dish=41;
                        }
                    }else{
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top overdone)";
                            dish=42;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top overdone)";
                            dish=43;
                        }else{
                            foundDish="Burger (burnt, top overdone)";
                            dish=44;
                        }
                    }
                }else if(cookTime[4]<cookmin){
                    if(cookTime[5]>cookmin && cookTime[5]<cookmax){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (bottom underdone)";
                            dish=45;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, bottom underdone)";
                            dish=46;
                        }else{
                            foundDish="Burger (burnt, bottom underdone)";
                            dish=47;
                        }
                    }else if(cookTime[5]<cookmin){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top & bottom underdone)";
                            dish=48;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top & bottom underdone)";
                            dish=49;
                        }else{
                            foundDish="Burger (burnt, top & bottom underdone)";
                            dish=50;
                        }
                    }else{
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top overdone, bottom underdone)";
                            dish=51;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top overdone, bottom underdone)";
                            dish=52;
                        }else{
                            foundDish="Burger (burnt, top overdone, bottom underdone)";
                            dish=53;
                        }
                    }
                }else{
                    if(cookTime[5]>cookmin && cookTime[5]<cookmax){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (bottom overdone)";
                            dish=54;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, bottom overdone)";
                            dish=55;
                        }else{
                            foundDish="Burger (burnt, bottom overdone)";
                            dish=56;
                        }
                    }else if(cookTime[5]<cookmin){
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top underdone, bottom overdone)";
                            dish=57;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top underdone, bottom overdone)";
                            dish=58;
                        }else{
                            foundDish="Burger (burnt, top underdone, bottom overdone)";
                            dish=59;
                        }
                    }else{
                        if(cookTime[0]>cookmin && cookTime[0]<cookmax){
                            foundDish="Burger (top & bottom overdone)";
                            dish=60;
                        }else if(cookTime[0]<cookmin){
                            foundDish="Burger (raw, top & bottom overdone)";
                            dish=61;
                        }else{
                            foundDish="Burger (burnt, top & bottom overdone)";
                            dish=62;
                        }
                    }
                }
            }else if(ingredients[8] && ingredients[11]){
                foundDish="Tejuino";
                dish=8;
            }
        #endregion //LookupCraft
        if(foundDish!="(none)"){
            rend.sprite=dishImgs[dish];
            //del all nondrink children
            int cc=gameObject.transform.GetChild(1).childCount;
            for(int i=0; i<cc; i++){
                Destroy(transform.GetChild(1).GetChild(0).gameObject);
            }
            if(rb=null){
                rb=gameObject.AddComponent<Rigidbody2D>();
            }
            uimsg.AddLog(""+foundDish+" crafted!");
            return true;
        }else{
            return false;
        }
    }
    //helper
    public void PruneAndParent(Transform newParent){
        if(rb!=null){
            Destroy(rb);
        }
        Destroy(coll);
        gameObject.transform.parent=newParent;
        Destroy(this);
    }
}
