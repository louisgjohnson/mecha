using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float knockbackTime = 0.35f;
    float knockbackForce = 5f;
    float knockbackCounter = 0f;
    float gravityValue = -9.81f;

    Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;

    GameObject target;

    void Awake() {
        gameObject.tag = "Enemy";
    }

    void Start() {
        controller = gameObject.AddComponent<CharacterController>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

   int health = 3;
   public void ApplyDamage(int damage, GameObject player) {
       health -= damage;
       if (health <= 0) {
          // Destory enemy
       }
       
       Knockback(player.transform.forward);
   }

   void Update() {
       if (knockbackCounter <= 0) {
           // moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime * 3);
           moveDirection = (target.transform.position - transform.position) * 0.5f;
       } else {
           knockbackCounter -= Time.deltaTime;
       }
       controller.Move(moveDirection * Time.deltaTime);
   }


   public void Knockback(Vector3 direction) {
       knockbackCounter = knockbackTime;
       moveDirection = direction * knockbackForce;
   }
}
