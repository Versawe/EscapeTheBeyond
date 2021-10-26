using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPostProcessing : MonoBehaviour
{
    public GameObject playerPP;

    // Update is called once per frame
    void Update()
    {
        if (!NoDestroy.TriggerScarePP && !NoDestroy.TriggerScarePPAI) playerPP.SetActive(false);
        if (!NoDestroy.TriggerScarePPAI && !NoDestroy.TriggerScarePP) return;
        playerPP.SetActive(true);
    }
}
