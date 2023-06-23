using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { private set; get; }

   [SerializeField] private int NumberZombies;
   [SerializeField] private GameObject pfZombie;
   [SerializeField] private GameObject GameOverUI;
   [SerializeField] private PlayerController playerController;

   private float spawnTime = 60f;
   private float xPos;
   private float zPos;
   private int numberSpawned = 0;

   private List<GameObject> enemiesList = new List<GameObject>();

   private void Awake()
   {
      Instance = this;
      GameOverUI.SetActive(false);
   }

   private void Start()
   {
      StartCoroutine(SpawnEnemies());
   }

   IEnumerator SpawnEnemies()
   {
      while (spawnTime >= 10f)
      {
         Debug.Log("ENTRA");
         while(numberSpawned < NumberZombies)
         {
            xPos = Random.Range(-15.74f, 4.79f);
            zPos = Random.Range(1.26f, 4.63f);
            var enemy = Instantiate(pfZombie, new Vector3(xPos, 0.03f, zPos), Quaternion.identity);
            enemiesList.Add(enemy);
            yield return new WaitForSeconds(0.1f);
            numberSpawned += 1;
         }
         yield return new WaitForSeconds(spawnTime);
         Debug.Log($"PASO {spawnTime} segundos");
         if(spawnTime > 10f)
         {
            spawnTime -= 5f;
         }
         numberSpawned = 0;
      }
   }

   public void StopGame()
   {
      spawnTime = 0f;
      StopAllCoroutines();
      foreach(var enemy in enemiesList)
      {
         Destroy(enemy);
      }
      playerController.enabled = false;
      GameOverUI.SetActive(true);
   }

   public void Restart()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}
