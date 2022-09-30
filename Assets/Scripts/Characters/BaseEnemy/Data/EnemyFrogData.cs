using UnityEngine;

[CreateAssetMenu(fileName = "FrogData", menuName = "Data/Characters/Frog", order = 3)]
public class EnemyFrogData : BaseEnemyData
{
    public LayerMask playerLayerMask;
    public float attackDistance = 1f;
    public float attackCooldownTime = 3f;
    public float posionSpeed = 1f;
}
