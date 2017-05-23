using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllPlayerState
{
    public enum PlayerNowState
    {
        Idle,
        Move,
        Jump,
        Attack,
        MoveAttack,
        JumpAttack,
        Injurd,
    }

    public class StateComponent:BattleCharacterComponent
    {
        public float JumpForce;
        public bool Grounded;

        public void DoJump()
        {
            ThisRigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

        private GameObject CloneBullet = null;
        private Bullet BulletScript = null;
        public void DoCloneBullet()
        {
            Vector3 PlayerPosition = PlayerController.Instance.transform.position;
            Vector3 ClonePosition = new Vector3(PlayerPosition.x + (Direct.x * 1.75f), PlayerPosition.y);
            Quaternion CloneRotate = Quaternion.Euler(Vector3.zero);

            CloneBullet = GameObject.Instantiate(Resources.Load("CloneObject/Bullet", typeof(GameObject)), ClonePosition, CloneRotate) as GameObject;

            BulletScript = CloneBullet.GetComponent<Bullet>();
            BulletScript.Direct = Direct;
        }
    }

    public class PlayerStateContext
    {
        private PlayerState PlayerStateScript = null;

        public PlayerNowState NowState = PlayerNowState.Idle;

        public void AwakePlayerState(StateComponent statechangecomponent)
        {
            PlayerStateScript = new PlayerIdle(this, statechangecomponent);
        }

        public void SetPlayerState(PlayerState playerstate)
        {
            PlayerStateScript = playerstate;
        }

        public void StateWork()
        {
            PlayerStateScript.StateWork();
        }
    }

    public abstract class PlayerState
    {
        protected StateComponent PlayerComponent;

        protected PlayerStateContext PlayerStateContextScript;

        public abstract void StateWork();

        public delegate void Startwork();

        protected void StateSet(PlayerStateContext playerstatecontext, StateComponent statecomponent, PlayerNowState nowstate, Startwork startwork = null)
        {
            PlayerStateContextScript = playerstatecontext;
            PlayerComponent = statecomponent;
            PlayerStateContextScript.NowState = nowstate;

            Debug.Log(PlayerStateContextScript.NowState.ToString());

            if (startwork != null)
            {
                startwork();
            }
        }
    }

    public class PlayerIdle : PlayerState
    {
        public PlayerIdle(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.Idle, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerIdle");
            });
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))//Start Move
            {
                PlayerStateContextScript.SetPlayerState(new PlayerMove(PlayerStateContextScript, PlayerComponent));
            }
            if (Input.GetKeyDown(KeyCode.Space))//Start Jump
            {
                PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
            }
            if (Input.GetKeyDown(KeyCode.J))//Start Attack
            {
                PlayerStateContextScript.SetPlayerState(new PlayerAttack(PlayerStateContextScript, PlayerComponent));
            }
        }
    }

    public class PlayerMove : PlayerState
    {
        public PlayerMove(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.Move, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerMove");
            });
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.A))//Move Left
            {
                PlayerComponent.MoveRigidbody2D(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.D))//Move Right
            {
                PlayerComponent.MoveRigidbody2D(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.Space))//Start Jump
            {
                PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))//Stop Move
            {
                PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
            }

            if (Input.GetKeyDown(KeyCode.J))//Start Attack
            {
                PlayerStateContextScript.SetPlayerState(new PlayerMoveAttack(PlayerStateContextScript, PlayerComponent));
            }
        }
    }

    public class PlayerJump : PlayerState
    {
        public PlayerJump(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.Jump, () =>
            {
                if (PlayerComponent.Grounded)
                {
                    PlayerComponent.DoJump();
                }
                PlayerComponent.ThisAnimator.Play("PlayerJump");
            });
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.A))//Move Left
            {
                PlayerComponent.MoveRigidbody2D(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.D))//Move Right
            {
                PlayerComponent.MoveRigidbody2D(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.J))//Start Attack
            {
                PlayerStateContextScript.SetPlayerState(new PlayerJumpAttack(PlayerStateContextScript, PlayerComponent));
            }
        }
    }

    public class PlayerAttack : PlayerState
    {
        private float AttackTimeMax = 0.1f;
        private float ReturnIdleTimeMax = 0.25f;
        private float AttackTimer = 0;

        public PlayerAttack(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.Attack, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerAttack");
                PlayerComponent.DoCloneBullet();
            });
        }

        public override void StateWork()
        {
            if (Input.GetKeyDown(KeyCode.Space))//Start Jump
            {
                PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
            }

            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackTimeMax)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerAttack(PlayerStateContextScript, PlayerComponent));
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerMove(PlayerStateContextScript, PlayerComponent));
                }

                if (AttackTimer >= AttackTimeMax + ReturnIdleTimeMax)
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
                }
            }
        }
    }

    public class PlayerMoveAttack : PlayerState
    {
        private float AttackTimeMax = 0.01f;
        private float ReturnIdleTimeMax = 0.25f;
        private float AttackTimer = 0;

        public PlayerMoveAttack(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.MoveAttack, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerMoveAttack");
                PlayerComponent.DoCloneBullet();
            });
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.A))//Move Left
            {
                PlayerComponent.MoveRigidbody2D(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.D))//Move Right
            {
                PlayerComponent.MoveRigidbody2D(Vector2.right);
            }

            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackTimeMax)
            {
                if (Input.GetKeyDown(KeyCode.J) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerAttack(PlayerStateContextScript, PlayerComponent));
                }

                if (Input.GetKeyDown(KeyCode.J) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerMoveAttack(PlayerStateContextScript, PlayerComponent));
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
                }

                if (AttackTimer >= AttackTimeMax + ReturnIdleTimeMax)
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        PlayerStateContextScript.SetPlayerState(new PlayerMove(PlayerStateContextScript, PlayerComponent));
                    }
                    else
                    {
                        PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
                    }
                }
            }
        }
    }

    public class PlayerJumpAttack : PlayerState
    {
        private float AttackTimeMax = 0.01f;
        private float ReturnIdleTimeMax = 0.25f;
        private float AttackTimer = 0;

        public PlayerJumpAttack(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.JumpAttack, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerJumpAttack");
                PlayerComponent.DoCloneBullet();
            });
        }

        public override void StateWork()
        {
            if (Input.GetKey(KeyCode.A))//Move Left
            {
                PlayerComponent.MoveRigidbody2D(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.D))//Move Right
            {
                PlayerComponent.MoveRigidbody2D(Vector2.right);
            }

            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackTimeMax)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerJumpAttack(PlayerStateContextScript, PlayerComponent));
                }

                if (AttackTimer >= AttackTimeMax + ReturnIdleTimeMax)
                {
                    if (PlayerComponent.ThisRigidbody2D.velocity.y != 0)
                    {
                        PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
                    }
                    else
                    {
                        PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
                    }
                }
            }
        }
    }

    public class PlayerInjurd : PlayerState
    {
        private float TurnBackMaxTime = 0.5f;
        private float TurnBackTimer = 0;
        private float TurnBackSpeed = 1f;
        public PlayerInjurd(PlayerStateContext playerstatecontext, StateComponent statechangecomponent)
        {
            StateSet(playerstatecontext, statechangecomponent, PlayerNowState.Injurd, () =>
            {
                PlayerComponent.ThisAnimator.Play("PlayerInjurd");
                if (PlayerComponent.ThisRigidbody2D.velocity.y > 0)
                {
                    PlayerComponent.ThisRigidbody2D.velocity = Vector2.zero;
                }
            });
        }

        public override void StateWork()
        {
            TurnBackTimer += Time.deltaTime;
            PlayerComponent.ThisRigidbody2D.position -= PlayerComponent.Direct * TurnBackSpeed * Time.deltaTime;
            if (TurnBackTimer >= TurnBackMaxTime)
            {
                if (!PlayerComponent.Grounded)
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerJump(PlayerStateContextScript, PlayerComponent));
                }
                else
                {
                    PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
                }
            }
        }
    }
}