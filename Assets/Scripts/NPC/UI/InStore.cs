using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InStore : MonoBehaviour
{
    public Button inStore;
    public Button outStore;
    Image dialog;

    private void Awake()
    {
        //inStore.onClick.AddListener(EnterStore);
        outStore.onClick.AddListener(OutStore);
        dialog = GetComponentInChildren<Image>();
    }

    //private void EnterStore()
    //{
    //    SceneManager.LoadScene(1);
    //}

    void OutStore()
    {
        dialog.gameObject.SetActive(false);
    }
}
