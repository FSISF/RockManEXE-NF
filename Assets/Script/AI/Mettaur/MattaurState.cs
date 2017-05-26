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
        SwitchLine,
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

        private float MettaurPosX;
        private float TargetPosX;
        public override void StateWork()
        {
            MettaurPosX = MettaurComponentScript.ThisTransform.position.x;
            TargetPosX = MettaurComponentScript.FollowTarget.transform.position.x;
            if (Mathf.Abs(MettaurPosX - TargetPosX) <= 20)
            {
                MettaurStateContextScript.SetState(new MettaurFollow(MettaurStateContextScript, MettaurComponentScript));
            }
        }
    }

    public class MettaurFollow : IMettaurState
    {
        public MettaurFollow(MettaurStateContext mettaurstatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettaurstatecontext, mettaurcomponent, MettaurNowState.Follow, () =>
            {
                MettaurComponentScript.ThisAnimator.Play("MettaurMove");
            });
        }

        private float MettaurPosX;
        private float TargetPosX;
        private float WaittingNextMax = 0.25f;
        private float WaittingNextTimer = 0;
        public override void StateWork()
        {
            MettaurPosX = MettaurComponentScript.ThisTransform.position.x;
            TargetPosX = MettaurComponentScript.FollowTarget.transform.position.x;
            if (Mathf.Abs(MettaurPosX - TargetPosX) > 20)
            {
                MettaurStateContextScript.SetState(new MettaurIdle(MettaurStateContextScript, MettaurComponentScript));
            }
            else
            {
                if (Mathf.Abs(MettaurPosX - TargetPosX) <= 10 && MettaurComponentScript.FollowTarget.layer != MettaurComponentScript.ThisGameObject.layer)
                {
                    WaittingNextTimer += Time.deltaTime;
                    if (WaittingNextTimer >= WaittingNextMax)
                    {
                        MettaurStateContextScript.SetState(new MettaurSwitchLine(MettaurStateContextScript, MettaurComponentScript));
                    }
                    
                }
                if (Mathf.Abs(MettaurPosX - TargetPosX) <= 5 && MettaurComponentScript.FollowTarget.layer == MettaurComponentScript.ThisGameObject.layer)
                {
                    MettaurStateContextScript.SetState(new MettaurAttack(MettaurStateContextScript, MettaurComponentScript));
                }

                if (MettaurPosX > TargetPosX)
                {
                    MettaurComponentScript.MoveRigidbody2D(Vector2.left);
                }
                else if (MettaurPosX < TargetPosX)
                {
                    MettaurComponentScript.MoveRigidbody2D(Vector2.right);
                }
            }
        }
    }

    public class MettaurSwitchLine : IMettaurState
    {
        public MettaurSwitchLine(MettaurStateContext mettaurstatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettaurstatecontext, mettaurcomponent, MettaurNowState.SwitchLine, () =>
            {
                MettaurComponentScript.ThisAnimator.Play("MettaurJump");
                if (MettaurComponentScript.FollowTarget.layer > MettaurComponentScript.ThisGameObject.layer)
                {
                    MettaurComponentScript.SwitchLine(-1);
                }
                else
                {
                    MettaurComponentScript.SwitchLine(1);
                }
            });
        }

        public override void StateWork()
        {
            if (MettaurComponentScript.Grounded && MettaurComponentScript.ThisRigidbody2D.velocity.y == 0)
            {
                MettaurStateContextScript.SetState(new MettaurIdle(MettaurStateContextScript, MettaurComponentScript));
            }
        }
    }

    public class MettaurAttack : IMettaurState
    {
        public MettaurAttack(MettaurStateContext mettaurstatecontext, MettaurComponent mettaurcomponent)
        {
            StateSet(mettaurstatecontext, mettaurcomponent, MettaurNowState.Attack, () =>
            {
                MettaurComponentScript.ThisAnimator.Play("MettaurIdle");
                MettaurComponentScript.DoCloneBullet(1.5f, 1, "Player");
            });
        }

        private float MettaurPosX;
        private float TargetPosX;
        private float NextAttackMax = 1f;
        private float NextAttackTimer = 0;
        public override void StateWork()
        {
            MettaurPosX = MettaurComponentScript.ThisTransform.position.x;
            TargetPosX = MettaurComponentScript.FollowTarget.transform.position.x;
            NextAttackTimer += Time.deltaTime;
            if (NextAttackTimer >= NextAttackMax)
            {
                if (Mathf.Abs(MettaurPosX - TargetPosX) <= 5 && MettaurComponentScript.FollowTarget.layer == MettaurComponentScript.ThisGameObject.layer)
                {
                    MettaurStateContextScript.SetState(new MettaurAttack(MettaurStateContextScript, MettaurComponentScript));
                }
                else
                {
                    MettaurStateContextScript.SetState(new MettaurIdle(MettaurStateContextScript, MettaurComponentScript));
                }
            }
        }
    }
}
