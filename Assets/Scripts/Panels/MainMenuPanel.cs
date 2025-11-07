using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour, IUIElement
{
    [Header("UI References")]
    [SerializeField] private Button ui_button_start;

    public PanelEnum PanelId => PanelEnum.MainMenuPanel;

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
        if (ui_button_start == null)
            ui_button_start = GameObject.Find("ui_button_start")?.GetComponent<Button>();

        if (ui_button_start != null)
        {
            ui_button_start.onClick.RemoveAllListeners();
            ui_button_start.onClick.AddListener(OnStartButtonPressed);
        }
    }
#endif

    private void SetupButton()
    {
        if (ui_button_start != null)
        {
            ui_button_start.onClick.RemoveAllListeners();
            ui_button_start.onClick.AddListener(OnStartButtonPressed);
        }
    }

    private void OnStartButtonPressed()
    {
        UIManager.Instance.ShowPanel(PanelEnum.FurtuneWheelPanel);
        UIManager.Instance.HidePanel(PanelEnum.MainMenuPanel);
    }
}
