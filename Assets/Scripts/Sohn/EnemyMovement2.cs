using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement2 : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private float dist;
    public float followThreshold;
    public Animator enemyAnimator;
    public float runningRange;
    public float runSpeed;
    public float walkSpeed;
    private bool isGoingHome;

    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    Transform spawnPointTransform;

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        runningRange = 50f;
        runSpeed = 4f;
        walkSpeed = 2.5f;
        isGoingHome = false;
    }

    void Update()
    {
        dist = Vector3.Distance(targetTransform.position, transform.position); // distance between player and enemy
        enemyAgent.isStopped = false; // moving
        if(dist < followThreshold && !isGoingHome){ // if the enemy did not meet the boundary, keep going towards player
            enemyAgent.SetDestination(targetTransform.position);
            if(dist <= enemyAgent.stoppingDistance + runningRange){ // if the enemy is near the player
                Run();
            }
            else{ // if the enemy is far away from the player
                Walk();
            }
        }
        else if(isGoingHome){ //if the enemy met the boundary 
            enemyAgent.SetDestination(spawnPointTransform.position);
            Run();
        }
        else{ //if enemy is far away from the player, stop
            enemyAgent.isStopped = true; // stopped
            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetBool("Run", false);
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
            ExitGame();
            //End Game or restart
        }
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

private void OnTriggerEnter(Collider other){
    if(this.gameObject.tag!="Main Enemy" && other.gameObject.tag == "Boundary"){
        enemyAgent.SetDestination(spawnPointTransform.position);
        isGoingHome = true;
    }
    if(other.gameObject.tag == "SpawnPoint"){
        isGoingHome = false;
        enemyAgent.SetDestination(targetTransform.position);
    }
}

}
