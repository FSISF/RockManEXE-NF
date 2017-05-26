using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MettaurState
{
    public enum MettaurNowState
    {
        Idle,
        Follow,
        Attack,
        Injurd,
        Dead,
    }

    public class MettaurComponent : BattleCharacterComponent
    {
    }

    public class MettaurStateContext
    {
        private IMettaurState IMettaurStateScript = null;

        public MettaurNowState NowState;

        public void AwakeStateSet(MettaurComponent mettaurcomponent)
        {
            IMettaurStateScript = new MettaurIdle(this, mettaurcomponent);
        }

        public void SetState(IMettaurState imettaurstate)
        {
            IMettaurStateScript = imettaurstate;
        }

        public void StateWork()
        {
            IMettaurStateScript.StateWork();
        }
    }

    public abstract class IMettaurState
    {
        protected MettaurStateContext MettaurStateContextScript;

        protected MettaurComponent MettaurComponentScript;

        public abstract void StateWork();

        public delegate void StartWork();

        protected void StateSet(MettaurStateContext mettaurstatecontext, MettaurComponent mettaurcomponent, MettaurNowState nowstate, StartWork startwork = null)
        {
            MettaurStateContextScript = mettaurstatecontext;
            MettaurComponentScript = mettaurcomponent;
            MettaurStateContextScript.NowState = nowstate;

            Debug.Log(MettaurStateContextScript.NowState.ToString());

            if (startwork != null)
            {
                startwork();
            }
        }
    }

    public class MettaurIdle : IMettaurState
    {
        public MettaurIdle(MettaurStateContext mettaurstatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettaurstatecontext, mettaurcomponent, MettaurNowState.Idle, () => 
            {
                MettaurComponentScript.ThisAnimator.Play("MettaurIdle");
            });
        }

        public override void StateWork()
        {
        }
    }
}
