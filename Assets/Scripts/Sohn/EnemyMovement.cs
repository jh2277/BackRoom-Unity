using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private float dist;
    public float followThreshold;
    public Animator enemyAnimator;
    public float runningRange;
    public float runSpeed;
    public float walkSpeed;
    //Joohyuk Add For GameEnd UI and Sound.
    public GameObject gameOver;
    public AudioSource Sound;
    public AudioClip fail;
    //private bool isGoingHome;



    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    //Transform spawnPointTransform;
    

    void Start()
    {
        Sound = GetComponent<AudioSource>();
        enemyAgent = GetComponent<NavMeshAgent>();
        runningRange = 50f;
        runSpeed = 4f;
        walkSpeed = 2.5f;
    }

    void Update()
    {
        dist = Vector3.Distance(targetTransform.position, transform.position); // distance between player and enemy
        enemyAgent.isStopped = false; // moving
        enemyAgent.SetDestination(targetTransform.position);
        if (dist < followThreshold){ // if the enemy did not meet the boundary, keep going towards player
            if(dist <= enemyAgent.stoppingDistance + runningRange){ // if the enemy is near the player
                Run();
            }
            else{ // if the enemy is far away from the player
                Walk();
            }
        }

        else{ //if enemy is far away from the player, stop
            enemyAgent.isStopped = true; // stopped
            enemyAnimator.SetBool("isWalk", false);
            enemyAnimator.SetBool("isRun", false);
        }
    }
    private void Run(){
        enemyAgent.speed = runSpeed; // increase speed
        enemyAnimator.SetBool("isRun", true); //transition to running animation
        enemyAnimator.SetBool("isWalk", false);
    }
    private void Walk(){
        enemyAgent.speed = walkSpeed; // decrease speed
        enemyAnimator.SetBool("isWalk", true); // transition to walking animation
        enemyAnimator.SetBool("isRun", false);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            Sound.clip = fail;
            Sound.Play();
            gameOver.SetActive(true);
            // ���� Ŭ���� ó�� �߰�
            StartCoroutine(ExitGame(1.5f));
            //End Game or restart
        }
    }
    IEnumerator ExitGame(float delay)
    {
        yield return new WaitForSeconds(delay); // delay �ð�(3��)��ŭ ����մϴ�.

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
