using UnityEngine;

//This triggers a local Post-Processing Volume that is attached to the player when scary stuff happens
public class LocalPostProcessing : MonoBehaviour
{
    public GameObject playerPP;

    // Update is called once per frame
    void Update()
    {
        if (NoDestroy.stairSpawnCount >= 7) playerPP.SetActive(false);
        if (!NoDestroy.TriggerScarePP && !NoDestroy.TriggerScarePPAI && !NoDestroy.completedQandA) playerPP.SetActive(false);
        if (!NoDestroy.TriggerScarePPAI && !NoDestroy.TriggerScarePP) return;
        if (NoDestroy.stairSpawnCount >= 7) return;
        playerPP.SetActive(true);
    }
}
