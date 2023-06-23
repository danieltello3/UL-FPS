using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private float turnSpeed;
   [SerializeField] private float health = 100f;
   [SerializeField] private GameObject[] Weapons;
   public int weaponIndicator;

   private Rigidbody mRb;
   private Vector2 mDirection;
   private Vector2 mDeltaLook;
   private Transform cameraMain;
   private GameObject debugImpactSphere;
   private GameObject bloodObjectParticle;
   private GameObject otherObjectParticle;
   private GameObject rifleObjectParticle;
   private WeaponTypeSO weaponType;
   private Vector3 weaponPosition;

   void Start()
   {
      mRb = GetComponent<Rigidbody>();
      cameraMain = transform.Find("Main Camera");

      debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
      bloodObjectParticle = Resources.Load<GameObject>("BloodSplat_FX Variant");
      otherObjectParticle = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");
      rifleObjectParticle = Resources.Load<GameObject>("WFX_MF FPS RIFLE2 Variant");

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      SwitchWeapons(0);
      CanvasManager.Instance.UpdateHealth(health);
   }

   void Update()
   {
      mRb.velocity = mDirection.y * speed * transform.forward
         + mDirection.x * speed * transform.right;

      transform.Rotate(
         Vector3.up,
         turnSpeed * Time.deltaTime * mDeltaLook.x
      );

      cameraMain.GetComponent<CameraMovement>().RotateUpDown(
         -turnSpeed * Time.deltaTime * mDeltaLook.y
      );

      weaponPosition = weaponPosition = Weapons[weaponIndicator].GetComponent<Weapon>().shootBox.transform.position;
   }

   private void OnMove(InputValue value)
   {
      mDirection = value.Get<Vector2>();
   }

   private void OnLook(InputValue value)
   {
      mDeltaLook = value.Get<Vector2>();
   }

   private void OnSwitchWeapon(InputValue value)
   {
      if (value.isPressed)
      {
         int weapon = weaponIndicator < Weapons.Length - 1 ? weaponIndicator + 1 : 0;
         SwitchWeapons(weapon);
      }
   }

   private void OnFire(InputValue value)
   {
      if (value.isPressed)
      {
         Shoot();
      }
   }

   private void OnRestart(InputValue value)
   {
      if (value.isPressed)
      {
         GameManager.Instance.Restart();
      }
   }

   private void Shoot()
   {
      var weapontype = weaponType.name;
      
      if(weapontype == "Shotgun")
      {
         var otherObject = Instantiate(otherObjectParticle,weaponPosition, Quaternion.identity);
         otherObject.GetComponent<ParticleSystem>().Play();
         Destroy(otherObject, 3f);
      }
      else
      {
         var otherObject = Instantiate(rifleObjectParticle, weaponPosition, Quaternion.identity);
         otherObject.GetComponent<ParticleSystem>().Play();
         Destroy(otherObject, 0.02f);
      }

      RaycastHit hit;
      if (Physics.Raycast(
         cameraMain.position,
         cameraMain.forward,
         out hit,
         weaponType.shootDistance
      ))
      {
         //var debugSphere = Instantiate(debugImpactSphere, hit.point, Quaternion.identity);
         //Destroy(debugSphere, 3f);
         if (hit.collider.CompareTag("Enemy"))
         {
            var bloodPS = Instantiate(bloodObjectParticle, hit.point, Quaternion.identity);
            Destroy(bloodPS, 3f);
            var enemyController = hit.collider.GetComponent<EnemyController>();
            enemyController.TakeDamage(weaponType.damage);
         }
         else
         {
            var otherObject = Instantiate(otherObjectParticle, hit.point, Quaternion.identity);
            otherObject.GetComponent<ParticleSystem>().Play();
            Destroy(otherObject, 3f);
         }
      }
   }

   private void TakeDamage(float damage)
   {
      health -= damage;
      if (health <= 0f)
      {
         Debug.Log("FIN DEL JUEGO");
         GameManager.Instance.StopGame();
      }
      CanvasManager.Instance.UpdateHealth(health);
   }

   private void OnTriggerEnter(Collider col)
   {
      if (col.CompareTag("EnemyAttack"))
      {
         Debug.Log("player recibio daño");
         TakeDamage(15f);
      }
   }

   private void SwitchWeapons(int index)
   {
      for (int i = 0; i < Weapons.Length; i++)
      {
         Weapons[i].SetActive(false);
      }
      Weapons[index].SetActive(true);
      weaponIndicator = index;
      weaponType = Weapons[index].GetComponent<Weapon>().weaponType;
   }
}
