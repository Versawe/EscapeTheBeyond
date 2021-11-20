using UnityEngine.AI;
using UnityEngine;

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
        if (GameObject.Find("FPSController")) Player = GameObject.Find("FPSController").transform;
        else Player = null;

        main = GetComponent<AIMain>();
    }
    void Update()
    {
        Vector3 currPos = Player.position;
        Vector3 direction = Vector3.down;
        ray.origin = currPos;
        ray.direction = direction;

        Debug.DrawRay(ray.origin, direction, Color.red);

        if (Physics.Raycast(ray, out rayHit, 10f)) 
        {
            rayHitPos = rayHit.point;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rayHitPos, out hit, 10f, NavMesh.AllAreas))
        {
            newTarget = hit.position;
        }
        else 
        {
            print("No NavMesh");
        }
    }
}
