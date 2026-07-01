using UnityEngine;

[RequireComponent(typeof(Collider))]

//v1.0
//force changes lane

public class Oil : MonoBehaviour
{
    //attr
    public float lifetime=10f;
    //import



    //exe
    void Start(){
        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.ChangeLane();
            PlayerCar plr=col.gameObject.GetComponent<PlayerCar>();
            if(plr!=null){
                UIMsg uimsg=FindAnyObjectByType<UIMsg>();
                uimsg.AddLog("Slippy! Changing lane!");
            }
            Destroy(gameObject);
        }
    }
    //funx
    //helper
}
