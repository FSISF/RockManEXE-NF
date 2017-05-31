using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform CameraTransform = null;
    [SerializeField]
    private Camera ThisCamera = null;

    [SerializeField]
    private Transform FollowTarget = null;
    private Vector3 TargetVector;

    [SerializeField]
    private Transform MapTransform = null;
    [SerializeField]
    private BoxCollider2D BoxCollider = null;
    private float CameraDoubleSize = 0;
    private float MapLineLong = 0;

    private void Start()
    {
        CameraDoubleSize = ThisCamera.orthographicSize * 2;
        MapLineLong = BoxCollider.size.x;
    }

    private void Update()
    {
        if (FollowTarget != null)
        {
            TargetVector = FollowTarget.position;

            CameraTransform.position = new Vector3(TargetVector.x, 0, -10);

            if (CameraTransform.position.x < MapTransform.position.x + CameraDoubleSize)
            {
                CameraTransform.position = new Vector3(MapTransform.position.x + CameraDoubleSize, 0, -10);
            }

            if (CameraTransform.position.x > MapTransform.position.x + MapLineLong - CameraDoubleSize)
            {
                CameraTransform.position = new Vector3(MapTransform.position.x + MapLineLong - CameraDoubleSize, 0, -10);
            }
        }
    }
}
