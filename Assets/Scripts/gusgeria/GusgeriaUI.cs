using UnityEngine;

//v1.0
//desco

public class GusgeriaUI : MonoBehaviour
{
    //attr
    public GameObject[] areas;
    public GameObject[] areaBns;
    //import
    GusgeriaMarker mark;



    //exe
    void Start(){
        mark=FindAnyObjectByType<GusgeriaMarker>();
    }
    //funx
    public void ViewArea(int areaIdx){
        mark.gameObject.transform.position=new Vector3(areas[areaIdx].transform.position.x,
            mark.gameObject.transform.position.y, mark.gameObject.transform.position.z);
        foreach (GameObject bn in areaBns)
        {
            bn.SetActive(true);
        }
        areaBns[areaIdx].SetActive(false);
    }
    //helper
}
