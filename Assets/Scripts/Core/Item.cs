using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoulsLike.Core
{
    [CreateAssetMenu(fileName = "New item", menuName = "Create an item/Item")]
    public class Item : ScriptableObject
    {
        public Sprite sprite;
        public string itemName;
    }

}
