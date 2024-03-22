using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Editor를 상속받으면 에디터에서만 작동함 , true는 자식도 받겠다는 뜻
[CustomEditor(typeof(EnemyBase_) , true)]
public class EnemyEditor : Editor
{
    // EnemyEditor와 Enemy는 별개의 클래스이므로 실제 선택된 Enemy를 찾아올수 있어야함
    public EnemyBase_ selected;

    // Editor에서 OnEnable은 실제 에디터에서 오브젝트를 눌렀을때 활성화됨
    private void OnEnable()
    {
        // target은 Editor에 있는 변수로 선택한 오브젝트를 받아옴.
        if (AssetDatabase.Contains(target))
        {
            selected = null;
        }
        else
        {
            // target은 Object형이므로 Enemy로 형변환
            selected = (EnemyBase_)target;
        }
    }

    // 유니티가 인스펙터를 GUI로 그려주는함수
    public override void OnInspectorGUI()
    {
        if (selected == null)
            return;

        Texture2D texture;
        SpriteRenderer spriteRenderer = (selected).GetComponent<SpriteRenderer>();
        EditorGUILayout.LabelField(" 몬스터 스프라이트 ");
        texture = SpriteToTexture2D(spriteRenderer.sprite);

        if (texture != null)
        {
            GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("설명용");
        EditorGUILayout.Space();

        GUI.color = Color.white;
        selected.maxHp = EditorGUILayout.IntField("몬스터 체력", selected.Hp);
        if (selected.maxHp < 0)
            selected.maxHp = 1;

        selected.mobDamage = EditorGUILayout.IntField("몬스터 공격력", selected.mobDamage);


        selected.IsMove = EditorGUILayout.Toggle("몬스터 이동 여부", selected.IsMove);

        selected.mobMoveSpeed = EditorGUILayout.FloatField("몬스터 이동속도", selected.mobMoveSpeed);
        if (selected.mobMoveSpeed < 0)
            selected.mobMoveSpeed = 0;

        // 즉시 죽인다.
        if (GUILayout.Button("죽이기"))
        {
            selected.Die();
        }
    }

    /// <summary>
    /// 스프라이트를 텍스쳐2D로 바꿔주는 메서드
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}

