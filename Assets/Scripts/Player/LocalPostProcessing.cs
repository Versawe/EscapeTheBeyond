using UnityEngine;

public class LocalPostProcessing : MonoBehaviour
{
    public GameObject playerPP;

    // Update is called once per frame
    void Update()
    {
        if (!NoDestroy.TriggerScarePP && !NoDestroy.TriggerScarePPAI && !NoDestroy.completedQandA) playerPP.SetActive(false);
        if (!NoDestroy.TriggerScarePPAI && !NoDestroy.TriggerScarePP) return;
        playerPP.SetActive(true);
    }
}
