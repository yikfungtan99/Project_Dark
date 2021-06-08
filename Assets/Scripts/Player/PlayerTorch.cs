using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using static UnityEngine.InputSystem.InputAction;

public class PlayerTorch : NetworkBehaviour
{
    [Header ("Torch")]
    [SerializeField] private GameObject selfLight;
    [SerializeField] private Light2D torch;
    [SerializeField] private float torchRange;

    [Range(0.1f, 3.0f)]
    [SerializeField] private float torchRangeOffset;
    [SerializeField] private LayerMask torchBlockLayer;
    [SerializeField] private LayerMask torchPlayerLayer;

    [Header("Battery")]
    [SyncVar] public float battery;
    public float maxTorchBattery;
    public float currentPercentage;
    public float drainRate;


    public bool torchOn;
    private float radius;

    public bool outOfBattery = false;

    [SerializeField] private PlatformerMovement move;

    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<PlayerStats>();
        battery = maxTorchBattery;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;
        CmdBatteryUpdate();
    }

    [Command]
    private void CmdBatteryUpdate()
    {
        BatteryUpdate();
    }

    private void BatteryUpdate()
    {
        CheckForBattery();

        if (torchOn) battery -= drainRate * Time.deltaTime;

        if (battery <= 0)
        {
            battery = 0;
        }

        currentPercentage = (battery / maxTorchBattery) * 100;

        stats.battery = (int) currentPercentage;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (torchOn)
        {
            TorchRange();
        }
    }

    public void Torch()
    {
        if (torchOn)
        {

            TorchOff();

        }
        else
        {
            TorchOn();
        }
    }

    public void TorchOn()
    {
        selfLight.SetActive(true);
        torch.enabled = true;
        torchOn = true;
    }

    public void TorchOff()
    {
        selfLight.SetActive(false);
        torch.enabled = false;
        torchOn = false;
    }

    private void TorchRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, torchRange, torchBlockLayer);
        //RaycastHit2D[] hitPlayer = Physics2D.RaycastAll(transform.position, transform.up, torchRange, torchPlayerLayer);

        if (hit)
        {
            torch.pointLightOuterRadius = hit.distance + torchRangeOffset;
        }
        else
        {
            torch.pointLightOuterRadius = torchRange;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * torchRange);
    }

  
    private void CheckForBattery()
    {
        if (battery <= 0)
        {
            outOfBattery = true;
            if (torchOn)
            {
                CmdTorchOff();
            }
        }
        else
        {
            outOfBattery = false;
        }
    }

    public void Torch(CallbackContext ctx)
    {
        if (!hasAuthority) return;
        if (!outOfBattery)
        {
            if (!torchOn)
            {
                CmdTorchOn();
            }
            else
            {
                CmdTorchOff();
            }
        }
    }

    [Command]
    public void CmdTorchOn()
    {
        RpcTorchOn();
    }

    [ClientRpc]
    public void RpcTorchOn()
    {
        TorchOn();
    }
    
    [Command]
    public void CmdTorchOff()
    {
        RpcTorchOff();
    }

    [ClientRpc]
    public void RpcTorchOff()
    {
        TorchOff();
    }
}
