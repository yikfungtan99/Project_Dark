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
    [SerializeField] private GameObject torchObject;
    [SerializeField] private float torchRange;

    private Light2D torchLight;
    private PlayerRevealer playerTorchRevealer;

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

    [SerializeField] private PlatformerMovement move;

    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<PlayerStats>();
        battery = maxTorchBattery;

        torchLight = torchObject.GetComponent<Light2D>();
        playerTorchRevealer = torchObject.GetComponent<PlayerRevealer>();
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
        torchLight.enabled = true;
        torchOn = true;
    }

    public void TorchOff()
    {
        selfLight.SetActive(false);
        torchLight.enabled = false;
        torchOn = false;
    }

    private void TorchRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, torchRange, torchBlockLayer);

        if (hit)
        {
            torchLight.pointLightOuterRadius = hit.distance + torchRangeOffset;
            playerTorchRevealer.torchRevealRange = hit.distance;
        }
        else
        {
            torchLight.pointLightOuterRadius = torchRange;
            playerTorchRevealer.torchRevealRange = torchRange;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * torchRange);
    }

  
    private void CheckForBattery()
    {
        if (battery <= 0)
        {
            if (torchOn)
            {
                CmdTorchOff();
            }
        }
    }

    public void Torch(CallbackContext ctx)
    {
        if (!hasAuthority) return;

        if (battery <= 0) return;

        if (!torchOn)
        {
            CmdTorchOn();
        }
        else
        {
            CmdTorchOff();
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdTorchOn()
    {
        RpcTorchOn();
    }

    [ClientRpc]
    public void RpcTorchOn()
    {
        TorchOn();
    }
    
    [Command(ignoreAuthority = true)]
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
