using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        SceneManager.LoadScene("Town");
    }


}
