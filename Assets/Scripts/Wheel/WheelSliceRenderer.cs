using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WheelSliceRenderer : MonoBehaviour
{
    [SerializeField] private List<GameObject> ref_slice_obj;

    private void Awake()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        EventBroker.Subscribe(Events.ON_READY_FOR_SPIN, DrawSlices);
    }

    private void DrawSlices()
    {
        var sliceDatas = FurtuneSpinWheelPanel.Instance.GetWheelSliceData();

        for (int i = 0; i < sliceDatas.Count; i++)
        {
            ref_slice_obj[i].transform.GetChild(0).GetComponent<Image>().sprite = sliceDatas[i].GetSprite();
            ref_slice_obj[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(sliceDatas[i].RewardText);
        }
    }

    private void UnSubscribe()
    {
        EventBroker.UnSubscribe(Events.ON_READY_FOR_SPIN, DrawSlices);
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
