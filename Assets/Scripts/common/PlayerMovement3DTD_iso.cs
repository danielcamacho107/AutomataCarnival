using UnityEngine;

//v1.1
//allows the player to move in the x-z plane
//now wasd is aligned straight

public class PlayerMovement : MonoBehaviour
{
    //stats
    public float moveSpeed=5f;
    //import



    //exe
    void Update()
    {
        if(Time.timeScale!=0f){
            Move();
        }
    }
    //funx
    void Move(){
        float movX=Input.GetAxis("Horizontal");
        float movZ=Input.GetAxis("Vertical");
        Vector3 axis=new Vector3( (movX), 0f, (movZ) );
        transform.Translate(axis.normalized*moveSpeed*Time.deltaTime);
    }
    //helper
}
