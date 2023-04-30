using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public Movement Movement;
    public FMOD.Studio.EventInstance jumpInstance;
    public FMODUnity.EventReference jumpReference;
    public FMOD.Studio.EventInstance landInstance;
    public FMODUnity.EventReference landReference;
    bool wasGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpSound();
        LandSound();
    }

    void JumpSound(){
        bool jumpPress = Input.GetButtonDown("Jump");
        if(jumpPress){
            jumpInstance = FMODUnity.RuntimeManager.CreateInstance(jumpReference);
            jumpInstance.start();
            jumpInstance.release();
        }
    }
    void LandSound(){
        if(Movement.isGrounded()){
            Debug.Log("grounded");
            if(!wasGrounded){
                Debug.Log("just landed");
                landInstance = FMODUnity.RuntimeManager.CreateInstance(landReference);
                landInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
                landInstance.start();
                landInstance.release();
            }
            wasGrounded = true;
        }
        else{
            wasGrounded = false;
        }
    }
}
