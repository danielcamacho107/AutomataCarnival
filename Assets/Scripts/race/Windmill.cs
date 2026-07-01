using UnityEngine;

[RequireComponent(typeof(Collider))]

//v1.0
//adds a speed boost

public class Windmill : MonoBehaviour
{
    //attr
    public float speedBoost=12f;
    public float lifetime=10f;
    //import



    //exe
    void Start(){
        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.currentSpeed+=speedBoost;
            PlayerCar plr=col.gameObject.GetComponent<PlayerCar>();
            if(plr!=null){
                UIMsg uimsg=FindAnyObjectByType<UIMsg>();
                uimsg.AddLog("Speed boost!");
            }
            Destroy(gameObject);
        }
    }
    //funx
    //helper
}
