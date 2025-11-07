using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ZonesController : MonoBehaviour
{
    [Header("Zone Info")]
    [SerializeField] private int safeZoneIntervals = 5;
    [SerializeField] private int superZoneIntervals = 30;
    [SerializeField] private TextMeshProUGUI ui_text_safe_zone_value;
    [SerializeField] private TextMeshProUGUI ui_text_super_zone_value;

    [Header("Zone BG")]
    [SerializeField] private Sprite ui_sprite_special_zone;
    [SerializeField] private Sprite ui_sprite_default_zone;

    [Header("Top Bar")]
    [SerializeField] List<TextMeshProUGUI> ui_text_topBar_zone_values;
    [SerializeField] private Image ui_image_topBar_currentZone_background_value;

    private void Awake()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        EventBroker.Subscribe(Events.ON_READY_FOR_SPIN, Init);
    }

    private void Init()
    {
        SetZoneInfoText();
        SetTopBar();
    }

    private void SetZoneInfoText()
    {
        if (FurtuneSpinWheelPanel.Instance.SpinCounter % superZoneIntervals == 0)
        {
            ui_image_topBar_currentZone_background_value.sprite = ui_sprite_special_zone;
            FurtuneSpinWheelPanel.Instance.SetWheelUI(2);
        }
        else if (FurtuneSpinWheelPanel.Instance.SpinCounter % safeZoneIntervals == 0)
        {
            ui_image_topBar_currentZone_background_value.sprite = ui_sprite_special_zone;
            FurtuneSpinWheelPanel.Instance.SetWheelUI(1);
        }
        else 
        {
            ui_image_topBar_currentZone_background_value.sprite = ui_sprite_default_zone;
            FurtuneSpinWheelPanel.Instance.SetWheelUI(0);
        }

        var values = (FurtuneSpinWheelPanel.Instance.SpinCounter / superZoneIntervals) + 1;
        ui_text_super_zone_value.SetText((values * superZoneIntervals).ToString());
        var values1 = (FurtuneSpinWheelPanel.Instance.SpinCounter / safeZoneIntervals) + 1;
        ui_text_safe_zone_value.SetText((values1 * safeZoneIntervals).ToString());
    }

    private void SetTopBar()
    {
        var spinCount = FurtuneSpinWheelPanel.Instance.SpinCounter;
        var minValue = Mathf.CeilToInt(ui_text_topBar_zone_values.Count / 2);
        minValue = -minValue;
        for (int i = 0; i < ui_text_topBar_zone_values.Count; i++)
        {
            ui_text_topBar_zone_values[i].SetText(spinCount + minValue > 0 ? (spinCount + minValue).ToString() : "");
            minValue++;
        }
    }

    private void UnSubscribe()
    {
        EventBroker.UnSubscribe(Events.ON_READY_FOR_SPIN, Init);
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
