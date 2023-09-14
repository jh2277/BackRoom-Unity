using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed=1.0f;
    public float runSpeed=2.0f;
    public float rotateSpeed=1.0f;
    public float jumpPower=30;
    public Rigidbody rigid;
    //public GameManager gameManager;
    public Animator playerAnimator;
    private Vector2 input;
    private bool isJump;
    private Vector3 targetDirection;
    private Camera mainCamera;
    public float turnSpeedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isJump=false;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If space is pressed, jump
        if(Input.GetKeyDown(KeyCode.Space) && !isJump){
            isJump = true;
            playerAnimator.SetBool("isJump", isJump);
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }

        // // get input from arrowkeys & WASD
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        UpdateTargetDirection();

        if(input == Vector2.zero){ // idle
            playerAnimator.SetBool("isWalk", false);
            playerAnimator.SetBool("isRun", false);
        }
        else{ // moving
            if(targetDirection.magnitude > 0.1f){
                Vector3 lookDirection = targetDirection.normalized;
                Quaternion freeRotation = Quaternion.LookRotation(lookDirection, transform.up);

                var differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;

                var eulerY = transform.eulerAngles.y;

                if(differenceRotation < 0 || differenceRotation > 0){
                    eulerY = freeRotation.eulerAngles.y;
                }

                var euler = new Vector3(0, eulerY, 0);

                this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), rotateSpeed * turnSpeedMultiplier);
            }
            if(Input.GetKey(KeyCode.LeftShift)){
                playerAnimator.SetBool("isWalk", false);
                playerAnimator.SetBool("isRun", true);
                rigid.AddForce(targetDirection * runSpeed, ForceMode.Impulse);
            }
            else{
                playerAnimator.SetBool("isRun", false);
                playerAnimator.SetBool("isWalk", true);
                rigid.AddForce(targetDirection * speed, ForceMode.Impulse);
            }

        }
    }

    public void UpdateTargetDirection(){
        turnSpeedMultiplier = 1.0f;
        var forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;

        var right = mainCamera.transform.TransformDirection(Vector3.right);
        targetDirection = (input.x * right) + (input.y * forward);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Floor"){
            isJump = false;
            playerAnimator.SetBool("isJump", isJump);
            playerAnimator.SetBool("isWalk", false); //tried turning it off to improve jump performance
        }
        // if(other.gameObject.tag == "Item"){
        //     Destroy(other.gameObject);
        //}
    }
    // private void OnTriggerEnter(Collider other) {
    //     if(other.gameObject.tag == "Item"){
    //         gameManager.itemCount++;
    //         gameManager.GetItem(gameManager.itemCount);

    //         Destroy(other.gameObject);
    //         //other.gameObject.setActive(false);
    //     }
    //     // if(other.gameObject.tag == "Finish"){
    //     //     gameManager.MoveNextStage();
    //     // }
    // }
}
