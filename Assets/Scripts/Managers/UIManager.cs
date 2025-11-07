using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Sprite Atlases")]
    [SerializeField] private SpriteAtlas mainAtlas;
    public SpriteAtlas MainAtlas => mainAtlas;

    [SerializeField] private List<MonoBehaviour> panelScripts;

    private Dictionary<PanelEnum, IUIElement> _panels = new();
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
        SetupPanels();
    }

    private void SetupPanels()
    {
        foreach (var panel in panelScripts)
        {
            if (panel is IUIElement ui)
            {
                _panels.Add(ui.PanelId, ui);
            }
        }
    }

    public void ShowPanel(PanelEnum id)
    {
        _panels[id].Show();
    }

    public void HidePanel(PanelEnum id)
    {
        _panels[id].Hide();
    }
}

