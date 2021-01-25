using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TorchBattery : MonoBehaviour
{

    [SerializeField] private CustomTorch torch;
    [SerializeField] private TextMeshProUGUI batteryText;

    public float torchBattery;
    public float maxTorchBattery;
    public float currentPercentage;
    public float drainRate;

    // Start is called before the first frame update
    void Start()
    {
        torchBattery = maxTorchBattery;
    }

    // Update is called once per frame
    void Update()
    {
        if (!torch) return;

        if (torch.torchOn) torchBattery -= drainRate * Time.deltaTime;

        if (torchBattery <= 0) torchBattery = 0;

        currentPercentage = (torchBattery / maxTorchBattery) * 100;

        if (!batteryText) return;
        batteryText.text = "Battery - " + (int)currentPercentage + " %";
    }
}
