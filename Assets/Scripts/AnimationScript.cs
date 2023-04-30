using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] Movement Movement;
    [SerializeField] Sprite facingLeft;
    [SerializeField] Sprite facingRight;
    [SerializeField] Sprite regular;
    [SerializeField] Sprite upsideDown;

    Animator animator;
    SpriteRenderer sr;
    Vector3 colliderBounds;

    bool isFacingLeft;
    bool isFacingRight;
    bool isUpsideDown;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // trying to get the collider to change to match sprite when the sprite changes.....
        // Vector3 colliderBounds = Movement.slimeCollider.bounds.size;
        //  float newY = colliderBounds.x;
        //     colliderBounds.x = colliderBounds.y;
        //     colliderBounds.y = newY;
        //     Debug.Log(colliderBounds);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Movement.PlayerTouchingCeiling());
        //right
        if(Movement.isSliding() && Input.GetAxis("Horizontal") > 0f){
            sr.sprite = regular;
            transform.SetLocalPositionAndRotation(transform.position,new Quaternion(0f,0f,0.5f,0.5f));
        }
        //left
        else if(Movement.isSliding() && Input.GetAxis("Horizontal") < 0f){
            sr.sprite = regular;
            transform.SetLocalPositionAndRotation(transform.position,new Quaternion(0f,0f,-0.5f,0.5f));
        }
        //upside down
        // else if(Movement.PlayerTouchingCeiling() && Input.GetAxis("Vertical") > 0f){
        //     animator.SetBool("upsideDown", true);
        // }
        //idle
        else if(!Movement.isSliding() && !Movement.PlayerTouchingCeiling()){
            transform.SetLocalPositionAndRotation(transform.position,new Quaternion(0f,0f,0f,0.5f));
            sr.sprite = regular;
        }
        //jump
        JumpAnimation();
        UpsideDown();
        FloatAnimation();
    }

    void JumpAnimation(){
        if(Input.GetButtonDown("Jump")){
            animator.SetBool("isJumping",true);
        }
        else{
            animator.SetBool("isJumping",false);
        }
    }

    void FloatAnimation(){
        if(Movement.isFloating && !Movement.isGrounded() && !Movement.isSliding()){
            //Debug.Log("animation is floating");
            animator.SetBool("isFloating",true);
        }
        else{
            //Debug.Log("animation is NOT floating");
            animator.SetBool("isFloating",false);
        }
    }
    

    void UpsideDown(){
        if(Movement.PlayerTouchingCeiling() && Input.GetAxis("Vertical") > 0f){
            animator.SetBool("upsideDown", true);
        }
        else if(!Movement.PlayerTouchingCeiling()){
            animator.SetBool("upsideDown", false);
        }
    }

    IEnumerator onDead(){
        animator.SetBool("isExploding",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isExploding",false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Hazzard"){
            StartCoroutine(onDead());
        }
    }
}
