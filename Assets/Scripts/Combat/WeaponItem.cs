using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsLike.Core;
namespace SoulsLike.Combat
{
    [CreateAssetMenu(fileName ="New Weapon", menuName = "Create a new weapon/Weapon")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        [Header("Attack animations")]
        public string lightAttack;
        public string heavyAttack;
    }
}

