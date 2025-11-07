using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour, IUIElement
{
    [Header("UI References")]
    [SerializeField] private Button ui_button_collectReward;
    [SerializeField] private Button ui_button_goBack;

    public PanelEnum PanelId => PanelEnum.ExitPanel;

    private void Awake()
    {
        SetupButton();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ui_button_collectReward == null)
            ui_button_collectReward = GameObject.Find("ui_button_collectReward")?.GetComponent<Button>();

        if (ui_button_goBack == null)
            ui_button_goBack = GameObject.Find("ui_button_goBack")?.GetComponent<Button>();

        if (ui_button_collectReward != null)
        {
            ui_button_collectReward.onClick.RemoveAllListeners();
            ui_button_collectReward.onClick.AddListener(OnCollectRewardButtonPressed);
        }

        if (ui_button_goBack != null)
        {
            ui_button_goBack.onClick.RemoveAllListeners();
            ui_button_goBack.onClick.AddListener(OnGoBackButtonPressed);
        }
    }
#endif

    private void SetupButton()
    {
        if (ui_button_collectReward != null)
        {
            ui_button_collectReward.onClick.RemoveAllListeners();
            ui_button_collectReward.onClick.AddListener(OnCollectRewardButtonPressed);
        }

        if (ui_button_goBack != null)
        {
            ui_button_goBack.onClick.RemoveAllListeners();
            ui_button_goBack.onClick.AddListener(OnGoBackButtonPressed);
        }
    }

    private void OnCollectRewardButtonPressed()
    {
        EventBroker.Publish(Events.ON_TAKE_REWARD);
        UIManager.Instance.ShowPanel(PanelEnum.MainMenuPanel);
        UIManager.Instance.HidePanel(PanelEnum.ExitPanel);
        UIManager.Instance.HidePanel(PanelEnum.FurtuneWheelPanel);
    }

    private void OnGoBackButtonPressed()
    {
        UIManager.Instance.HidePanel(PanelEnum.ExitPanel);
    }
}
