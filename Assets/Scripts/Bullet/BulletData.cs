using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable Object/Bullet Data", order = 4)]
public class BulletData : ScriptableObject
{
    [Header("Bullet 기본 정보")]
    public Sprite bulletIcon;           // Bullet 모양
    public BulletCode code;
    public BulletType bulletType;       // Bullet 타입
    public float moveSpeed = 5;         // Bullet 속도
    public uint bulletDamage = 1;       // Bullet 데미지
    public float bulletSize = 0.3f;     // Bullet 크기
    public float lifeTime = 10.0f;      // Bullet 생존 시간
    public float floatDir = 0;          // Bullet Dir 을 위한 변수
    public bool isThrougt;              // Bullet 의 관통 여부
    public bool isParring;              // Bullet 의 패링 여부
    public bool isPlayer;               // Bullet 의 적/아군 여부
}