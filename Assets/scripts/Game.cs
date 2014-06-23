using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

  public Vector2 mapSize;
  public int t;
  public List<string> teams = new List<string>();
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
  public GameObject sqPrefab;
  public GameObject resourcePrefab;
  public GameObject playerPrefab;

  // Use this for initialization
  void Start () {
    // Time.timeScale = ?;
  }
  
  // Update is called once per frame
  void Update () {
  
  }
}
