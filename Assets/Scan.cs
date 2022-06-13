using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Scan : MonoBehaviour
{
    public float magnitude;
    public Transform ballSpawnPos;
    public GameObject[] prefabBalls;
    public Transform[] dest;
    public Transform home;
    List<string> storage = new List<string>();
    bool full = false;
    Transform target;
    NavMeshAgent agent;
    List<GameObject> balls = new List<GameObject>();
    Vector3 destination;
    GameObject shootBall;
    public int storageCapacity = 10;

    public void resetDest(){
        target = home;
    }

    public void sortStorage(){
        storage.Sort();
    }

    public void dumpInventory(){

        string temp;
        if(storage[0] == "Yellow"){
            shootBall = Instantiate(prefabBalls[3], ballSpawnPos.position, this.transform.rotation);
            temp = "Yellow";
        }
        else if (storage[0] == "Blue"){
            shootBall = Instantiate(prefabBalls[0], ballSpawnPos.position, this.transform.rotation);
            temp = "Blue";
        }
        else if (storage[0] == "Green"){
            shootBall = Instantiate(prefabBalls[1], ballSpawnPos.position, this.transform.rotation);
            temp = "Green";
        }
        else{
            shootBall = Instantiate(prefabBalls[2], ballSpawnPos.position, this.transform.rotation);
            temp = "Red";
        } 
        storage.Remove(storage[0]);
        shootBall.GetComponent<Rigidbody>().velocity = ((ballSpawnPos.position - this.transform.position) * magnitude );
        //Debug.Log(storage.Count);
        if(storage.Count != 0){
            if(storage[0] == temp){
                dumpInventory();
            }
            else{
                findDest();
            }
            
        }
        else if (storage.Count == 0){
            full = false;
            findBalls();
        }
    }

    Transform GetClosestBall(List<GameObject> balls){
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in balls){
            float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
            if (dist < minDist){
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    void findBalls(){
        if(balls.Count > 0){
            target = GetClosestBall(balls);
            Debug.DrawRay(transform.position, target.position - transform.position, Color.red, .5f);
        }
        else if (balls.Count == 0 && storage.Count == 0){
            target = home;
        }

    }
    public void addBall(GameObject b){
        balls.Add(b);
    }

    public void removeBall(GameObject b){
        balls.Remove(b);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ball")){
            balls.Add(g);
        }
        destination = agent.destination;
        findBalls();
    }

    void findDest(){
        if(storage[0] == "Yellow"){
            target = dest[2];
        }
        else if (storage[0] == "Blue"){
            target = dest[0];
        }
        else if (storage[0] == "Green"){
            target = dest[1];
        }
        else{
            target = dest[3];
        }
    }

    void OnCollisionEnter(Collision collision){
        if(storage.Count >= storageCapacity){
            full = true;
        }
        if (collision.gameObject.tag == "Ball" && !full){
            storage.Add(LayerMask.LayerToName(collision.gameObject.layer));
            if(storage.Count >= storageCapacity){
                full = true;
            }
            sortStorage();
            Destroy(collision.gameObject);
            balls.Remove(collision.gameObject);
            if(balls.Count > 0 && !full){
                target = GetClosestBall(balls);
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red, .5f);
            }
            else if (balls.Count == 0 && storage.Count == 0){
                target = home;
            }
            else if (full || balls.Count == 0 && storage.Count != 0){
                findDest();
            }
        }
        else if(full || balls.Count == 0 && storage.Count != 0){
            findDest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(destination, target.position) > .1f){
            destination = target.position;
            agent.destination = destination;
        }
    }

}
