using UnityEngine.SceneManagement;
using UnityEngine;

//This is a script that will switch scenes after the audio scene has finished playing
public class CarCrashAudio : MonoBehaviour
{
    AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!source.isPlaying) 
        {
            SceneManager.LoadScene("GlitchyStart");
        }
    }
}
