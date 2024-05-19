using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    private void Awake()
    {
        Remover.RemoveObjMark(gameObject);
    }

}
