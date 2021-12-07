using UnityEngine.AI;
using UnityEngine;

//this script was made to change the targeting of the AI not to the player, but to a raycast point being casted down from the player onto the ground
//that point is then shifted to the nearest part of the navmesh that the raycast point is near
//this fixes 98% of the navmesh glitches i was getting when the AI would get stuck if the player was slightly off the mesh
public class FindNewPath : MonoBehaviour
{
    AIMain main;

    public Transform Player;
    Vector3 rayHitPos;
    Ray ray;
    RaycastHit rayHit;
    public Vector3 newTarget;

    private void Awake()
    {
        //gets players transform
        if (GameObject.Find("FPSController")) Player = GameObject.Find("FPSController").transform;
        else Player = null;

        main = GetComponent<AIMain>(); //ai main script
    }
    void Update()
    {
        //creates raycast from players position and then casts it directly down
        Vector3 currPos = Player.position;
        Vector3 direction = Vector3.down; //(0,-1,0) down
        ray.origin = currPos;
        ray.direction = direction;

        Debug.DrawRay(ray.origin, direction, Color.red); //draws the ray in editor for testing

        if (Physics.Raycast(ray, out rayHit, 10f)) //if hitting the ground rayHitPos is the vector3 made out of the rayHit's point
        {
            rayHitPos = rayHit.point;
        }

        NavMeshHit hit; //navmesh hit
        if (NavMesh.SamplePosition(rayHitPos, out hit, 10f, NavMesh.AllAreas)) //SamplePosition great function for getting nearest part of the mesh out of raycast hit
        {
            newTarget = hit.position;
        }
        else 
        {
            print("No NavMesh"); //should not be called unless there is no navmesh in map
        }
    }
}
