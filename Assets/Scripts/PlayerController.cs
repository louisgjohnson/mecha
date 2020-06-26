using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField]
    float boostTimer = 0.15f;
    [SerializeField]
    float boostForce = 1.1f;
    float boostCounter = 0f;
    Vector3 boostVelocity = Vector3.zero;

    GameObject target = null;
    int target_index = 0;

    void Awake() {
        gameObject.tag = "Player";
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        FindClosestEnemy();
    }


    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDistance = float.PositiveInfinity;
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (lowestDistance > distance)
            {
                lowestDistance = distance;
                target = enemy;
                target_index = i;
            }
        }
    }

    void SwapTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 1)
        {
            target_index = target_index + 1 >= enemies.Length ? 0 : target_index + 1;
            target = enemies[target_index];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwapTarget();
        }

        
      

        if (target)
        {
            var targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 3, 0.0f);
            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);
            transform.forward = newDirection;

            if (Input.GetKeyDown(KeyCode.V) && Vector3.Distance(transform.position, target.transform.position) <= 2.0f) {
                target.GetComponent<Enemy>().ApplyDamage(1, gameObject);
            }
        }

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        move.Normalize();
        Vector3 direction = move;

        if (Input.GetKeyDown(KeyCode.L)) {
            Boost(move);
        } 

        if (boostCounter > 0) {
            boostCounter -= Time.deltaTime;
            direction = boostVelocity;
        }

        controller.Move(direction * Time.deltaTime * playerSpeed);
    }

    public void Boost(Vector3 direction) {
       boostCounter = boostTimer;
       boostVelocity = direction * boostForce;
   }
}