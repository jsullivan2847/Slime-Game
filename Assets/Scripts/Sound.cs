using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference jump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpSound();
    }

    void JumpSound(){
        bool jumpPress = Input.GetButtonDown("Jump");
        if(jumpPress){
            instance = FMODUnity.RuntimeManager.CreateInstance(jump);
            instance.start();
            Debug.Log("should be playing");
        }
    }
}
