using UnityEngine;

[RequireComponent(typeof(Collider))]

//v1.0
//blocks once your path

public class Barrel : MonoBehaviour
{
    //attr
    public float lifetime=10f;
    //import



    //exe
    void Start(){
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter(Collision col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.Crash();
            PlayerCar plr=col.gameObject.GetComponent<PlayerCar>();
            if(plr!=null){
                UIMsg uimsg=FindAnyObjectByType<UIMsg>();
                uimsg.AddLog("Crashed!");
            }
            Destroy(gameObject);
        }
    }
    //funx
    //helper
}
