using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MattaurState;

public class Mettaur : BattleCharacter
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public override void CharacterInjurd(int Damage)
    {
        HPManagerScript.DecreaseHP(Damage);
    }
}