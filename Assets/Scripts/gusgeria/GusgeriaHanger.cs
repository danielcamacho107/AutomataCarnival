using UnityEngine;

//v1.0
//desco

public class GusgeriaHanger : MonoBehaviour
{
    //attr
    //import



    //exe
    void OnCollisionEnter2D(Collision2D col){
        GusgeriaNote gNote=col.gameObject.GetComponentInParent<GusgeriaNote>();
        if(gNote!=null){
            gNote.StopDragging();
        }
    }
    //funx
    //helper
}
