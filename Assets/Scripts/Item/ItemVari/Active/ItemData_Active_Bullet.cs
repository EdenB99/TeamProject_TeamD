using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Active", menuName = "Scriptable Object/Item Active Data", order = 5)]
public class ItemData_Active_Bullet : ItemData, IActivatable
{
    [Header("����� ������ ������")]
    public BulletCode bulletCode;

    public bool ItemActive(Vector2 pos)
    {
        Player player = GameManager.Instance.Player;

        Vector2 playerPos = (Vector2)player.transform.position;

        Vector2 dir = (pos - playerPos).normalized;



        Factory.Instance.MakeBullet(playerPos, dir, bulletCode);

        return true;
    }
}