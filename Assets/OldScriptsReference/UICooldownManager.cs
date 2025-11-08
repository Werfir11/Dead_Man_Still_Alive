using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownManager : MonoBehaviour
{
    public MovementR move;
    public Text dashCd;
    public Text wallJumpCd;
    public Text Health;
    public HealthSystem healthSystem;

    public float dashCDCalc;
    public float wallJumpCDCalc;

    void Update()
    {
        dashCDCalc = move.lastDashTime + move.dashCooldown - Time.time;
        wallJumpCDCalc = move.lastWallJumpTime + move.wallJumpColldown - Time.time;
        if (dashCDCalc > 0)
        {
            dashCd.text = Math.Round(dashCDCalc, 1).ToString();
        }
        else
        {
            dashCd.text = "0";
        }
        if (wallJumpCDCalc > 0)
        {
            wallJumpCd.text = Math.Round(wallJumpCDCalc, 1).ToString();
        }
        else
        {
            wallJumpCd.text = "0";
        }
        Health.text = healthSystem.currentHealth.ToString();
    }
}
