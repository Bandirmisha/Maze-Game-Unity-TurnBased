
using UnityEngine;

public readonly struct EnemyRenderData
{
    public EnemyRenderData(int enemyHP, Vector3 enemyPos)
    {
        EnemyHP = enemyHP;
        EnemyPosition = enemyPos;
    }

    public readonly int EnemyHP { get; }
    public readonly Vector3 EnemyPosition { get; }
}
