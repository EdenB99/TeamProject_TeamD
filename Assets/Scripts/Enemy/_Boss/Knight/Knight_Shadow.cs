using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Knight_Shadow : MonoBehaviour
{
    public GameObject obj;
    SpriteRenderer sprite;
    Sprite sprite2d;
    Texture2D texture;
    Player player;
    Vector2 pos;
    int dirBlade;

    Boss_Knight knight;

    readonly int Texture2DID = Shader.PropertyToID("_Texture2D");
    readonly int FadeID = Shader.PropertyToID("_Fade");
    float fade = 1.0f;
    bool fadeAction= false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        // 몹 메테리얼 가져오기
        sprite.material = GameManager.Instance.Mobmaterial;
        sprite2d = sprite.sprite;
        texture = sprite2d.texture;
        sprite.material.SetTexture(Texture2DID, texture);
    }

    // Start is called before the first frame update
    void Start()
    {
        knight = FindAnyObjectByType<Boss_Knight>();
        

        dirBlade = Random.Range(-30, 30);
        if ( dirBlade > 0) { sprite.flipX = true; }
        player = GameManager.Instance.Player;
        transform.position = player.transform.position;
        pos = transform.position;
        pos.y += 0.8f;
        StartCoroutine(Shadow_Attack());
    }

    private void Update()
    {


        if ( fadeAction )
        {
            fade += Time.deltaTime * 0.6f;
            sprite.material.SetFloat(FadeID, 1 - fade);
        }
        else
        {
            if ( fade > 0 ) fade -= Time.deltaTime * 0.9f;
            sprite.material.SetFloat(FadeID, 1 - fade);
        }

    }

    IEnumerator Shadow_Attack()
    {
        float dir = dir = Random.Range(87, 93);

        yield return new WaitForSeconds(2.0f);

        knight.MakeBlade(pos, dir);

        yield return new WaitForSeconds(1.0f);

        knight.MakeBlade(pos, dirBlade);
        fadeAction = true;

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);


    }


}
