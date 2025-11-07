using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour, IUIElement
{
    public PanelEnum PanelId => PanelEnum.RewardCardPanel;
    [SerializeField] private Transform targetTransform;

    [Header("UI References")]
    [SerializeField] private Image ui_image_reward_icon_value;
    [SerializeField] private TextMeshProUGUI ui_text_reward_amount_value;

    private Sequence _reward_Seq;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        var result = SpinManager.Instance.GetSpinResult();
        ui_image_reward_icon_value.sprite = result.GetSprite();
        ui_text_reward_amount_value.SetText(result.RewardText);
        CardAnimations();
    }

    private void CardAnimations()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;

        _reward_Seq?.Kill(true);
        _reward_Seq = DOTween.Sequence();

        _reward_Seq.Append(transform.DOScale(Vector3.one, 1f));

        _reward_Seq.AppendInterval(1f);

        _reward_Seq.Append(transform.DOMove(targetTransform.position, 0.5f));
        _reward_Seq.Join(transform.DOScale(Vector3.zero, 0.5f));

        _reward_Seq.OnComplete(() =>
        {
            EventBroker.Publish(Events.ADD_REWARD_IN_REWARD_PANEL);
            FurtuneSpinWheelPanel.Instance.IncreaseSpinCounter();
            EventBroker.Publish(Events.ON_READY_FOR_SPIN);
            EventBroker.Publish(Events.SET_EXIT_BUTTON, true);
            Hide();
        });
    }
}
