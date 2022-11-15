using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike.Combat
{
    public class WeaponSlotManager : MonoBehaviour
    {
        [SerializeField] WeaponSlotHolder leftWeaponSlotHolder;
        [SerializeField] WeaponSlotHolder rightWeaponSlotHolder;
        // 
        private void Awake() {
            WeaponSlotHolder[] weaponSlotHolders = GetComponentsInChildren<WeaponSlotHolder>();
            
            foreach(var item in weaponSlotHolders)
            {
                if(item.isLeftHanded) 
                {
                    leftWeaponSlotHolder = item;
                    
                } else if (item.isRightHanded)
                {
                    rightWeaponSlotHolder = item;
                    
                }

            }
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeftHanded)
        {
            
            if(isLeftHanded)
            {
                leftWeaponSlotHolder.LoadWeaponModel(weaponItem);
            } else 
            {
                rightWeaponSlotHolder.LoadWeaponModel(weaponItem);
            }
        }
    }

}