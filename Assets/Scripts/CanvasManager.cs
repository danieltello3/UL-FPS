using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
   public static CanvasManager Instance { private set; get; }

   public TextMeshProUGUI health;

   public SpriteAnimationUI healthIndicator;

   public Sprite[] healthFull;
   public Sprite[] health75;
   public Sprite[] health50;
   public Sprite[] health30;
   public Sprite[] health15;
   public Sprite[] death;

   private void Awake()
   {
      Instance = this;
   }

   public void UpdateHealth(float healthValue)
   {
      if (healthValue > 0)
         health.text = healthValue.ToString() + "%";
      else
         health.text = "--";
      UpdateHealthIndicator(healthValue);
   }

   public void UpdateHealthIndicator(float healthvalue)
   {
      if(healthvalue >= 75f)
         healthIndicator.SetSprites(healthFull);

      if(healthvalue < 75 && healthvalue >= 50)
         healthIndicator.SetSprites(health75);

      if(healthvalue < 50 && healthvalue >= 30)
         healthIndicator.SetSprites(health50);

      if (healthvalue < 30 && healthvalue >= 15)
         healthIndicator.SetSprites(health30);

      if (healthvalue < 15 && healthvalue > 0)
         healthIndicator.SetSprites(health15);

      if (healthvalue <= 0)
         healthIndicator.SetSprites(death);

      healthIndicator.Func_PlayUIAnim();
   }
}
