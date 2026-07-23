using UnityEngine;

//v1.0
//desco

public class GusgeriaUI : MonoBehaviour
{
    //attr
    public GameObject[] areas;
    public GameObject[] areaBns;
    public GameObject canvasAreas;
    public GameObject[] anchors;
    //import
    GusgeriaMarker mark;



    //exe
    void Start(){
        mark=FindAnyObjectByType<GusgeriaMarker>();
        ViewArea(0);
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
        canvasAreas.transform.position=new Vector3(anchors[areaIdx].transform.position.x, canvasAreas.transform.position.y, canvasAreas.transform.position.z);
        if(areaIdx==0){
            areaBns[3].SetActive(true);
            areaBns[4].SetActive(true);
        }else{
            areaBns[3].SetActive(false);
            areaBns[4].SetActive(false);
        }
    }
    //helper
}
