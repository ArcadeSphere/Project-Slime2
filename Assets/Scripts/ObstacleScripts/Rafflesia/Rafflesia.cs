using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafflesia : PlayerDetection
{
    private enum RafflesiaState {idle, attack}

   
    override public void OnPlayerIn() {
        ChangeState(RafflesiaState.attack);
    }

    override public void OnPlayerOut() {
        ChangeState(RafflesiaState.idle);

    }

    void ChangeState(RafflesiaState state) {
        detectorOriginAnim.SetInteger("state", (int)state);
    }

}
