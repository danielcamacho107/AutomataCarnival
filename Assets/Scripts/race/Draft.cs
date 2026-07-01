using UnityEngine;

//v1.0
//makes whatever car is in this trigger faster

public class Draft : MonoBehaviour
{
    //attr
    //import



    //exe
    //draft on
    void OnTriggerStay(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.Draft();
        }
    }
    //draft off
    void OnTriggerExit(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.draftScale=0f;
        }
    }
    //funx
    //helper
}
