using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  public string id;
  public Vector2 pos;
  public Vector2 tpos;
  public int orientation;
  public int level;
  public string team;
  public Color color;
  public int[] inventory = new int[7];
  private GameObject sq;
  private Square square;
  private Game game;
  static string winId = "";

  static Rect plRect = new Rect(50, 50, 200, 300);

  public enum State {
    idle,
    born,
    move,
    grab,
    cast,
    dead
  }

  public State state;
  private float dTime;


  void Awake () {
    inventory = new int[7];
    state = State.born;
  }

  // Use this for initialization
  void Start () {
    game = GameObject.Find("Game").GetComponent<Game>();
    animation["born"].speed = 0.6f;
  }

  // Update is called once per frame
  void Update () {
    if (state != State.idle) {
      dTime = 0;
      // animation.CrossFade("Idle_stand", 0.1f);
    }
    else {
      dTime += Time.deltaTime;
    }

    if (dTime >= 0.3f) {
      animation.Play("Idle_stand");
    }

    transform.LookAt(new Vector3(tpos.x, transform.position.y, tpos.y));
  }


  void OnMouseUp () {
    if (winId == this.id) {
      winId = "";
    }
    else {
      winId = this.id;
    }
  }

  void OnGUI () {
    if (winId == this.id) {
      plRect = GUILayout.Window(1, plRect, dispWin, "Player " + this.id);
    }
  }

  void dispWin(int windowID) {
    GUI.DragWindow(new Rect(0, 0, 10000, 20));
    GUILayout.Label("Team: " + this.team);
    GUILayout.Label("Lvl: " + this.level);
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
    this.tpos = new Vector2(pos.x + Random.Range(-0.4f, 0.4f), pos.y + Random.Range(-0.4f, 0.4f));
    return this.pos = pos;
  }

  //{N:1, E:2, S:3, O:4}
  public int setOrientation(int orientation) {
    // transform.localRotation = Quaternion.Euler(0, (orientation - 1) * -90 + 180, 0);
    return this.orientation = orientation;
  }

  public int setLevel(int level) {
    // Game.logs += "Player" + this.id + " lvl UP!\n";
    return this.level = level;
  }

  public string setTeam(string team) {
    return this.team = team;
  }

  public Color setColor(Color color) {
    transform.Find("robot_body").renderer.materials[1].color = color;
    transform.Find("robot_head").renderer.materials[1].color = color;
    transform.Find("linerenderer").renderer.material.color = color;
    transform.Find("Spotlight").light.color = color;
    return this.color = color;
  }

  public void setInventory(int[] res) {
    for(var k=0; k < res.Length; k++) {
      this.inventory[k] = res[k];
    }
  }

  public void getResource(int res) {
    state = State.grab;
    animation.CrossFade("grab", 0.1f);
  }

  public void dropResource(int res) {
    state = State.grab;
    animation.CrossFade("grab", 0.1f);
  }

  public bool isCasting = false;

  public void cast() {
    isCasting = true;
    state = State.cast;
    animation.CrossFade("preying", 0.2f);
  }

  public void die() {
    state = State.dead;
    animation.CrossFade("death", 0.2f);
    Game.logs += "Player" + this.id + " died! Peace to his soul\n";
    Destroy(this.gameObject, 5f);
  }


}
