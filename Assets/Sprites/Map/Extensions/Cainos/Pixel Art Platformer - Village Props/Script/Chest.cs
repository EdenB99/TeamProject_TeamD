using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [System.Serializable]
        public struct Itemvalue
        {
            public ItemData itemData;
            [SerializeField, Range(1.0f, 100.0f)]
            public float value;
        }

        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Items")]
        public Itemvalue[] items;  // items를 배열로 선언

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
            DropItem();  // 상자가 열릴 때 아이템을 드랍
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }

        public void DropItem()
        {
            float totalValue = 0;
            foreach (var item in items)
            {
                totalValue += item.value;
            }

            float randomValue = Random.Range(0, totalValue);
            float cumulativeValue = 0;

            foreach (var item in items)
            {
                cumulativeValue += item.value;
                if (randomValue <= cumulativeValue)
                {
                    float rand = Random.Range(-0.5f, 0.5f);
                    Vector2 itempos = (Vector2)transform.position + new Vector2(rand, 2);
                    Factory.Instance.MakeItems(item.itemData.code, 1,itempos);
                    break;
                }
            }
        }
    }
}
