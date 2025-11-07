using UnityEngine;

[CreateAssetMenu(fileName = "WheelSliceData", menuName = "Game/Wheel Slice Data", order = 0)]
public class WheelSliceData : ScriptableObject
{
    public SliceType SliceType;
    public string SpriteName;
    public string RewardText;
    public int RewardValue;

    public Sprite GetSprite()
    {
        var atlas = UIManager.Instance.MainAtlas;
        return atlas.GetSprite(SpriteName);
    }
}