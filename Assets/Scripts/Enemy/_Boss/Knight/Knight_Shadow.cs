using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Knight_Shadow : MonoBehaviour
{
    public GameObject obj;
    SpriteRenderer sprite;
    Player player;
    Vector2 pos;
    int dirBlade;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        dirBlade = Random.Range(-30, 30);

        if ( dirBlade > 0) { sprite.flipX = true; }
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
        BladeLine lineScript = line.GetComponent<BladeLine>();
        lineScript.dir = Random.Range(87, 93);
        yield return new WaitForSeconds(1.0f);

        GameObject line2 = Instantiate(obj, pos, Quaternion.identity);
        BladeLine lineScript2 = line2.GetComponent<BladeLine>();
        lineScript2.dir = dirBlade;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);


    }


}
