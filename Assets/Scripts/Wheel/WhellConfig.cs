using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WheelConfig", menuName = "Game/Wheel Config", order = 1)]
public class WheelConfig : ScriptableObject
{
    public List<WheelData> WheelSlices;
    public List<WheelUI> WheelIUs;
}

[System.Serializable]
public class WheelData
{
    public List<WheelSliceData> Slices;
}


[System.Serializable]
public class WheelUI
{
    public Sprite Ui_sprite_wheel;
    public Sprite Ui_sprite_selector;
}
