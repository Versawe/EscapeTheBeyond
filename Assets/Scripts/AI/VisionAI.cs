using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionAI : MonoBehaviour
{
    public bool spawnRay = false;
    private float sightDistance = 15;

    GameObject Player;
    Transform Monster;
    AIMain mainScript;
    PlayerHiding hideScript;

    private Ray upperSight;
    private Ray sight;
    RaycastHit rayHit;
    public bool ray1See = false;
    public bool ray2See = false;

    //This script is attached to the child of the AI called AISight
    //AIsight is a trigger box in the line of sigh for the AI, if the player...
    // is in the box rays will automatically go out and look directly at the player.
    //the rays will check if there are any objects inbetween the AI and the player.
    //this script controls the activation of visualAI and hasBeenChanged in ripperAI.cs

    void Start()
    {
        Player = GameObject.Find("FPSController");
        hideScript = Player.GetComponentInChildren<PlayerHiding>();

        Monster = GetComponentInParent<Transform>();
        mainScript = GetComponentInParent<AIMain>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (spawnRay)
        {
            //gets direction from point of player to point of monster's line of sight
            Vector3 direction = Player.transform.position - Monster.position;
            sight.origin = Monster.position;
            sight.direction = direction;

            //gets direction from upper point on player to point of monster's line of sight
            Vector3 upperDirection = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.75f, Player.transform.position.z) - Monster.position;
            upperSight.origin = Monster.position;
            upperSight.direction = upperDirection;

            //used to see AI's sight in engine
            Debug.DrawRay(sight.origin, direction, Color.red);
            Debug.DrawRay(upperSight.origin, upperDirection, Color.red);

            //if the first ray sees the player
            if (Physics.Raycast(sight, out rayHit, sightDistance))
            {
                if (rayHit.collider.tag == "Player" && !hideScript.isHiding)
                {
                    mainScript.visualAI = true;
                    mainScript.hasBeenSeen = true;
                    ray1See = true;
                    //print("I see player");
                }
                else
                {
                    ray1See = false;
                }
            }
            
            //if the second ray sees the player
            if (Physics.Raycast(upperSight, out rayHit, sightDistance))
            {
                if (rayHit.collider.tag == "Player" && !hideScript.isHiding)
                {
                    mainScript.visualAI = true;
                    mainScript.hasBeenSeen = true;
                    ray2See = true;
                    //print("I see player");
                }
                else
                {
                    ray2See = false;
                }
            }
        }
        else 
        {
            ray1See = false;
            ray2See = false;
        }
    }

    //if player enters vision box in line of sight of monster
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spawnRay = true;
        }
    }

    //if player exits vision box in line of sight of monster
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spawnRay = false;
        }
    }
}
