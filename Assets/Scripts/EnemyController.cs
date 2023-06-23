using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
   public float Speed = 2f;
   public float AwakeRadio = 2f;
   public float AttackRadio = .5f;
   public float Health = 5f;
   public GameObject hitboxRight;
   public GameObject hitboxLeft;

   private Animator mAnimator;
   private Rigidbody mRb;
   private NavMeshAgent navMeshAgent;

   private Vector2 mDirection; //xz
   private bool mIsAttacking = false;
   

   void Start()
   {
      mAnimator = transform.GetComponentInChildren<Animator>(false);
      mRb = GetComponent<Rigidbody>();
      navMeshAgent = GetComponent<NavMeshAgent>();
   }

   // Update is called once per frame
   void Update()
   {
      var collider1 = IsPlayerInAttackArea();
      if(collider1 != null && !mIsAttacking)
      {
         mRb.velocity = new Vector3(
            0f,
            0f,
            0f
         );
         navMeshAgent.isStopped = true;
         mAnimator.SetBool("IsWalking", false);
         mAnimator.SetTrigger("Attack");
         return;
      }

      var collider2 = IsPlayerNearby();
      if(collider2 != null && !mIsAttacking)
      {
         //Walk(collider2);
         mAnimator.SetBool("IsWalking", true);
         navMeshAgent.isStopped = false;
         navMeshAgent.SetDestination(collider2.transform.position);

      }
      else
      {
         //parar
         mRb.velocity = Vector3.zero;
         mAnimator.SetBool("IsWalking", false);
         navMeshAgent.isStopped = true;
         navMeshAgent.ResetPath();
      }
   }

   private void Walk(Collider collider2)
   {
      //caminar
      var playerPosition = collider2.transform.position;
      var direction = playerPosition - transform.position;
      mDirection = new Vector2(direction.x, direction.z);

      //transform.LookAt(playerPosition, Vector3.up);
      direction.y = 0f;
      transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.5f);

      mRb.velocity = new Vector3(
         mDirection.x * Speed,
         0f,
         mDirection.y * Speed
      );

      mAnimator.SetBool("IsWalking", true);
      mAnimator.SetFloat("Horizontal", mDirection.x);
      mAnimator.SetFloat("Vertical", mDirection.y);
   }

   private Collider IsPlayerNearby()
   {
      var colliders = Physics.OverlapSphere(transform.position, AwakeRadio, LayerMask.GetMask("Player"));
      if (colliders.Length == 1) return colliders[0];
      else return null;
   }

   private Collider IsPlayerInAttackArea()
   {
      var colliders = Physics.OverlapSphere(transform.position, AttackRadio, LayerMask.GetMask("Player"));
      if (colliders.Length == 1) return colliders[0];
      else return null;
   }

   public void StartAttack()
   {
      mIsAttacking = true;
   }

   public void StopAttack()
   {
      mIsAttacking = false;
      hitboxLeft.SetActive(false);
      hitboxRight.SetActive(false);
   }

   public void EnableHitbox()
   {
      hitboxLeft.SetActive(true);
      hitboxRight.SetActive(true);
   }

   public void TakeDamage(float damage)
   {
      Health -= damage;
      if(Health <= 0)
      {
         Destroy(gameObject);
      }
   }

   //public void OnDrawGizmos()
   //{
   //   Gizmos.DrawSphere(transform.position, AttackRadio);
   //}
}
