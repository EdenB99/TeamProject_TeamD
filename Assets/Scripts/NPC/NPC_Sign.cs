using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;

public class NPC_Sign : MonoBehaviour
{
    private Collider2D trigger;
    private TextMeshProUGUI Text;
    private GameObject TextObject;

    private void Awake()
    {
        trigger = GetComponent<Collider2D>();
        
        Text = GetComponentInChildren<TextMeshProUGUI>();
        TextObject = transform.GetChild(0).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        TextObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) TextObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) TextObject.SetActive(false);
    }
}
