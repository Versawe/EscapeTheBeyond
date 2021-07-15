using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSpawnLocation : MonoBehaviour
{
    public GameObject RelicOBJ;
    private GameObject Instance;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Instance = Instantiate(RelicOBJ, transform.position, transform.rotation);
        Instance.transform.SetParent(gameObject.transform);
    }
}
