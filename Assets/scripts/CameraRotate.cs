﻿using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

  private Vector2 rotationVelocity;
  public bool isWatching = false;
  public Vector3 watchPoint;

  // Use this for initialization
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update () {
  
    if ( Input.GetMouseButton(0) || Input.GetMouseButton(1) ) {
      rotationVelocity.x += Mathf.Pow(Mathf.Abs(Input.GetAxis("Mouse X")), 1.5f) * Mathf.Sign(Input.GetAxis("Mouse X"));
      rotationVelocity.y -= Input.GetAxis("Mouse Y") * 0.04f;
      isWatching = true;
    }
    else
      isWatching = false;

    Vector3 tmpV = transform.position;
    tmpV.y += rotationVelocity.y;
    transform.position = tmpV;

    transform.RotateAround(Vector3.zero, Vector3.up, rotationVelocity.x);
    rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, Time.deltaTime * 10.0f);

    Vector3 tmpV2 = transform.position;
    tmpV.y = Mathf.Clamp(transform.position.y, 0 , 5);
    transform.position = tmpV2;

    transform.LookAt(watchPoint);

  }
}


