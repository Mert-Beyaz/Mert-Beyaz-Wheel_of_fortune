using UnityEngine;

[CreateAssetMenu(fileName = "PoolItem", menuName = "Pooling/Pool Item")]
public class PoolItemSO : ScriptableObject
{
    public GameObject Prefab;
    public int InitialSize = 10;
    public PoolType PoolType;
}