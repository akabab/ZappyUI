using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class Socket : MonoBehaviour {

  private TcpClient     socket;
  private NetworkStream stream;
  private StreamWriter  writer;
  private StreamReader  reader;

  public string host;
  public int port;
  public bool isConnected = false;

  void Start () {
    host = PlayerPrefs.GetString("host");
    port = PlayerPrefs.GetInt("port");
    init();
  }

  public void init()
  {
    try {
      socket = new TcpClient(host, port);
      stream = socket.GetStream();
      stream.ReadTimeout = 1;
      writer = new StreamWriter(stream);
      reader = new StreamReader(stream);
      isConnected = true;
    }
    catch (Exception e) {
      isConnected = false;
      Game.logs += "Socket connection error: " + e;
      Debug.Log("Socket connection error: " + e);
    }
  }

  public void sendMessage(string message)
  {
    writer.Write(message + "\n");
    writer.Flush();
  }

  public string listen()
  {
    try {
      return reader.ReadLine();
    } catch (Exception e) {
      return null;
    }
  }

  public void closeSocket()
  {
    writer.Close();
    reader.Close();
    socket.Close();
    isConnected = false;
  }

}
