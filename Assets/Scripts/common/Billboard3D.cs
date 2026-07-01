using UnityEngine;

//v1.0
//keeps 2D sprites aligned w the camera

public class Billboard3D : MonoBehaviour
{
    //attr
    //import
    


    //exe
    void Update(){
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
    //funx
    //helper
}
