using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knight_Shadow : MonoBehaviour
{
    public GameObject obj;
    Player player;
    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        transform.position = player.transform.position;
        pos = transform.position;
        pos.y += 0.8f;
        StartCoroutine(Shadow_Attack());
    }

    IEnumerator Shadow_Attack()
    {
        
        yield return new WaitForSeconds(1.0f);

        GameObject line = Instantiate(obj, pos, Quaternion.identity);
        BladeLine lineScript = obj.GetComponent<BladeLine>();
        lineScript.dir = Random.Range(-3, 3);
        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);


    }


}
