using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

  public Vector2 mapSize;
  public int t;
  public List<string> teams = new List<string>();
  public Color[] teamsColor = new Color[10];
  public List<Player> players = new List<Player>();
  public string[] resourcesName = new string[7] {
                                                  "Food",
                                                  "LineMate",
                                                  "Deraumere",
                                                  "Sibur",
                                                  "Mendiane",
                                                  "Phiras",
                                                  "Thystame"
                                                 };
  public Color[] resourcesColor = new Color[7];
  public GameObject sqPrefab;
  public GameObject resourcePrefab;
  public GameObject playerPrefab;
  public static string logs;

  // Use this for initialization
  void Start () {
    // Time.timeScale = ?;
  }
  
  // Update is called once per frame
  void Update () {
  
  }

  public Vector2 setMapSize(int width, int height) {
    return mapSize = new Vector2(width, height);
  }
}
