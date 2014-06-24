using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageHandler : MonoBehaviour {

  private Socket socket;
  private Game game;
  public string messageReceived = "";
  private GameObject worldGO;

  // Use this for initialization
  void Start () {
    worldGO = GameObject.Find("World");
    socket = gameObject.GetComponent<Socket>();
    game = gameObject.GetComponent<Game>();
  }
  
  // Update is called once per frame
  void Update () {
    if (socket.isConnected) {
      messageReceived = socket.listen();

      if (messageReceived != null) {
        Debug.Log(messageReceived);
        handleMessage(messageReceived);
      }
    }
  }

  void OnGUI () {

  }

  void loadResources(Vector2 pos, int[] res) {
    GameObject sq = GameObject.Find(pos.x + " " + pos.y);
    Square square = sq.GetComponent<Square>();

    for (var k = 0; k < res.Length; k++) {
      square.resources[k] = res[k];
      for (var n = 0; n < res[k]; n++) {
        GameObject resource = Instantiate(game.resourcePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        resource.name = k.ToString();
        resource.renderer.material.color = game.resourcesColor[k];
        resource.transform.parent = sq.transform;
        resource.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0.5f, Random.Range(-0.4f, 0.4f));
      }
    }
  }
 

  void createMap(int width, int height) {
    //Center Map
    // worldGO.transform.position = new Vector3(-(width / 2.0f - 0.5f), 0, -(height / 2f - 0.5f));

    for (int j = 0; j < height; j++) {
      for (int i = 0; i < width; i++) {
        GameObject sq = Instantiate(game.sqPrefab) as GameObject;
        sq.transform.localPosition = new Vector3(i, 0, j);
        sq.transform.rotation = Quaternion.identity;
        sq.name = i + " " + j;
        sq.transform.parent = worldGO.transform;
      }
    }
  }

  void newPlayer(string id, Vector2 pos, int orientation, int level, string team) {
    GameObject player = Instantiate(game.playerPrefab) as GameObject;
    // player.transform.parent = worldGO.transform;
    player.transform.position = new Vector3(pos.x, 0.5f, pos.y);
    Player pl = player.GetComponent<Player>();
    game.players.Add(pl);
    pl.setId(id);
    pl.setPosition(pos);
    pl.setOrientation(orientation);
    pl.setLevel(level);
    pl.setTeam(team);
    // socket.sendMessage("pin " + id + "\n");
  }

  public Player getPlayerById(string id) {
    foreach(Player pl in game.players) {
      if (pl.id == id) {
        return pl;
      }
    }
    return null;
  }

  private int _bct;

  public void handleMessage(string message) {

    string[] msg = message.Split(' ');

    string    id;
    Vector2   pos;
    int       orientation;
    int       level;
    string    team;
    int[]     res;
    int       resource;
    // string    messg;
    Player    player;

    switch (msg[0]) {

      case "BIENVENUE":
        socket.sendMessage("GRAPHIC");
        break;
      //Map Size
      case "msz":
        Game.logs += "Generating Map\n";
        pos = new Vector2(int.Parse(msg[1]), int.Parse(msg[2]));
        game.setMapSize(int.Parse(msg[1]), int.Parse(msg[2]));
        createMap(int.Parse(msg[1]), int.Parse(msg[2]));
        Camera.main.transform.position = new Vector3(pos.x / 2f, 5, -(pos.y / 2f));
        Camera.main.GetComponent<CameraOrbit>().target = new Vector3(pos.x / 2f, 0, pos.y / 2f);
        break;
      //Square content
      case "bct":
        // game.logs += "Loading Resources\n";
        pos = new Vector2(int.Parse(msg[1]), int.Parse(msg[2]));
        res = new int[7] {
                            int.Parse(msg[3]),
                            int.Parse(msg[4]),
                            int.Parse(msg[5]),
                            int.Parse(msg[6]),
                            int.Parse(msg[7]),
                            int.Parse(msg[8]),
                            int.Parse(msg[9])
                          };
        if (_bct < (Game.mapSize.x * Game.mapSize.y)) {
          loadResources(pos, res);
          _bct++;
        }
        break;
      //Team Name
      case "tna":
        team = msg[1];
        Game.logs += "New team joined: '" + team + "'\n";
        game.teams.Add(team);
        break;
      //New Player
      case "pnw":
        id = msg[1];
        Game.logs += "New player connected: " + id + "\n";
        pos = new Vector2(int.Parse(msg[2]), int.Parse(msg[3]));
        orientation = int.Parse(msg[4]);
        level = int.Parse(msg[5]);
        team = msg[6];
        newPlayer(id, pos, orientation, level, team);
        break;
      //Player position & orientation
      case "ppo":
        id = msg[1];
        pos = new Vector2(int.Parse(msg[2]), int.Parse(msg[3]));
        orientation = int.Parse(msg[4]);
        player = getPlayerById(id);
        player.setPosition(pos);
        player.setOrientation(orientation);
        break;
      //Player Level
      case "plv":
        id = msg[1];
        level = int.Parse(msg[2]);
        player = getPlayerById(id);
        player.setLevel(level);
        break;
      //Player inventory
      case "pin":
        id = msg[1];
        pos = new Vector2(int.Parse(msg[2]), int.Parse(msg[3]));
        res = new int[7] {
                            int.Parse(msg[4]),
                            int.Parse(msg[5]),
                            int.Parse(msg[6]),
                            int.Parse(msg[7]),
                            int.Parse(msg[8]),
                            int.Parse(msg[9]),
                            int.Parse(msg[10])
                          };
        player = getPlayerById(id);
        player.setInventory(res);
        break;
      //Player Expulse
      case "pex":
        id = msg[1];
        // ?
        break;
      //Player broadcast
      case "pbc":
        id = msg[1];
        // messg = msg[2];
        //broadcastMessage(id, messg);
        break;
      //
      case "pic":
        break;
      case "pie":
        break;
      case "pfk":
        break;
      //Player drop ressource
      case "pdr":
        id = msg[1];
        resource = int.Parse(msg[2]);
        player = getPlayerById(id);
        player.dropResource(resource);
        break;
      //Player get ressource
      case "pgt":
        id = msg[1];
        resource = int.Parse(msg[2]);
        player = getPlayerById(id);
        player.getResource(resource);
        break;
      //Player is death starving
      case "pdi":
        id = msg[1];
        player = getPlayerById(id);
        player.die();
        break;
      case "sgt":
        game.t = int.Parse(msg[1]);
        break;
    }
  }
}
