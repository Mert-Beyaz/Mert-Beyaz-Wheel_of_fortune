using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinManager : MonoBehaviour
{
    public static SpinManager Instance;

    [Header("Wheel Settings")]
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private Transform selectorTransform;

    [Header("Spin Settings")]
    [Range(1, 20)][SerializeField] private int spinRounds = 5;
    [Range(2, 20)][SerializeField] private int sliceCount = 8;
    [SerializeField] private bool randomResult = true;
    [SerializeField] private int manualResultIndex = 0;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private AnimationCurve spinEaseCurve;

    [Header("UI References")]
    [SerializeField] private Button ui_button_spin;

    private bool _isSpinning = false;
    private Sequence _spinSeq;
    private int _resultIndex;

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
        SetupButton();
        Subscribe();
    }

    private void OnEnable()
    {
        OnActiveSpin();
    }

    private void Subscribe()
    {
        EventBroker.Subscribe(Events.ON_READY_FOR_SPIN, OnActiveSpin);
        EventBroker.Subscribe(Events.SET_SPIN_BUTTON, OnActiveSpin);
    }



#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ui_button_spin == null)
            ui_button_spin = GameObject.Find("ui_button_spin")?.GetComponent<Button>();

        if (ui_button_spin != null)
        {
            ui_button_spin.onClick.RemoveAllListeners();
            ui_button_spin.onClick.AddListener(OnSpinButtonPressed);
        }
    }
#endif

    private void SetupButton()
    {
        if (ui_button_spin != null)
        {
            ui_button_spin.onClick.RemoveAllListeners();
            ui_button_spin.onClick.AddListener(OnSpinButtonPressed);
        }
    }

    private void OnSpinButtonPressed()
    {
        ui_button_spin.interactable = false;
        EventBroker.Publish(Events.SET_EXIT_BUTTON, false);
        Spin();
    }

    private void OnActiveSpin()
    {
        ui_button_spin.interactable = true;
    }

    public void Spin()
    {
        if (_isSpinning) return;
        StartCoroutine(SpinRoutine());
    }
    private IEnumerator SpinRoutine()
    {
        _isSpinning = true;

        _resultIndex = randomResult
            ? UnityEngine.Random.Range(0, sliceCount)
            : Mathf.Clamp(manualResultIndex, 0, sliceCount - 1);

        float anglePerSlice = 360f / sliceCount;
        float targetAngle = (spinRounds * 360f) + (360 - (_resultIndex * anglePerSlice));
        int totalPassCount = (spinRounds * sliceCount) + _resultIndex;

        _spinSeq?.Kill(true);
        _spinSeq = DOTween.Sequence();

        _spinSeq.Append(
            wheelTransform.DORotate(new Vector3(0, 0, -targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(spinEaseCurve)
        );

        for (int i = 0; i < totalPassCount; i++)
        {
            float normalizedT = i / (float)totalPassCount;
            float easedT = spinEaseCurve.Evaluate(normalizedT);
            float triggerTime = easedT * spinDuration;

            _spinSeq.InsertCallback(triggerTime, () =>
            {
                SelectorTickEffect();
            });
        }

        _spinSeq.OnComplete(() =>
        {
            _isSpinning = false;
            ShowReward();
        });
        yield return new WaitForSeconds(spinDuration + 0.1f);
    }

    private void SelectorTickEffect()
    {
        selectorTransform.DOLocalRotate(new Vector3(0, 0, 10), 0.05f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad).OnComplete(() =>
            {
                selectorTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.05f);
            });

        //if (tickAudioSource != null && tickClip != null)
        //{
        //    tickAudioSource.PlayOneShot(tickClip);
        //}
    }

    public WheelSliceData GetSpinResult()
    {
        return FurtuneSpinWheelPanel.Instance.GetWheelSliceData()[_resultIndex];
    }

    public void ShowReward()
    {
        var result = GetSpinResult();

        if (result.SliceType == SliceType.Fail)
        {
            UIManager.Instance.ShowPanel(PanelEnum.FailPanel);
        }
        else
        {
            UIManager.Instance.ShowPanel(PanelEnum.RewardCardPanel);
        }
    }

    private void UnSubscribe()
    {
        EventBroker.UnSubscribe(Events.ON_READY_FOR_SPIN, OnActiveSpin);
        EventBroker.UnSubscribe(Events.SET_SPIN_BUTTON, OnActiveSpin);
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
