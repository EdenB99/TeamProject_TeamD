using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable Object/Bullet Data", order = 4)]
public class BulletData : ScriptableObject
{
    [Header("Bullet �⺻ ����")]
    public Sprite bulletIcon;           // Bullet ���
    public BulletCode code;
    public BulletType bulletType;       // Bullet Ÿ��
    public float moveSpeed = 5;         // Bullet �ӵ�
    public uint bulletDamage = 1;       // Bullet ������
    public float bulletSize = 0.3f;
    public bool isThrougt;              // Bullet �� ���� ����
    public bool isParring;              // Bullet �� �и� ����
    public bool isPlayer;               // Bullet �� ��/�Ʊ� ����
}