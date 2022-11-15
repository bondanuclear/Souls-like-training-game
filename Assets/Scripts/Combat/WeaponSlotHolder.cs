using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike.Combat
{
    public class WeaponSlotHolder : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHanded;
        public bool isRightHanded;
        public GameObject currentWeaponModel;
        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.gameObject.SetActive(false);
            }
        }
        public void UnloadAndDestroyWeapon()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadAndDestroyWeapon();
            if(weaponItem == null)
            {
                UnloadWeapon();    
                return;
            }

            currentWeaponModel = Instantiate(weaponItem.modelPrefab) as GameObject;

            if(currentWeaponModel != null)
            {
                if(parentOverride != null)
                {
                    currentWeaponModel.transform.parent = parentOverride;
                } else currentWeaponModel.transform.parent = transform;

               currentWeaponModel.transform.localPosition = Vector3.zero;
               currentWeaponModel.transform.localRotation = Quaternion.identity;
               currentWeaponModel.transform.localScale = Vector3.one; 
            }
        }

    }

}