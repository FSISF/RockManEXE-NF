using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterComponent
{
    public GameObject ThisGameObject;
    public Transform ThisTransform;
    public Rigidbody2D ThisRigidbody2D;
    public Animator ThisAnimator;

    public float MoveSpeed;
    public float JumpForce;
    public bool Grounded;
    public Vector2 Direct;

    public GameObject ThisBodyCheck;
    public GameObject FollowTarget;

    public void MoveRigidbody2D(Vector2 direct)
    {
        ThisRigidbody2D.position += direct * MoveSpeed * Time.deltaTime;
        Direct = direct;

        if (direct == Vector2.left)
        {
            ThisTransform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ThisTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SwitchLine(int UpDown)
    {
        if (ThisGameObject.layer - UpDown < 8 || ThisGameObject.layer - UpDown > 10)
        {
            return;
        }

        ThisGameObject.layer -= UpDown;
        ThisBodyCheck.layer -= UpDown;

        if (UpDown > 0)
        {
            ThisRigidbody2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
        else
        {
            ThisRigidbody2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }

    private GameObject CloneBullet = null;
    private Bullet BulletScript = null;
    public void DoCloneBullet(float FixX, float FixY, string TargetTag)
    {
        Vector3 PlayerPosition = ThisTransform.position;
        Vector3 ClonePosition = new Vector3(PlayerPosition.x + (Direct.x * FixX), PlayerPosition.y + FixY);
        Quaternion CloneRotate = Quaternion.Euler(Vector3.zero);

        CloneBullet = GameObject.Instantiate(Resources.Load("CloneObject/Bullet", typeof(GameObject)), ClonePosition, CloneRotate) as GameObject;
        CloneBullet.layer = ThisGameObject.layer;

        BulletScript = CloneBullet.GetComponent<Bullet>();
        BulletScript.TargetTag = TargetTag;
        BulletScript.Direct = Direct;
    }
}
