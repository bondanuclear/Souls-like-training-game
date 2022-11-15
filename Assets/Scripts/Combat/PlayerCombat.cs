using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsLike.Core;
namespace SoulsLike.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        AnimationHandler animationHandler;


        private void Awake() {
            animationHandler = GetComponent<AnimationHandler>();
        }

        public void HitLightAttack(string animation)
        {
            animationHandler.PlayTargetAnimation(animation, true, true);
        }
        public void HitHeavyAttack(string animation)
        {
            animationHandler.PlayTargetAnimation(animation, true, true);
        }
    }

}