using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRestart : MonoBehaviour
{
    MapManager mapmanger;
    void Start()
    {
        mapmanger = FindAnyObjectByType<MapManager>();
    }

    public void RestartGen()
    {
        StopCoroutine(Restart());
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.5f);
        mapmanger.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        mapmanger.gameObject.SetActive(true);

    }


}
