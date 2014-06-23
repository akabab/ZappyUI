using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  public string id;
  public Vector2 pos;
  public int orientation;
  public int level;
  public string team;
  private GameObject sq;
  private Square square;
  private Game game;

  // Use this for initialization
  void Start () {
    game = GameObject.Find("Game").GetComponent<Game>();
  }
  
  // Update is called once per frame
  void Update () {

  }

  public string setId(string id) {
    this.gameObject.name = id;
    return this.id = id;
  }

  public Vector3 setPosition(Vector2 pos) {
    sq = GameObject.Find(pos.x + " " + pos.y);
    square = sq.GetComponent<Square>();
    return this.pos = pos;
  }

  //{N:1, E:2, S:3, O:4}
  public int setOrientation(int orientation) {
    // transform.localRotation = Quaternion.Euler(0, (orientation - 1) * 90, 0); //
    return this.orientation = orientation;
  }

  public int setLevel(int level) {
    return this.level = level;
  }

  public string setTeam(string team) {
    return this.team = team;
  }

  public void getResource(int res) {
    Transform r = sq.transform.Find(res.ToString());
    Destroy(r.gameObject);
    square.resources[res] -= 1;
  }

  public void dropResource(int res) {
    GameObject resource = Instantiate(game.resourcePrefab) as GameObject;
    square.resources[res] += 1;
    resource.name = res.ToString();
    resource.transform.parent = sq.transform;
    resource.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0.5f, Random.Range(-0.4f, 0.4f));
  }

}
