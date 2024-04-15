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

    Boss_Knight knight;

    // Start is called before the first frame update
    void Start()
    {
        knight = FindAnyObjectByType<Boss_Knight>();

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
        float dir = dir = Random.Range(87, 93);

        yield return new WaitForSeconds(1.0f);

        knight.MakeBlade(pos, dir);

        yield return new WaitForSeconds(1.0f);

        knight.MakeBlade(pos, dirBlade);

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);


    }


}
