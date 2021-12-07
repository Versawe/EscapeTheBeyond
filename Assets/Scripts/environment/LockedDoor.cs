using UnityEngine;

//some variables to know if a door is locked or not
//or to see if opening/attempting to open a door will cause a story event
public class LockedDoor : MonoBehaviour
{
    public bool IsLocked = false;
    public bool Puzzle2Trigger = false;
    public bool WasFirstOpenedTriggered = false;
    public bool TriggerDialogue = false;
}
