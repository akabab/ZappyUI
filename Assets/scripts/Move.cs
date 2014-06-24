using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

  public float speed = 1.0f;
  public float smooth = 1.0f;
  public Vector3 targetPos;
  public Vector3 targetRot;
  public Vector3 dir;
  public float dist;
  public bool isMoving = false;
  private GameObject gameGO;
  private Player pl;
  private Game game;

  // Use this for initialization
  void Start () {
    pl = GetComponent<Player>();
    gameGO = GameObject.Find("Game");
    game = gameGO.GetComponent<Game>();
  }
  
  // Update is called once per frame
  void Update () {

    targetPos = new Vector3(pl.pos.x, transform.position.y, pl.pos.y);

    dir = targetPos - transform.position;
   
    // magnitude is the total length of a vector.
    // getting the magnitude of the direction gives us the amount left to move
    float dist = dir.magnitude;
   
    // this makes the length of dir 1 so that you can multiply by it.
    dir = dir.normalized;
   
    // the amount we can move this frame
    float move = (game.t / 7) * speed * Time.deltaTime; //(7 / game.t) * 
   
    // limit our move to what we can travel.
    if (move > dist) {
      move = dist;
      isMoving = false;
    }
    else {
      isMoving = true;
    }

    // apply the movement to the object.
    transform.Translate(dir * move, Space.World);

    // apply rotation
    // Quaternion target = Quaternion.Euler(0, (pl.orientation - 1) * 90, 0);
    // transform.localRotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
  }
}
