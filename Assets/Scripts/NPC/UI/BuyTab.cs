using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTab : MonoBehaviour
{
    StoreSlot storeSlot;
    Inventory inven;
    Button yesButton;
    Button noButton;
    GameManager manager;
    Player player;

    private void Awake()
    {
        manager = new GameManager();
        player = GameManager.Instance.Player;
        inven = player.PlayerStats.Inventory;
        storeSlot = FindAnyObjectByType<StoreSlot>();

        Transform child = transform.GetChild(1);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(() =>
        {
            Debug.Log("구매 완료");
            gameObject.SetActive(false);
            inven.AddItem(ItemCode.Sword);

        });

        child = transform.GetChild(2);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        //manager = new GameManager();
        //player = GameManager.Instance.Player;
        //inven = player.PlayerStats.Inventory;

        //IngameUI ingame = GameManager.Instance.IngameUI;
        //ingame.SetItemCodeToData(ItemCode.HealingPotion_A);
    }
}
