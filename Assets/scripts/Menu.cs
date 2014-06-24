using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

  public string host;
  public string port;

  // Use this for initialization
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update () {
  
  }

  void OnGUI () {
    host = GUILayout.TextField(host);
    port = GUILayout.TextField(port);
    if (GUILayout.Button("PLAY")) {
      PlayerPrefs.SetString("host", host);
      PlayerPrefs.SetInt("port", int.Parse(port));
      Application.LoadLevel("game");
    }
  }
}
