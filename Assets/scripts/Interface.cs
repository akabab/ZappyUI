using UnityEngine;
using System.Collections;

// [ExecuteInEditMode]
public class Interface : MonoBehaviour {

  public Socket socket;
  public Game game;
  public MessageHandler messageHandler;
  public string messageReceived = "";
  public string messageToSend = "";
  public Rect[] rect;
  public Rect logRect;
  public bool displayLog;
  public Vector2 scrollPosition;
  private string logs;


  // Use this for initialization
  void Start () {
    socket = gameObject.GetComponent<Socket>();
    game = gameObject.GetComponent<Game>();
    messageHandler = gameObject.GetComponent<MessageHandler>();
  }
  
  // Update is called once per frame
  void Update () {

    if (socket.isConnected) {
      messageReceived = messageHandler.messageReceived;
    }

    if (logs != Game.logs)
      scrollPosition = new Vector2(0, 100000000);

    logs = Game.logs;
  }

  void OnGUI () {

    if (!socket.isConnected) {
      if (GUILayout.Button("Connect"))
        socket.init();
    }
    // else {
    //   messageToSend = GUI.TextField(rect[0], messageToSend);
    //   if (GUI.Button(rect[1], "SEND"))
    //     socket.sendMessage(messageToSend);
    // }

    if (displayLog) {
      logRect = GUILayout.Window(0, logRect, logWin, "Debug");
    }
  }

  void logWin(int windowID) {
    GUI.DragWindow(new Rect(0, 0, 10000, 20));

    GUI.BeginGroup(rect[2]);
    scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(100));
    GUILayout.Label(logs);
    GUILayout.EndScrollView();
    GUI.EndGroup();
  }
}
