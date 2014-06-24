using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  public string id;
  public Vector2 pos;
  public int orientation;
  public int level;
  public string team;
  public int[] inventory = new int[7];
  private GameObject sq;
  private Square square;
  private Game game;
  static string winId = "";

  static Rect plRect = new Rect(50, 50, 200, 300);

  // Use this for initialization
  void Start () {
    inventory = new int[7];
    game = GameObject.Find("Game").GetComponent<Game>();
  }

  // Update is called once per frame
  void Update () {

  }

  void OnMouseUp () {
    if (winId == this.id)
      winId = "";
    else
      winId = this.id;
  }

  void OnGUI () {
    if (winId == this.id) {
      plRect = GUILayout.Window(1, plRect, dispWin, "Player " + this.id);

    }
  }

  void dispWin(int windowID) {
    GUI.DragWindow(new Rect(0, 0, 10000, 20));
    for (var k=0; k < inventory.Length; k++) {
      GUILayout.Label(game.resourcesName[k] + ": " + this.inventory[k]);
    }
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

  public void setInventory(int[] res) {
    for(var k=0; k < res.Length; k++) {
      this.inventory[k] = res[k];
    }
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
    resource.renderer.material.color = game.resourcesColor[res];
    resource.transform.parent = sq.transform;
    resource.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0.5f, Random.Range(-0.4f, 0.4f));
  }

  public void die() {
    Game.logs += "Player" + this.id + " died! Peace to his soul\n";
    Destroy(this.gameObject);
  }

}
