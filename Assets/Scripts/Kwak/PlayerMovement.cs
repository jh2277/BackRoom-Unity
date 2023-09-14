using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Rigidbody rigid;

    //점프
    private bool isJump;
    public float jumpPower;
    //게임 매니저
    //public GameManager gameManager;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            isJump = true;
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v) * speed, ForceMode.Impulse);
        //transform.Translate(new Vector3(h,0,v) * speed); 얘를 쓰면 x축 y축이 움직임. rigid.addforce를 써서 절대 힘으로 미는 느낌으로 하자.
    }
   
    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Floor")
    //    {
    //        isJump = false;
    //    }

    //    if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
    //    {
    //        gameManager.EatItem(other.gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (gameObject.layer == LayerMask.NameToLayer("Item"))
        //{
        //    gameManager.itemCount++;
        //    gameManager.GetItem(gameManager.itemCount); //UI에 나타내기 위함.
        //    Destroy(other.gameObject);
        //}

        if (other.gameObject.tag == "Finish")
        {
            //gameManager.NextStage(); //UI에 나타내기 위함.
        }

        if (other.gameObject.tag == "Fall")
        {
            //SceneManager.LoadScene(gameManager.stage);
        }
    }
}



