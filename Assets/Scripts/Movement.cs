using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour

{
    [SerializeField] GameManager GameManager;
    Rigidbody2D rb;
    Animator animator;
    public Collider2D slimeCollider;
    float jPressRemember;
    float groundedRemember;
    float slideRemember;
    bool jumpEnabled;
    public bool isFloating;
    public bool jumpSound;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask ceilingLayer;
    [SerializeField] Transform wallCheckOrigin;
    [SerializeField] Transform ceilingCheckOrigin;
    [SerializeField] Transform ceilingCheckOrigin2;
    [SerializeField] float jumpPower;
    [SerializeField] float fallSpeed;
    [SerializeField] float floatGravity;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float wallCheckRadius;
    [SerializeField] float ceilingCheckRadius;
    [SerializeField] bool gizmos;

    public Timer Timer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slimeCollider = transform.GetComponent<Collider2D>();
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
        if(isFloating){
            //Debug.Log("is floating");
        }
        //Debug.Log(PlayerTouchingCeiling());
        // if(Timer.seconds <= 0f){
        //     Debug.Log("times up from movement! <3");
        // }
        // Debug.Log(jPressRemember);
    }

    public bool isGrounded(){
        Vector2 position = slimeCollider.bounds.center;
        Vector2 size = slimeCollider.bounds.size;
        // float distance = 1f;
        RaycastHit2D hit = Physics2D.BoxCast(position,size,0f,Vector2.down, 1f,ground);
        if(hit.collider != null){
            groundedRemember = 0.1f;
        }
        if(groundedRemember >= 0f){
            return true;
        }
        return false;
    }

    public bool isSliding(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(PlayerTouchingWall() && !isGrounded()){
            return true;
        }
        else {
            return false;
        }
    }

    public bool PlayerTouchingWall(){
        return Physics2D.OverlapCircle(wallCheckOrigin.position,wallCheckRadius,wallLayer);
    }
    public bool PlayerTouchingCeiling(){
        if(Physics2D.OverlapCircle(ceilingCheckOrigin.position,ceilingCheckRadius,ceilingLayer)){
            return true;
        }

        if(Physics2D.OverlapCircle(ceilingCheckOrigin2.position,ceilingCheckRadius,ceilingLayer)){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isPlayerFalling(){
        if(rb.velocity.y < 0f){
            return true;
        }
        return false;
    }

    void processInput(){

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool jumpHold = Input.GetButton("Jump");
        bool jumpPress = Input.GetButtonDown("Jump");
        bool jumpRelease = Input.GetButtonUp("Jump");
        bool jPressed = Input.GetKey(KeyCode.J);

        jPressRemember -= Time.deltaTime;
        groundedRemember -= Time.deltaTime;
        slideRemember -= Time.deltaTime;
        if(GameManager.alive){
        //Jumping
        if(jPressRemember > 0){
            PlayerJump();
            rb.gravityScale = 10;
        }
        if(jumpPress){
            jumpEnabled = true;
            jPressRemember = 0.1f;
        }
        // if jumping is released set the upwards velocity to 0 
        // allows player to short jump
        if(jumpRelease && !isPlayerFalling()){
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        
        // Movement
        MovePlayer(acceleration);

        //slowdown / slide / skid
        if(horizontal == 0f){
            rb.velocity = new Vector2(rb.velocity.x * 0.6f, rb.velocity.y);
        }

        //Wall Slide
        if(isSliding() && horizontal !=0f){
            rb.gravityScale = floatGravity;
        }
        
        //fast fall
        if(isPlayerFalling() && !isSliding() && !PlayerTouchingCeiling()){
            if(jumpHold && !isGrounded()){
                isFloating = true;
                rb.gravityScale = floatGravity;
                rb.velocity = new Vector2((rb.velocity.x * 0.9f), rb.velocity.y);
            }
            else{
                isFloating = false;
                rb.gravityScale = 10;
                pushPlayerDown();
            }
        }

        //sliding
        if(isSliding()){
            slideRemember = 0.2f;
        }

        //sticky
        if(vertical > 0.1f){
            StickToCeiling();
        }
        if(!PlayerTouchingCeiling()){
            maxSpeed = 30f;
        }
        }
    }

    void PlayerJump(){
        if(isGrounded() && jumpEnabled || !isGrounded() && slideRemember > 0f && jumpEnabled){
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            jumpEnabled = false;
            
        }
    }

    void MovePlayer(float acceleration){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float velocity = rb.velocity.x;

        if(velocity < 0f){
            velocity = velocity * -1;
        }

        if(isGrounded() || !isGrounded() && PlayerTouchingCeiling()){
            if(velocity <= maxSpeed){
            rb.AddForce(new Vector2(horizontal * acceleration, 0f),ForceMode2D.Impulse);
            }
        }
        //if player is in the air they move slower
        else{
            //Debug.Log("player moving in air");
            rb.AddForce(new Vector2((horizontal) * 0.1f, 0f),ForceMode2D.Impulse);
        }
    }

    void pushPlayerDown(){
        if(!PlayerTouchingCeiling()){
            rb.AddForce(new Vector2(rb.velocity.x, fallSpeed));
        }
    }

    void StickToCeiling(){
        bool jumpPress = Input.GetButtonDown("Jump");
        if(PlayerTouchingCeiling() && !isGrounded()){
            rb.gravityScale = 0;
            maxSpeed = 15f;
        }
        else {
            rb.gravityScale = 10;
            maxSpeed = 30f;
        }
    }

    //if player is not grounded and is touching wall and input is towards wall - lower rb.gravity

    void OnDrawGizmos()
{
    if(gizmos){
    slimeCollider = transform.GetComponent<Collider2D>();
    Gizmos.color = Color.red;
    Vector2 position = slimeCollider.bounds.center;
    Vector2 size = slimeCollider.bounds.size;
    //Gizmos.DrawSphere(ceilingCheckOrigin.position,ceilingCheckRadius);
    Gizmos.DrawSphere(wallCheckOrigin.position,wallCheckRadius);
    //Gizmos.DrawSphere(wallCheckOrigin.position,wallCheckRadius);
    Gizmos.DrawCube(position,size);
    //RaycastHit2D hit = Physics2D.BoxCast(position,size,0f,Vector2.down, 1f,ground);
    }
}

}
