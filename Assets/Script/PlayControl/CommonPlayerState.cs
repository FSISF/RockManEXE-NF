using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonPlayerState
{
    public enum CommonPlayerNowState
    {
        Idle,
        Move,
    }

    public class CommonPlayerComponent : CharacterComponent
    {
    }

    public class CommonPlayerStateContext
    {
        private ICommonPlyaerState ICommonPlyaerStateScript = null;

        public CommonPlayerNowState NowState = CommonPlayerNowState.Idle;

        public void AwakePlayerState(CommonPlayerComponent commonplayercomponent)
        {
            ICommonPlyaerStateScript = new CommonPlayerIdle(this, commonplayercomponent);
        }

        public void SetState(ICommonPlyaerState icommonplayerstate)
        {
            ICommonPlyaerStateScript = icommonplayerstate;
        }

        public void StateWork()
        {
            ICommonPlyaerStateScript.StateWork();
        }
    }

    public abstract class ICommonPlyaerState
    {
        protected CommonPlayerComponent CommonPlayerComponentScript;

        protected CommonPlayerStateContext CommonPlayerStateContextScript;

        public abstract void StateWork();

        public delegate void StartWork();

        protected void StateSet(CommonPlayerStateContext commonplayerstatecontext, CommonPlayerComponent commonplayercomponent, CommonPlayerNowState nowstate, StartWork startwork = null)
        {
            CommonPlayerStateContextScript = commonplayerstatecontext;
            CommonPlayerComponentScript = commonplayercomponent;
            CommonPlayerStateContextScript.NowState = nowstate;

            //Debug.Log(PlayerStateContextScript.NowState.ToString());

            if (startwork != null)
            {
                startwork();
            }
        }
    }

    public class CommonPlayerIdle : ICommonPlyaerState
    {
        public CommonPlayerIdle(CommonPlayerStateContext commonplayerstatecontext, CommonPlayerComponent commonplayercomponent)
        {
            StateSet(commonplayerstatecontext, commonplayercomponent, CommonPlayerNowState.Idle);
            CommonPlayerComponentScript.ThisAnimator.speed = 0;
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                CommonPlayerStateContextScript.SetState(new CommonPlayerMove(CommonPlayerStateContextScript, CommonPlayerComponentScript));
                CommonPlayerComponentScript.ThisAnimator.speed = 1;
            }
        }
    }

    public class CommonPlayerMove : ICommonPlyaerState
    {
        public CommonPlayerMove(CommonPlayerStateContext commonplayerstatecontext, CommonPlayerComponent commonplayercomponent)
        {
            StateSet(commonplayerstatecontext, commonplayercomponent, CommonPlayerNowState.Idle);
        }

        public override void StateWork()
        {
            MoveKeyPlay();
            MoveKeyUp();
            if (!UpFirst && !DownFirst && !LeftFirst && !RightFirst)
            {
                CommonPlayerStateContextScript.SetState(new CommonPlayerIdle(CommonPlayerStateContextScript, CommonPlayerComponentScript));
            }
        }

        private bool UpFirst = false;
        private bool DownFirst = false;
        private bool LeftFirst = false;
        private bool RightFirst = false;
        private void MoveKeyPlay()
        {
            if (Input.GetKey(KeyCode.W) && !DownFirst && !LeftFirst && !RightFirst)
            {
                UpFirst = true;
                SetMove("CommonPlayerMoveUp", Vector2.up);
                MoveLeftOrRight();
            }
            else if (Input.GetKey(KeyCode.S) && !UpFirst && !LeftFirst && !RightFirst)
            {
                DownFirst = true;
                SetMove("CommonPlayerMoveDown", Vector2.down);
                MoveLeftOrRight();
            }
            else if (Input.GetKey(KeyCode.A) && !DownFirst && !UpFirst && !RightFirst)
            {
                LeftFirst = true;
                SetMove("CommonPlayerMoveLeft", Vector2.left);
                MoveUpOrDown();
            }
            else if (Input.GetKey(KeyCode.D) && !DownFirst && !LeftFirst && !UpFirst)
            {
                RightFirst = true;
                SetMove("CommonPlayerMoveRight", Vector2.right);
                MoveUpOrDown();
            }
        }

        private void MoveKeyUp()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                UpFirst = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                DownFirst = false;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                LeftFirst = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                RightFirst = false;
            }
        }

        private void SetMove(string AnimatorPlay,Vector2 direct)
        {
            CommonPlayerComponentScript.ThisAnimator.Play(AnimatorPlay);
            CommonPlayerComponentScript.MoveRigidbody2D(direct);
            CommonPlayerComponentScript.Direct = direct;
        }

        private void MoveLeftOrRight()
        {
            if (Input.GetKey(KeyCode.A))
            {
                CommonPlayerComponentScript.MoveRigidbody2D(Vector2.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                CommonPlayerComponentScript.MoveRigidbody2D(Vector2.right);
            }
        }

        private void MoveUpOrDown()
        {
            if (Input.GetKey(KeyCode.W))
            {
                CommonPlayerComponentScript.MoveRigidbody2D(Vector2.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                CommonPlayerComponentScript.MoveRigidbody2D(Vector2.down);
            }
        }
    }
}