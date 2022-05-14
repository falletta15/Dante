﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    :base(currentContext, playerStateFactory){
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState(){
        StartDash();
    }
    public override void UpdateState(){
        CheckSwitchStates();
        PerformDash();
    }
    public override void ExitState(){}
    public override void InitializeSubState(){}
    public override void CheckSwitchStates(){

    }

    void StartDash()
    {
        //Debug.Log("Player enter Dash state");
        Ctx.DashSpeed = 100f;
    }

    void PerformDash()
    {
        Vector2 dashDir = Ctx.MovementInputValue.normalized;
        Ctx.Rb.velocity = dashDir * Ctx.DashSpeed * Time.deltaTime;
        DashSpeedSlowdown();
    }

    private void DashSpeedSlowdown()
    {
        Ctx.DashSpeed -= Ctx.DashSpeed * 5f * Time.deltaTime;

        if(Ctx.DashSpeed < 20f)
        {
            SwitchState(Factory.Move());
        }
    }
}
