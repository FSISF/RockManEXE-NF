using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCState
{
    public enum MoveStyle
    {
        None,
        SwitchDirect,
        Once,
        PingPong,
        Loop,
    }

    public enum NPCNowState
    {
        Idle,
        Move,
        SwitchDirect,
        Waitting,
        Talking,
    }

    public class NPCComponent : CharacterComponent
    {
        public string NPCName;
        public MoveStyle ThisMoveStyle;
        public List<Vector2> MoveLine = new List<Vector2>();
        public int NowPoint = 0;
    }

    public class NPCStateContext
    {
        private INPCState INPCStateScript = null;

        public NPCNowState NowState;

        public void AwakeSet(NPCComponent npccomponent)
        {
            INPCStateScript = new NPCIdle(this, npccomponent);
        }

        public void SetState(INPCState inpcstate)
        {
            INPCStateScript = inpcstate;
        }

        public void StateWork()
        {
            INPCStateScript.StateWork();
        }
    }

    public abstract class INPCState
    {
        protected NPCStateContext NPCStateContextScript;

        protected NPCComponent NPCComponentScript;

        public abstract void StateWork();

        public delegate void StartWork();

        protected void StateSet(NPCStateContext npcstatecontext, NPCComponent npccomponent, NPCNowState nowstate, StartWork startwork = null)
        {
            NPCStateContextScript = npcstatecontext;
            NPCComponentScript = npccomponent;
            NPCStateContextScript.NowState = nowstate;

            Debug.Log(NPCStateContextScript.NowState.ToString());

            if (startwork != null)
            {
                startwork();
            }
        }

        protected void AnimatorDirect(Vector2 direct, string action)
        {
            float X = Mathf.Abs(direct.x);
            float Y = Mathf.Abs(direct.y);
            if (X > Y)
            {
                if (direct.x > 0)
                {
                    NPCComponentScript.ThisAnimator.Play(NPCComponentScript.NPCName + action + "Right");
                }
                else
                {
                    NPCComponentScript.ThisAnimator.Play(NPCComponentScript.NPCName + action + "Left");
                }
            }
            else if (X < Y)
            {
                if (direct.y > 0)
                {
                    NPCComponentScript.ThisAnimator.Play(NPCComponentScript.NPCName + action + "Up");
                }
                else
                {
                    NPCComponentScript.ThisAnimator.Play(NPCComponentScript.NPCName + action + "Down");
                }
            }
        }

        protected bool IsOnNextPoint(Vector2 nowposition, Vector2 nextpoint)
        {
            if (Vector2.Distance(nowposition, nextpoint) < 0.05f)
            {
                return true;
            }
            return false;
        }
    }

    public class NPCIdle : INPCState
    {
        public NPCIdle(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Idle, () =>
            {
            });
        }

        public override void StateWork()
        {
            switch (NPCComponentScript.ThisMoveStyle)
            {
                case MoveStyle.SwitchDirect:
                    NPCStateContextScript.SetState(new NPCSwitchDirect(NPCStateContextScript, NPCComponentScript));
                    break;
                case MoveStyle.Once:
                    NPCStateContextScript.SetState(new NPCMoveOnce(NPCStateContextScript, NPCComponentScript));
                    break;
                case MoveStyle.PingPong:
                    NPCStateContextScript.SetState(new NPCMovePingPong(NPCStateContextScript, NPCComponentScript));
                    break;
                case MoveStyle.Loop:
                    NPCStateContextScript.SetState(new NPCMoveLoop(NPCStateContextScript, NPCComponentScript));
                    break;
            }
        }
    }

    /// <summary>
    /// NPC切換面相
    /// </summary>
    public class NPCSwitchDirect : INPCState
    {
        public NPCSwitchDirect(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.SwitchDirect);
        }

        private float SwitchTimeMax = 1;
        private float SwitchTimer = 0;
        private int RandomNum = 0;
        private int RandomDirect = 0;
        public override void StateWork()
        {
            SwitchTimer += Time.deltaTime;
            if (SwitchTimer >= SwitchTimeMax)
            {
                RandomNum= UnityEngine.Random.Range(0, 4);
                if (RandomNum == RandomDirect)
                {
                    return;
                }
                RandomDirect = RandomNum;
                switch (RandomDirect)
                {
                    case 0:
                        AnimatorDirect(Vector2.up, "Move");
                        break;
                    case 1:
                        AnimatorDirect(Vector2.down, "Move");
                        break;
                    case 2:
                        AnimatorDirect(Vector2.left, "Move");
                        break;
                    case 3:
                        AnimatorDirect(Vector2.right, "Move");
                        break;
                }
                SwitchTimer = 0;
            }
        }
    }

    /// <summary>
    /// NPC循環路徑移動
    /// </summary>
    public class NPCMoveLoop : INPCState
    {
        public NPCMoveLoop(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Move);
        }

        public override void StateWork()
        {
            DoMove();
        }

        private Vector2 NowPosition;
        private Vector2 MoveDirect;
        private void DoMove()
        {
            NowPosition = NPCComponentScript.ThisTransform.position;
            if (NPCComponentScript.NowPoint < NPCComponentScript.MoveLine.Count - 1)
            {
                MoveDirect = NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1] - NowPosition;
                if (IsOnNextPoint(NowPosition, NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1]))
                {
                    NPCComponentScript.NowPoint++;
                }
            }
            else if (NPCComponentScript.NowPoint == NPCComponentScript.MoveLine.Count - 1)
            {
                MoveDirect = NPCComponentScript.MoveLine[0] - NowPosition;
                if (IsOnNextPoint(NowPosition, NPCComponentScript.MoveLine[0]))
                {
                    NPCComponentScript.NowPoint = 0;
                }
            }
            NPCComponentScript.MoveTransform(MoveDirect.normalized);
            AnimatorDirect(MoveDirect.normalized, "Move");
        }
    }

    /// <summary>
    /// NPC單次路徑移動
    /// </summary>
    public class NPCMoveOnce : INPCState
    {
        public NPCMoveOnce(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Move);
        }

        public override void StateWork()
        {
            DoMove();
        }

        private Vector2 NowPosition;
        private Vector2 MoveDirect;
        private void DoMove()
        {
            NowPosition = NPCComponentScript.ThisTransform.position;
            if (NPCComponentScript.NowPoint < NPCComponentScript.MoveLine.Count - 1)
            {
                MoveDirect = NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1] - NowPosition;
                if (IsOnNextPoint(NowPosition, NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1]))
                {
                    NPCComponentScript.NowPoint++;
                }
                NPCComponentScript.MoveTransform(MoveDirect.normalized);
                AnimatorDirect(MoveDirect.normalized, "Move");
            }
        }
    }

    /// <summary>
    /// NPC來回路徑移動
    /// </summary>
    public class NPCMovePingPong : INPCState
    {
        public NPCMovePingPong(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Move);
        }

        public override void StateWork()
        {
            DoMove();
        }

        private Vector2 NowPosition;
        private Vector2 MoveDirect;
        private bool IsOnReverse = false;
        private void DoMove()
        {
            NowPosition = NPCComponentScript.ThisTransform.position;
            if (!IsOnReverse)
            {
                if (NPCComponentScript.NowPoint < NPCComponentScript.MoveLine.Count - 1)
                {
                    MoveDirect = NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1] - NowPosition;
                    if (IsOnNextPoint(NowPosition, NPCComponentScript.MoveLine[NPCComponentScript.NowPoint + 1]))
                    {
                        NPCComponentScript.NowPoint++;
                    }
                }
                else
                {
                    IsOnReverse = true;
                }
            }
            else
            {
                if (NPCComponentScript.NowPoint > 0)
                {
                    MoveDirect = NPCComponentScript.MoveLine[NPCComponentScript.NowPoint - 1] - NowPosition;
                    if (IsOnNextPoint(NowPosition, NPCComponentScript.MoveLine[NPCComponentScript.NowPoint - 1]))
                    {
                        NPCComponentScript.NowPoint--;
                    }
                }
                else
                {
                    IsOnReverse = false;
                }
            }
            NPCComponentScript.MoveTransform(MoveDirect.normalized);
            AnimatorDirect(MoveDirect.normalized, "Move");
        }
    }

    /// <summary>
    /// NPC碰到玩家後，等待玩家離開
    /// </summary>
    public class NPCWaitting : INPCState
    {
        public NPCWaitting(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Waitting);
        }

        public override void StateWork()
        {
        }
    }

    /// <summary>
    /// NPC對話中
    /// </summary>
    public class NPCTalking : INPCState
    {
        public NPCTalking(NPCStateContext npcstatecontext, NPCComponent npccomponent)
        {
            StateSet(npcstatecontext, npccomponent, NPCNowState.Talking);
        }

        public override void StateWork()
        {
        }
    }
}