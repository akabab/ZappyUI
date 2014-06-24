using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

  public int[] resources = new int[7];
  public GameObject[] resGO = new GameObject[7];
  private Game game;

  // Use this for initialization
  void Start () {
    game = GameObject.Find("Game").GetComponent<Game>();

    resources = new int[7];
    resGO = new GameObject[7];

    for (var k=0; k < resources.Length; k++) {
      resGO[k] = Instantiate(game.resourcePrefab) as GameObject;
      resGO[k].name = k.ToString();
      resGO[k].renderer.material.color = game.resourcesColor[k];
      resGO[k].transform.parent = transform;
      resGO[k].transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0.4f, 0.5f), Random.Range(-0.4f, 0.4f));
      resGO[k].transform.localRotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
    }
  }
  
  // Update is called once per frame
  void Update () {
    for (var k=0; k < resources.Length; k++) {
      if (resources[k] == 0)
        resGO[k].SetActive(false);
      else
        resGO[k].SetActive(true);
    }
  }
}
