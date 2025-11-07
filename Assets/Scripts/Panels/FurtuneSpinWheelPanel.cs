using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurtuneSpinWheelPanel : MonoBehaviour, IUIElement
{
    public static FurtuneSpinWheelPanel Instance;

    [SerializeField] private WheelConfig wheelConfig;
    [SerializeField] private int spinCounter = 1;
    [SerializeField] private int maxSpin = 30;
    private int _spinLoopAmount = 1;

    [Header("UI References")]
    [SerializeField] private Image ui_image_wheel_value;
    [SerializeField] private Image ui_image_selector_value;

    public int SpinCounter { get => spinCounter; private set => spinCounter = value; }

    public PanelEnum PanelId => PanelEnum.FurtuneWheelPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetWheelUI(0);
        spinCounter = 1;
        _spinLoopAmount = 1;
        EventBroker.Publish(Events.ON_READY_FOR_SPIN);
    }

    public void SetWheelUI(int index)
    {
        ui_image_wheel_value.sprite = wheelConfig.WheelIUs[index].Ui_sprite_wheel;
        ui_image_selector_value.sprite = wheelConfig.WheelIUs[index].Ui_sprite_selector;
    }

    public List<WheelSliceData> GetWheelSliceData()
    {
        return wheelConfig.WheelSlices[_spinLoopAmount - 1].Slices;
    }

    public void IncreaseSpinCounter()
    {
        spinCounter++;
        _spinLoopAmount++;
        if (_spinLoopAmount > maxSpin) _spinLoopAmount = 1;
    }
}
