using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
  public float xAngle;
  public float yAngle;
  public Vector2 xAngleLimits;
  public Vector2 yAngleLimits;

  // Use this for initialization
  void Start () {
    xAngle = (xAngleLimits.y - xAngleLimits.x) / 2; //Set the camera X Angle to the medium value
    yAngle = yAngleLimits.x;
  }
  
  // Update is called once per frame
  void Update () {
    yAngle += Input.GetAxis("Horizontal");
    xAngle += Input.GetAxis("Vertical");

    if ( yAngle > yAngleLimits.y) yAngle = yAngleLimits.x;
    if ( yAngle < yAngleLimits.x) yAngle = yAngleLimits.y;
    if ( xAngle > xAngleLimits.y) xAngle = xAngleLimits.y;
    if ( xAngle < xAngleLimits.x) xAngle = xAngleLimits.x;

    // transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
  }
}
