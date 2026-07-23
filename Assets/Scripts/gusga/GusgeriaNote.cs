using UnityEngine;
using UnityEngine.UI;

//v1.0
//holds the order info and functions for the notes' buttons

public class GusgeriaNote : MonoBehaviour
{
    //attr
    public Sprite[] mainDishes;
    public Image mainDishImg;
    int selectedDish=0;
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
    public Sprite[] drinks;
    public Image drinkImg;
    int selectedDrink=0;
    /*
    0 (none)
    1 agua
    2 rfrsc claro
    3 rfrsc oscuro
    */
    public Sprite[] checkbox=new Sprite[2];
    public Image[] spiceCheckboxes=new Image[12];
    bool[] selectedSpices=new bool[12];
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
    public GameObject[] buttons;
    public GameObject[] destroyObjs;
    bool isSmall=false;
    public GameObject smallNote;
    public GameObject bigNote;
    Vector3 offset;
    bool dragging=false;
    //import



    //exe
    void Start(){
        selectedDish=0;
        selectedDrink=0;
        UpdateDishesUI();
        UpdateDrinksUI();
        UpdateSpicesUIAll();
        UpdateSizeUI();
    }
    void Update(){
        if(Time.timeScale!=0f && dragging){
            transform.position=(Input.mousePosition+offset);
        }
    }
    //funx
    public void SaveOrder(){
        foreach(GameObject bn in buttons){
            Destroy(bn.GetComponent<Button>());
            //excluding shrink/grow
            //including the one to save!
            //only img remains!
        }
        foreach(GameObject dObj in destroyObjs){
            Destroy(dObj);
        }
    }
    public void DeleteNote(){
        Destroy(gameObject);
    }
    public void NextDish(bool forward){
        if(forward){
            selectedDish++;
            if(selectedDish>=mainDishes.Length){
                selectedDish=0;
            }
        }else{
            selectedDish--;
            if(selectedDish<mainDishes.Length){
                selectedDish=mainDishes.Length-1;
            }
        }
        UpdateDishesUI();
    }
    public void NextDrink(bool forward){
        if(forward){
            selectedDrink++;
            if(selectedDrink>=drinks.Length){
                selectedDrink=0;
            }
        }else{
            selectedDrink--;
            if(selectedDrink<drinks.Length){
                selectedDrink=drinks.Length-1;
            }
        }
        UpdateDrinksUI();
    }
    public void ToggleSpice(int idx){
        selectedSpices[idx]=!selectedSpices[idx];
        UpdateSpiceUI(idx);
    }
    public void ToggleDragging(){
        offset=transform.position-Input.mousePosition;
        dragging=!dragging;
    }
    //helper
    void UpdateDishesUI(){
        mainDishImg.sprite=mainDishes[selectedDish];
    }
    void UpdateDrinksUI(){
        drinkImg.sprite=drinks[selectedDrink];
    }
    void UpdateSpiceUI(int idx){
        if(selectedSpices[idx]){
            spiceCheckboxes[idx].sprite=checkbox[1];
        }else{
            spiceCheckboxes[idx].sprite=checkbox[0];
        }
    }
    void UpdateSpicesUIAll(){
        for(int i=0; i<selectedSpices.Length; i++){
            UpdateSpiceUI(i);
        }
    }
    public void ToggleSize(){
        isSmall=!isSmall;
        UpdateSizeUI();
    }
    void UpdateSizeUI(){
        if(isSmall){
            smallNote.SetActive(true);
            bigNote.SetActive(false);
        }else{
            smallNote.SetActive(false);
            bigNote.SetActive(true);
        }
    }
}
