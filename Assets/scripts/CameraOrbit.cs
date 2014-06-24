using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

  public Vector3 target;
  public float damping = 6.0f;
  public float speed = 1.0f;
  public bool smooth = true;
  public bool rotate = false;

  // Use this for initialization
  void Start () {
  }
  
  // Update is called once per frame
  void Update () {
  }

  void LateUpdate () {
    if (smooth)
    {
      // Look at and dampen the rotation
      var rotation = Quaternion.LookRotation(target - transform.position);
      transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
    else
        transform.LookAt(target);
    if (rotate)
      transform.RotateAround(target, Vector3.up, speed * Time.deltaTime);
  }

}
