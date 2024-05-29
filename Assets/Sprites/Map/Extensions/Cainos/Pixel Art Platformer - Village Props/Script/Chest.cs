using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;
using static UnityEditor.Progress;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : NPC_Base
    {
        /// <summary>
        /// 아이템이 나올 확률 : 현재 퍼센티지 100
        /// </summary>
        public float totalValue = 100.0f;
        [System.Serializable]
        public struct Itemvalue
        {
            public ItemData itemData;
            /// <summary>
            /// 아이템이 퍼센티지 내에서 등장할 확률
            /// </summary>
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

        Transform Key;

        protected override void Awake()
        {
            Key = transform.GetChild(1);
        }

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
            Key.gameObject.SetActive(false);
            DropItem();  // 상자가 열릴 때 아이템을 드랍
            transform.GetChild(2).gameObject.SetActive(false); //미니맵 마커 제거
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }
        public override void StartDialog()
        {
            if (!isOpened)
            {
                Open();
            }
        }


        public void DropItem()
        {

            for (int i  = 0; i < items.Length; i++)
            {
                float randomValue = Random.Range(0, totalValue);
                if (items[i].value > randomValue)
                {
                    float rand = Random.Range(-0.5f, 0.5f);
                    Vector2 itempos = (Vector2)transform.position + new Vector2(rand, 2);
                    Factory.Instance.MakeItems(items[i].itemData.code, 1, itempos);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isOpened)
            {
                Key.gameObject.SetActive(true);
                isInsideTrigger = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isOpened)
            {
                Key.gameObject.SetActive(false);
                isInsideTrigger = false;
            }
        }
    }
}
