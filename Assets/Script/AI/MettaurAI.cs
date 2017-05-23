using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using System;

public class MettaurAI : EnemyState
{
    [SerializeField]
    private HPManager HPManageScript = null;

    void Start()
    {

    }

    void Update()
    {

    }

    public override void EnemyInjurd(int Damage)
    {
        HPManageScript.DecreaseHP(Damage);
    }
}

namespace Enemy
{
    public enum MettaurAINowState
    {
        Idle,
        Move,
        Attack,
        Injurd,
    }

    public class MettaurComponent : BattleCharacterComponent
    {
    }

    public class MettaurAIStateContext
    {
        private MettaurAIState MettaurAIStateScript = null;

        public MettaurAINowState NowState = MettaurAINowState.Idle;

        public void AwakeMettaurAIState(MettaurComponent mettaurcomponent)
        {
            MettaurAIStateScript = new MettaurIdle(this, mettaurcomponent);
        }

        public void SetState(MettaurAIState mettauraistate)
        {
            MettaurAIStateScript = mettauraistate;
        }

        public void StateWork()
        {
            MettaurAIStateScript.StateWork();
        }
    }

    public abstract class MettaurAIState
    {
        protected MettaurComponent MettaurAIComponent;

        protected MettaurAIStateContext MettaurAIStateContextScript;

        public abstract void StateWork();

        public delegate void StartWork();

        protected void StateSet(MettaurAIStateContext mettauraistatecontext, MettaurComponent mettaurcomponent, MettaurAINowState nowstate, StartWork startwork = null)
        {
            MettaurAIStateContextScript = mettauraistatecontext;
            MettaurAIComponent = mettaurcomponent;
            MettaurAIStateContextScript.NowState = nowstate;

            Debug.Log(MettaurAIStateContextScript.NowState.ToString());

            if (startwork != null)
            {
                startwork();
            }
        }
    }

    public class MettaurIdle : MettaurAIState
    {
        public MettaurIdle(MettaurAIStateContext mettauraistatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettauraistatecontext, mettaurcomponent, MettaurAINowState.Idle, () =>
            {

            });
        }

        public override void StateWork()
        {
        }
    }

    public class MettaurMove : MettaurAIState
    {
        public MettaurMove(MettaurAIStateContext mettauraistatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettauraistatecontext, mettaurcomponent, MettaurAINowState.Move, () =>
            {

            });
        }

        public override void StateWork()
        {
        }
    }

    public class MettaurAttack : MettaurAIState
    {
        public MettaurAttack(MettaurAIStateContext mettauraistatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettauraistatecontext, mettaurcomponent, MettaurAINowState.Attack, () =>
            {

            });
        }

        public override void StateWork()
        {
        }
    }

    public class MettaurInjrud : MettaurAIState
    {
        public MettaurInjrud(MettaurAIStateContext mettauraistatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettauraistatecontext, mettaurcomponent, MettaurAINowState.Injurd, () =>
            {

            });
        }

        public override void StateWork()
        {
        }
    }
}