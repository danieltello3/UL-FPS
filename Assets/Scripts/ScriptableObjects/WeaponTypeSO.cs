using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Weapon")]
public class WeaponTypeSO : ScriptableObject
{
   public new string name;
   public float shootDistance = 4f;
   public float damage = 1f;
   public GameObject shootPS;
}
