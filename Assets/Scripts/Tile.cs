using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public SpriteRenderer sr;
	public int x;
	public int y;
	public int terrainCost;
	public bool guarded;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int toCoordinate(float f) {
		if (f - (int)f >= 0.5f)
			return (int)f + 1;
		else
			return (int)f;
	}

	void OnMouseDown() {
		Vector2 coord = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		coord.x = toCoordinate (coord.x);
		coord.y = toCoordinate (coord.y);

		GameManager.instance.buyGuard (x, y);
	}
}
