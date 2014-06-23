using UnityEngine;
using System.Collections;

// [ExecuteInEditMode]
public class Interface : MonoBehaviour {

  public Socket socket;
  public MessageHandler messageHandler;
  public string messageReceived = "";
  public string messageToSend = "";
  public Rect[] rect = new Rect[5];

  // Use this for initialization
  void Start () {
    socket = gameObject.GetComponent<Socket>();
    messageHandler = gameObject.GetComponent<MessageHandler>();
  }
  
  // Update is called once per frame
  void Update () {

    if (socket.isConnected) {
      messageReceived = messageHandler.messageReceived;
    }

  }

  void OnGUI () {

    if (!socket.isConnected) {
      if (GUILayout.Button("Connect"))
        socket.init();
    }
    else {
      messageToSend = GUI.TextField(rect[0], messageToSend);
      if (GUI.Button(rect[1], "SEND"))
        socket.sendMessage(messageToSend);
    }
  }
}
