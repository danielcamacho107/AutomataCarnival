using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//v1.0
//desco

public class GusgeriaUI : MonoBehaviour
{
    //attr
    public GameObject[] areas;
    public GameObject[] areaBns;
    public GameObject canvasAreas;
    public GameObject[] anchors;
    /*[HideInInspector]*/ public List<GameObject> GOsInKitchen=new List<GameObject>();
    /*[HideInInspector]*/ public List<GameObject> GOsInFurnace=new List<GameObject>();
    /*[HideInInspector]*/ public List<GameObject> GOsInCounter=new List<GameObject>();
    [HideInInspector] public int currentArea=0;
    //import
    GusgeriaMarker mark;



    //exe
    void Start(){
        mark=FindAnyObjectByType<GusgeriaMarker>();
        ViewArea(currentArea);
        StartCoroutine(Cook());
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
        currentArea=areaIdx;
    }
    public void SendGOsToNext(int fromArea){
        if(fromArea==1){
            int count=GOsInKitchen.Count;
            for(int i=0; i<count; i++){
                if(GOsInKitchen[0]!=null){
                    GOsInCounter.Add(GOsInKitchen[0]);
                    GOsInKitchen[0].transform.position=new Vector3(areas[0].transform.position.x,
                        GOsInKitchen[0].transform.position.y, GOsInKitchen[0].transform.position.z);
                }
                GOsInKitchen.RemoveAt(0);
            }
        }else if(fromArea==2){
            int count=GOsInFurnace.Count;
            for(int i=0; i<count; i++){
                if(GOsInFurnace[0]!=null){
                    GOsInKitchen.Add(GOsInFurnace[0]);
                    GOsInFurnace[0].transform.position=new Vector3(areas[1].transform.position.x,
                        GOsInFurnace[0].transform.position.y, GOsInFurnace[0].transform.position.z);
                }
                GOsInFurnace.RemoveAt(0);
            }
        }
    }
    public void DestroyGOsIn(int areaIdx){
        if(areaIdx==0){

            for(int i=GOsInCounter.Count-1; i>=0; i--){
                if(GOsInCounter[i]!=null){
                    Destroy(GOsInCounter[i]);
                    GOsInCounter.RemoveAt(i);
                }else{
                    GOsInCounter.RemoveAt(i);
                }
            }
        }else if(areaIdx==1){
            for(int i=GOsInKitchen.Count-1; i>=0; i--){
                if(GOsInKitchen[i]!=null){
                    Destroy(GOsInKitchen[i]);
                    GOsInKitchen.RemoveAt(i);
                }else{
                    GOsInKitchen.RemoveAt(i);
                }
            }
        }else if(areaIdx==2){
            for(int i=GOsInFurnace.Count-1; i>=0; i--){
                if(GOsInFurnace[i]!=null){
                    Destroy(GOsInFurnace[i]);
                    GOsInFurnace.RemoveAt(i);
                }else{
                    GOsInFurnace.RemoveAt(i);
                }
            }
        }else if(areaIdx==4){
            DestroyDividers();
        }
    }
    //helper
    float cookrate=1f;
    IEnumerator Cook(){
        while(true){
            yield return new WaitForSeconds(cookrate);
            if(Time.timeScale!=0f){
                foreach(GameObject go in GOsInFurnace){
                    if(go!=null){
                        GusgeriaFoodItem gfi=go.GetComponent<GusgeriaFoodItem>();
                        if(gfi!=null){
                            for(int i=0; i<gfi.cookTime.Length; i++){
                                if(gfi.ingredients[i]){
                                    gfi.cookTime[i]+=cookrate;
                                    //change color
                                    float H, S, V;
                                    Color.RGBToHSV(gfi.rend.color, out H, out S, out V);
                                    if(V>0){
                                        V-=(1*cookrate)/30f;
                                    }
                                    gfi.rend.color=Color.HSVToRGB(H, S, V);
                                }
                            }
                        }
                    }
                }
            }
        }
    }//cook
    public void DestroyDividers()
    {
        Divider[] dividers = FindObjectsByType<Divider>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        for (int i = dividers.Length - 1; i >= 0; i--)
        {
            if (dividers[i] != null){
                Destroy(dividers[i].gameObject);
            }
        }
    }
}
