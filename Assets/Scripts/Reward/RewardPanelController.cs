using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelController : MonoBehaviour
{
    [SerializeField] private Transform ui_transform_content;
    [SerializeField] private Button ui_button_exit;

    private List<WheelSliceData> _rewardList_data = new();
    private List<GameObject> _rewardsList_refs = new();

    private void Awake()
    {
        Subscribe();
        SetupButton();
    }

    private void OnEnable()
    {
        OnActiveExitButton();
    }

    private void OnActiveExitButton()
    {
        ui_button_exit.gameObject.SetActive(true);
        ui_button_exit.interactable = true;
    }

    private void OnClickExitButton()
    {
        UIManager.Instance.ShowPanel(PanelEnum.ExitPanel);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ui_button_exit == null)
            ui_button_exit = GameObject.Find("ui_button_exit")?.GetComponent<Button>();
    
        if (ui_button_exit != null)
        {
            ui_button_exit.onClick.RemoveAllListeners();
            ui_button_exit.onClick.AddListener(OnClickExitButton);
        }
    }
#endif

    private void SetupButton()
    {
        if (ui_button_exit != null)
        {
            ui_button_exit.onClick.RemoveAllListeners();
            ui_button_exit.onClick.AddListener(OnClickExitButton);
        }
    }

    private void Subscribe()
    {
        EventBroker.Subscribe(Events.ADD_REWARD_IN_REWARD_PANEL, AddRewardInPanel);
        EventBroker.Subscribe(Events.ON_TAKE_REWARD, OnTakeReward);
        EventBroker.Subscribe<bool>(Events.SET_EXIT_BUTTON, SetExitButton);
    }

    private void SetExitButton(bool isInteractive)
    {
        ui_button_exit.interactable = isInteractive;
    }

    private void AddRewardInPanel()
    {
        var sliceData = SpinManager.Instance.GetSpinResult();
        for (int i = 0; i < _rewardList_data.Count; i++)
        {
            if (_rewardList_data[i].SliceType == sliceData.SliceType)
            {
                _rewardList_data[i].RewardValue += sliceData.RewardValue;
                _rewardsList_refs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(_rewardList_data[i].RewardValue.ToString());
                return;
            }
        }

        var newSliceData = new WheelSliceData
        {
            SliceType = sliceData.SliceType,
            SpriteName = sliceData.SpriteName,
            RewardValue = sliceData.RewardValue
        };
        _rewardList_data.Add(newSliceData);
        var reward = PoolManager.Instance.GetObject(PoolType.RewardPanelCard);
        _rewardsList_refs.Add(reward);
        reward.transform.SetParent(ui_transform_content);
        reward.transform.localScale = Vector3.one;
        reward.transform.GetChild(0).GetComponent<Image>().sprite = sliceData.GetSprite();
        reward.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(sliceData.RewardValue.ToString());
    }

    private void OnTakeReward()
    {
        //foreach (var rewardData in _rewardList_data)
        //{
        //    Debug.Log($"Player received {rewardData.RewardValue} of {rewardData.SliceType}");
        //    //Here you can add the logic to give the reward to the player based on rewardData.SliceType and rewardData.RewardValue
        //}

        foreach (var reward in _rewardsList_refs)
        {
            PoolManager.Instance.ReturnObject(reward);
        }
        _rewardList_data.Clear();
        _rewardsList_refs.Clear();
    }

    private void UnSubscribe()
    {
        EventBroker.UnSubscribe(Events.ADD_REWARD_IN_REWARD_PANEL, AddRewardInPanel);
        EventBroker.UnSubscribe(Events.ON_TAKE_REWARD, OnTakeReward);
        EventBroker.UnSubscribe<bool>(Events.SET_EXIT_BUTTON, SetExitButton);
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
