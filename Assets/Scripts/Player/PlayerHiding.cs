using UnityEngine;

public class PlayerHiding : MonoBehaviour
{

    //This script makes it so that the player cannot be seen if hiding in a wardrobe
    //PROBLEM: if wardrobe is open and AI is staring at player, he still cannot see the player, need to check if wardrobe is open or closed...
    public bool isHiding = false;

    public bool inBounds = false;

    public GameObject wardrobeHiding;

    private void Start()
    {
        //null at start
        wardrobeHiding = null;
    }

    private void Update()
    {
        //if you are inside of a wardrobe
        if (wardrobeHiding != null)
        {
            //decides if you are hiding or not determined on if the door is open or not
            //this will help the AI seeing you and jump scare to not be triggered when near you if the wardrobe doors are closed
            if (inBounds && wardrobeHiding.GetComponent<WardrobeOpen>().isOpen) 
            {
                isHiding = false;
            }
            else
            {
                isHiding = true;
            }

        }

    }

    //might remove these functions, but i may be using one of them in Ripper's Main script
    public void HidingTrue()
    {
        isHiding = true;
    }
    public void HidingFalse()
    {
        isHiding = false;
        wardrobeHiding = null;
    }
    
    //trigger colliders changing variables on entering and exiting inside zone of wardrobes
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "hide")
        {
            inBounds = true;
            wardrobeHiding = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "hide")
        {
            HidingFalse();
            inBounds = false;
        }
    }
}
