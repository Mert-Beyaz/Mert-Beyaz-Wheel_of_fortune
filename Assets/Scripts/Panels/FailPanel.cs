using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour, IUIElement
{
    [Header("UI References")]
    [SerializeField] private Button ui_button_giveUp;
    [SerializeField] private Button ui_button_revive;

    public PanelEnum PanelId => PanelEnum.FailPanel;

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
        EventBroker.Publish(Events.SET_EXIT_BUTTON, false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ui_button_giveUp == null)
            ui_button_giveUp = GameObject.Find("ui_button_giveUp")?.GetComponent<Button>();

        if (ui_button_revive == null)
            ui_button_revive = GameObject.Find("ui_button_revive")?.GetComponent<Button>();

        if (ui_button_giveUp != null)
        {
            ui_button_giveUp.onClick.RemoveAllListeners();
            ui_button_giveUp.onClick.AddListener(OnGiveUpButtonPressed);
        }

        if (ui_button_revive != null)
        {
            ui_button_revive.onClick.RemoveAllListeners();
            ui_button_revive.onClick.AddListener(OnReviveButtonPressed);
        }
    }
#endif

    private void SetupButton()
    {
        if (ui_button_giveUp != null)
        {
            ui_button_giveUp.onClick.RemoveAllListeners();
            ui_button_giveUp.onClick.AddListener(OnGiveUpButtonPressed);
        }

        if (ui_button_giveUp != null)
        {
            ui_button_revive.onClick.RemoveAllListeners();
            ui_button_revive.onClick.AddListener(OnReviveButtonPressed);
        }
    }

    private void OnGiveUpButtonPressed()
    {
        EventBroker.Publish(Events.SET_EXIT_BUTTON, true);
        UIManager.Instance.ShowPanel(PanelEnum.MainMenuPanel);
        UIManager.Instance.HidePanel(PanelEnum.FailPanel);
        UIManager.Instance.HidePanel(PanelEnum.FurtuneWheelPanel);
    }

    private void OnReviveButtonPressed()
    {
        EventBroker.Publish(Events.SET_EXIT_BUTTON, true);
        EventBroker.Publish(Events.SET_SPIN_BUTTON);
        UIManager.Instance.HidePanel(PanelEnum.FailPanel);
    }
}
