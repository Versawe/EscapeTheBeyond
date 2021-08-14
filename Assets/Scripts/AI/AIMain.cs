using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class AIMain : MonoBehaviour
{
    //nav mesh agent variable
    NavMeshAgent nm;
    public Transform thisMonster;

    //player's transform
    public GameObject player;
    PlayerHiding hideScript;

    //scare vars
    private float scareDis = 1.75f;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    Transform farthestSpawn;
    private bool isScaring = false;
    private float scareTimer = 2.5f;
    public string AIName;
    public bool IsRipper = false;

    PlayerHealth pHealth;

    //patrol vars
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    public Transform patrolPoint3;
    public Transform patrolPoint4;
    private float patrolNum;
    private bool patrolDoOnce = false;
    private float[] patrolNumber = {1, 2, 3, 4};

    //AI sight vars
    public bool visualAI = false;
    public bool hasBeenSeen = false;
    private float losesPlayerTimer = 10;
    public GameObject searchPoint;
    VisionAI visionScript;

    //ai state vars
    private string[] State = { "Chase", "ChasePlus", "Still", "Track", "Patrol", "Search", "Stalk", "Attack"};
    public string aiState;

    //destroy door vars
    public GameObject doorDestroyer;
    public GameObject doorTarget;
    public bool destroyObj = false;
    private float attackTimer = 3;

    public GameObject targetMoveObj; //These two vars help position & rotate the AI better for when he is attacking doors
    public GameObject targetRotateObj;

    //blink speed boost vars
    private bool speedBoost = false;
    private float checkForBoostTimer = 5;
    private float checkForBoostRandom = 11;
    private float AISpeed;
    private float AISpeedBoost;
    private float speedBoostLength = 0.25f;
    private float speedBoostChance = 3; // var / 10 chance to speed boost every 5 seconds while chasing

    //scream state vars
    public bool IsScreaming = false;
    public bool wasScreaming = false; //NEEDed TO DELAY THIS AFTER ISSCREAMING HAS ENDED SO CHASE STATE PLUS WORKS
    private float checkForScreamTimer = 10;
    private float checkForScreamRandom = 11;
    private float screamLength = 5f;
    private float screamChance = 2; // var / 10 chance to go into scream state every 10 seconds while patrolling (base is 1 or 2, switch to 9 or 10 if testing)

    AIScream ScreamScript;

    //chase plus var
    private float wasScreamingTimer = 0.5f;
    private float chasePlusTimer = 10;

    void Awake()
    {
        List<GameObject> temp = new List<GameObject>(); //temp list for finding objects on scene start
        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("aiSpawn")) //grabs all spawn points in the scene
        {
            temp.Add(spawn);
        }
        SpawnPoint1 = temp[0].transform;
        SpawnPoint2 = temp[1].transform;
        temp.Clear();
        foreach (GameObject patrol in GameObject.FindGameObjectsWithTag("patrolPoint")) //grabs all patrol poiints in the scene
        {
            temp.Add(patrol);
        }
        if(gameObject.name.Substring(0,1) == "r")
        {
            patrolPoint1 = temp[0].transform;
            patrolPoint2 = temp[1].transform;
            patrolPoint3 = temp[2].transform;
            patrolPoint4 = temp[3].transform;
        }
        temp.Clear();
        foreach (GameObject patrol in GameObject.FindGameObjectsWithTag("patrolPoint2")) //grabs all patrol poiints in the scene
        {
            temp.Add(patrol);
        }
        if (gameObject.name.Substring(0, 1) != "r")
        {
            patrolPoint1 = temp[0].transform;
            patrolPoint2 = temp[1].transform;
            patrolPoint3 = temp[2].transform;
            patrolPoint4 = temp[3].transform;
        }
        temp.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        AIName = gameObject.name; // gets the name of the AI
        if (AIName.Substring(0, 1) == "r") //changed variables for Ripper
        {
            AISpeed = 2.7f;
            AISpeedBoost = 500f;
            IsRipper = true;
        }
        else //changed variables for mutant zombies
        {
            AISpeed = 2.3f;
            AISpeedBoost = 700f;
            IsRipper = false;
            screamChance = 0; //cannot scream
            speedBoostChance = 5; //higher chance at speed boost during chase
        }
        if (NoDestroy.HasBeenTamperedWith) 
        {
            AISpeed = 24f;
        }

        nm = GetComponent<NavMeshAgent>();
        player = GameObject.Find("FPSController");
        pHealth = player.GetComponent<PlayerHealth>();
        if (player) hideScript = player.GetComponentInChildren<PlayerHiding>();

        if (IsRipper) nm.stoppingDistance = 0.75f;
        else nm.stoppingDistance = 0.65f;

        //scream script check
        ScreamScript = GetComponent<AIScream>();
        if (!ScreamScript) ScreamScript = null;
        else ScreamScript.enabled = false;

        visionScript = GetComponentInChildren<VisionAI>();

        farthestSpawn = null;

        aiState = State[4]; //chase0 idle2 patrol4 search5
        //hasBeenSeen = true; //this makes the look counter go down if you want this AKA use if start on chase
    }

    private void Update()
    {
        if (NoDestroy.HasBeenTamperedWith) //Are You A Cheater??
        {
            aiState = "Chase";
            visualAI = true;
            losesPlayerTimer = 10;
            AISpeed = 24f;
            nm.isStopped = false;
            nm.SetDestination(player.transform.position);
            nm.updateRotation = true;
            thisMonster.GetComponent<FindPoints>().enabled = false;
            distanceFromPlayer();
        }
        else //You are playing the game correctly!!
        {
            // functions called every frame
            if (!destroyObj)
            {
                movementAndRotation();
                distanceFromPlayer();

                if (!isScaring) losePlayer();
            }
            else
            {
                if (!IsScreaming) attackDoor();
                IsScreaming = false;
                checkForScreamTimer = 10f;
                screamLength = 5f;
                checkForScreamRandom = 11;
                ScreamScript.enabled = false;
                wasScreamingTimer = 1;
                IsScreaming = false;
            }
        }


    }

    //movement and rotation of the AI depending on what state it's in
    private void movementAndRotation()
    {
        if (aiState == "Chase")
        {
            //main chase functionality
            nm.isStopped = false;
            nm.SetDestination(player.transform.position);
            nm.updateRotation = true;
            thisMonster.GetComponent<FindPoints>().enabled = false;

            //blink-boost logic
            //actually just raises speed super high for short time, but will seem like he blinks forward
            if (!speedBoost)
            {
                nm.speed = AISpeed;
                checkForBoostTimer -= 1 * Time.deltaTime;
                if (checkForBoostTimer <= 0)
                {
                    checkForBoostRandom = UnityEngine.Random.Range(0, 10);
                    if (checkForBoostRandom <= speedBoostChance)
                    {
                        //print("speed Boost!!!");
                        speedBoost = true;
                    }
                    else if (checkForBoostRandom > speedBoostChance)
                    {
                        //print("no dice");
                        checkForBoostTimer = 5f;
                        speedBoostLength = 0.25f;
                        checkForBoostRandom = 11;
                        speedBoost = false;
                    }
                }
            }
            else
            {
                if (speedBoostLength >= 0) 
                {
                    speedBoostLength -= 1 * Time.deltaTime;
                    nm.speed = AISpeedBoost;
                } 
                else
                {
                    checkForBoostTimer = 5f;
                    speedBoostLength = 0.25f;
                    checkForBoostRandom = 11;
                    speedBoost = false;
                }
            }
        }
        else
        {
            checkForBoostRandom = 0;
            checkForBoostTimer = 5;
        }

        if (aiState == "Still")
        {
            nm.isStopped = true;
            nm.updateRotation = false;

        }

        if (aiState == "Patrol")
        {
            //blink-boost logic
            //actually just raises speed super high for short time, but will seem like he blinks forward
            nm.speed = AISpeed;
            if (!IsScreaming)
            {
                nm.isStopped = false;
                //update rot true in function below
                patrolPath();

                checkForScreamTimer -= 1 * Time.deltaTime;
                if (checkForScreamTimer <= 0)
                {
                    checkForScreamRandom = UnityEngine.Random.Range(1, 11);
                    if (checkForScreamRandom <= screamChance)
                    {
                        IsScreaming = true;
                        wasScreaming = true;
                    }
                    else if (checkForScreamRandom > screamChance)
                    {
                        //print("no dice");
                        checkForScreamTimer = 10f;
                        screamLength = 5f;
                        checkForBoostRandom = 11;
                        ScreamScript.enabled = false;
                        IsScreaming = false;
                    }
                }
            }
            else
            {
                if (screamLength >= 0)
                {
                    //countdowns timer for scream and activates scream script
                    screamLength -= 1 * Time.deltaTime;
                    ScreamScript.enabled = true;
                }
                else
                {
                    checkForScreamTimer = 10f;
                    screamLength = 5f;
                    checkForScreamRandom = 11;
                    ScreamScript.enabled = false;
                    IsScreaming = false;
                }
            }
            if (wasScreaming && !IsScreaming)
            {
                wasScreamingTimer -= Time.deltaTime;
            }
            if (wasScreamingTimer <= 0)
            {
                wasScreaming = false;
                wasScreamingTimer = 0.5f;
            }
        }
        else
        {
            checkForScreamTimer = 10f;
            screamLength = 5f;
            checkForScreamRandom = 11;
            ScreamScript.enabled = false;
            wasScreamingTimer = 1;
            IsScreaming = false;
        }

        if (aiState == "Track")
        {
            nm.isStopped = false;
            nm.updateRotation = true;
            nm.speed = AISpeed;
            GameObject searchPointClone;
            searchPointClone = GameObject.FindGameObjectWithTag("searchPoint");
            if (GameObject.FindGameObjectWithTag("searchPoint") && aiState == "Track")
            {
                nm.SetDestination(searchPointClone.transform.position);
            }
            else
            {
                //null reference insurance
                aiState = State[5];
            }

        }

        //THIS SHOULD WORK
        if (aiState == "Search")
        {
            nm.speed = AISpeed;
            thisMonster.GetComponent<FindPoints>().enabled = true;
            if (thisMonster.GetComponent<FindPoints>().searchDone) 
            {
                thisMonster.GetComponent<FindPoints>().enabled = false;
            }
        }

        if (aiState == "ChasePlus")
        {
            nm.isStopped = false;
            nm.SetDestination(player.transform.position);
            nm.updateRotation = true;
            thisMonster.GetComponent<FindPoints>().enabled = false;
            nm.speed = (AISpeed * 2);
            chasePlusTimer -= Time.deltaTime;

            if (chasePlusTimer <= 0)
            {
                nm.speed = AISpeed;
                wasScreaming = false;
                if (losesPlayerTimer < 10) aiState = "Chase";
                else aiState = "Track";
            }
        }
        else
        {
            chasePlusTimer = 10;
        }
    }

    //changing target patrol point
    private void patrolPath()
    {
        nm.updateRotation = true;
        if (!patrolDoOnce)
        {
            patrolNum = patrolNumber[0];
            patrolDoOnce = true;
        }

        if (patrolNum == 1)
        {
            nm.SetDestination(patrolPoint1.position);
        }
        else if (patrolNum == 2)
        {
            nm.SetDestination(patrolPoint2.position);
        }
        else if (patrolNum == 3)
        {
            nm.SetDestination(patrolPoint3.position);
        }
        else if (patrolNum == 4)
        {
            nm.SetDestination(patrolPoint4.position);
        }
    }

    //losing visual on player function
    private void losePlayer()
    {
        if (visualAI && !wasScreaming)
        {
            aiState = State[0];
            losesPlayerTimer = 10;
        }
        if (visualAI && wasScreaming)
        {
            aiState = State[1];
            losesPlayerTimer = 10;
        }

        if (!visualAI && losesPlayerTimer > 0 && hasBeenSeen)
        {
            losesPlayerTimer -= 1 * Time.deltaTime;
        }
        if (!visualAI && losesPlayerTimer <= 0)
        {
            //spawn search point
            Instantiate(searchPoint, player.transform.position, player.transform.rotation);
            aiState = State[3];
            losesPlayerTimer = 10;
            hasBeenSeen = false;
        }

        if (!visionScript.ray1See && !visionScript.ray2See)
        {
            visualAI = false;
            if (losesPlayerTimer <= 0)
            {
                hasBeenSeen = false;
                losesPlayerTimer = 10;
            }
        }

    }

    //jump-scare function
    private void distanceFromPlayer()
    {
        float dist = Vector3.Distance(player.transform.position, thisMonster.position);
        float dist1 = Vector3.Distance(player.transform.position, SpawnPoint1.position);
        float dist2 = Vector3.Distance(player.transform.position, SpawnPoint2.position);
        if (dist <= scareDis && !hideScript.isHiding)
        {
            player.GetComponent<ScareCam>().nameOfAI = AIName;
            player.GetComponent<ScareCam>().enabled = true;
            isScaring = true;
            if (wasScreaming) aiState = State[1];
            else if(!wasScreaming) aiState = State[0];
            //print("I am near enough to player");
            foreach (GameObject spooky in GameObject.FindGameObjectsWithTag("Monster")) //setting only the scaring monster to active to fix glitch
            {
                if(spooky.name != AIName) 
                {
                    spooky.GetComponent<AIMain>().enabled = false;
                }
            }
        }
        else
        {
            if(pHealth.health > 0) player.GetComponent<ScareCam>().enabled = false;
        }

        if (isScaring)
        {
            scareTimer -= 1 * Time.deltaTime;
            if (dist1 > dist2) 
            {
                farthestSpawn = SpawnPoint1;
            }
            else
            {
                farthestSpawn = SpawnPoint2;
            }
        }
        if (isScaring && scareTimer <= 0) 
        {
            nm.Warp(farthestSpawn.position);
            visualAI = false;
            hasBeenSeen = false;
            losesPlayerTimer = 10;
            chasePlusTimer = 10;
            wasScreaming = false;
            pHealth.health--;
            aiState = "Patrol";
            foreach (GameObject spooky in GameObject.FindGameObjectsWithTag("Monster")) //setting all monsters back to active if there are more than one
            {
                spooky.GetComponent<NavMeshAgent>().Warp(farthestSpawn.position);
                spooky.GetComponent<AIMain>().enabled = true;
                spooky.GetComponent<AIMain>().enabled = false; //for insurance
                spooky.GetComponent<AIMain>().enabled = true;
            }
            farthestSpawn = null;
            isScaring = false;
        }
        if (!isScaring)
        {
            scareTimer = 2.5f;
        }
    }

    //Determining what door the AI is attacking
    private void attackDoor()
    {
        string firstLetter = doorTarget.name.Substring(0, 1);

        if (firstLetter == "d") // checks if door so it activates correct script
        {
            doorTarget.GetComponent<DoorOpen>().enabled = true;
            if (doorTarget.GetComponent<DoorOpen>().isOpen && !doorTarget.GetComponent<DoorOpen>().isShutting)
            {
                doorTarget = null;
                destroyObj = false;
                targetMoveObj = null;
                targetRotateObj = null;
                return;
            }
            else if (!doorTarget.GetComponent<DoorOpen>().isOpen || doorTarget.GetComponent<DoorOpen>().isShutting)
            {
                float doorDist = Vector3.Distance(targetMoveObj.transform.position, thisMonster.transform.position);
                attackTimer -= 1 * Time.deltaTime;
                nm.updateRotation = true;

                transform.position = Vector3.MoveTowards(transform.position, targetMoveObj.transform.position, 0.1f);

                if (doorDist <= 0.5f) // makes AI rotate towards door.
                {
                    float easeTime = 50f;

                    Vector3 targetLook = targetRotateObj.transform.position - thisMonster.transform.position;
                    Quaternion lookDoor = Quaternion.LookRotation(targetLook, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDoor, easeTime * Time.deltaTime);

                    nm.isStopped = true;
                }

                if (attackTimer <= 0) // below is when door is destroyed
                {
                    doorTarget.SetActive(false);
                    doorTarget = null;
                    targetMoveObj = null;
                    targetRotateObj = null;
                    destroyObj = false;
                    nm.isStopped = false;
                    if (thisMonster.GetComponent<FindPoints>().targetCopy)
                    {
                        nm.SetDestination(thisMonster.GetComponent<FindPoints>().targetCopy.transform.position);
                    }
                    attackTimer = 3;
                }
            }
        }
        if (firstLetter == "w") // checks if wardrobe so it activates correct script
        {
            doorTarget.GetComponent<WardrobeOpen>().enabled = true;
            if (doorTarget.GetComponent<WardrobeOpen>().isOpen && !doorTarget.GetComponent<WardrobeOpen>().isShutting)
            {
                doorTarget = null;
                destroyObj = false;
                return;
            }
            else if (!doorTarget.GetComponent<WardrobeOpen>().isOpen || doorTarget.GetComponent<WardrobeOpen>().isShutting)
            {
                float doorDist = Vector3.Distance(doorTarget.transform.position, thisMonster.transform.position);

                attackTimer -= 1 * Time.deltaTime;
                nm.updateRotation = true;
                if (doorDist <= 2) // makes AI rotate towards wardrobe.
                {
                    float easeTime = 8f;

                    Vector3 targetLook = doorTarget.transform.position - thisMonster.transform.position;
                    Quaternion lookDoor = Quaternion.LookRotation(targetLook, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDoor, easeTime * Time.deltaTime);

                    nm.isStopped = true;
                }

                if (attackTimer <= 0) //Below is when a wardrobe is destroyed
                {
                    doorTarget.SetActive(false);
                    //code below fixes glitch of ontriggerexit not working if wardrobe,that the player is hiding in, is destroyed
                    float dist = Vector3.Distance(player.transform.position, thisMonster.position);
                    if (dist <= scareDis)
                    {
                        hideScript.HidingFalse();
                        print("hiding wardrobe destroyed");
                    }
                    doorTarget = null;
                    destroyObj = false;
                    nm.isStopped = false;
                    if (thisMonster.GetComponent<FindPoints>().targetCopy)
                    {
                        nm.SetDestination(thisMonster.GetComponent<FindPoints>().targetCopy.transform.position);
                    }
                    attackTimer = 3;
                }
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        //incrementing target partrol point
        if (other.gameObject.tag == "patrolPoint" && patrolDoOnce && IsRipper)
        {
            if (patrolNum == 1)
            {
                patrolNum = patrolNumber[1];
            }
            else if (patrolNum == 2)
            {
                patrolNum = patrolNumber[2];
            }
            else if (patrolNum == 3)
            {
                patrolNum = patrolNumber[3];
            }
            else if (patrolNum == 4)
            {
                patrolNum = patrolNumber[0];
            }
        }
        if (other.gameObject.tag == "patrolPoint2" && patrolDoOnce && !IsRipper)
        {
            if (patrolNum == 1)
            {
                patrolNum = patrolNumber[1];
            }
            else if (patrolNum == 2)
            {
                patrolNum = patrolNumber[2];
            }
            else if (patrolNum == 3)
            {
                patrolNum = patrolNumber[3];
            }
            else if (patrolNum == 4)
            {
                patrolNum = patrolNumber[0];
            }
        }
        //switches AI from tracking state to searching state
        if (other.gameObject.tag == "searchPoint" && aiState == "Track")
        {
            aiState = State[5];
            Destroy(other.gameObject);
        }
    }
}
