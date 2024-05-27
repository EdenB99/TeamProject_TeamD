using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{

    /// <summary>
    /// 카메라가 찍는 화면 크기
    /// </summary>
    const int mapTexWidth = 1920;
    const int mapTexHeight = 1080;
    RenderTexture mapTex;
    public Sprite mapSprite;
    SpriteRenderer img;

    Sprite sprite2D;
    Texture2D texture;
    MainCamera mainCamera;

    readonly int Texture2DID = Shader.PropertyToID("_MainTex");
    protected readonly int FadeID = Shader.PropertyToID("_Fade");
    protected float fade = 0.0f;


    private void Awake()
    {
        Remover.RemoveObjMark(this.gameObject);
        img = GetComponent<SpriteRenderer>();
        mainCamera = GameManager.Instance.MainCamera;
    }

    private void Start()
    {
        img.sprite = null;

        Color color = new (0, 0, 0, 0);

        img.color = color;

    }

    private void Update()
    { 

        Vector3 pos = mainCamera.transform.position;
        transform.position = new Vector3(pos.x, pos.y, 1);
        if (img.sprite != null)
        {
            fade += Time.deltaTime * 1.6f;

            texture = (Texture2D)img.material.GetTexture(Texture2DID);
            img.material.SetFloat(FadeID, 1.2f - fade );
            Color color = new(0, 0, 0, fade);

            if ( fade > 1 )
            {
                img.sprite = null;
                img.color = color;
            }
        }
    }

    /// <summary>
    /// 씬이 변경되었을때 호출하는 함수 (연출용)
    /// </summary>
    public void SceneChanged()
    {
        StartCoroutine(SceneChangeCoroutine());
    }

    private IEnumerator SceneChangeCoroutine()
    {
        // 화면 캡쳐 전에 이미지 비활성화
        img.sprite = null;

        // 생성
        mapTex = new RenderTexture(mapTexWidth, mapTexHeight, 24);
        Camera camera = Camera.main;
        camera.targetTexture = mapTex;
        camera.Render();

        // 텍스쳐 변환
        RenderTexture.active = mapTex;
        Texture2D screenTexture = new Texture2D(mapTexWidth, mapTexHeight, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, mapTexWidth, mapTexHeight), 0, 0);
        screenTexture.Apply();

        // 복원
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(mapTex);

        // 화면 캡쳐를 스프라이트로 변환
        mapSprite = Sprite.Create(screenTexture, new Rect(0, 0, mapTexWidth, mapTexHeight), new Vector2(0.5f, 0.5f));
        img.sprite = mapSprite;

        // 텍스처를 셰이더에 적용
        img.material.SetTexture(Texture2DID, screenTexture);
        img.color = new Color(1, 1, 1, 1);  

        // 페이드 초기화
        fade = 0.0f;

        yield return null;
    }

}
