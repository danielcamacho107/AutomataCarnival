using UnityEngine;

[RequireComponent(typeof(Collider))]

//v2.0
//Triggers next waypoint when reached

public class Waypoint : MonoBehaviour
{
    //attr
    [Header("Waypoint")]
    public bool faceRight=true;
    public bool lockLane=false;
    public GameObject[] nextWaypoints; //to 0 leftward, to INF rightward
    public int straightLane=0;
    [Header("Props")]
    public float spawnProbability=50f;
    public float spawnCD=15f;
    float spawnICD=0f;
    public float spawnCDRandomScale=2f;
    public GameObject[] props;
    //import



    //exe
    void Start(){
        spawnICD=spawnCD+(spawnCD*Random.Range(-spawnCDRandomScale, spawnCDRandomScale));
    }
    void Update(){
        if(Time.timeScale!=0f){
            Spawn();
        }
    }
    void OnTriggerEnter(Collider col){
        Car car=col.gameObject.GetComponent<Car>();
        if(car!=null){
            car.ReachWaypoint(this);
        }
    }
    //funx
    void Spawn(){
        if(Time.time>spawnICD){
            spawnICD=Time.time+spawnCD+(spawnCD*Random.Range(-spawnCDRandomScale, spawnCDRandomScale));
            if(Random.Range(0f, 100f)<=spawnProbability){
                Instantiate(props[Random.Range(0,props.Length)], transform.position, Quaternion.identity);
            }
        }
    }
    //helper
}
