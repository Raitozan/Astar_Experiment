using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour {

	public int width;
	public int height;
	public Tile[,] grid;
	public GameObject tile;

	public Sprite grassLand;
	public Sprite hill;
	public Sprite forest;
	public Sprite lake;
	public Sprite mountain;

	public float grassLandProb;
	public float hillProb;
	public float forestProb;
	public float lakeProb;
	public float mountainProb;


	// Use this for initialization
	void Start () {
		grid = new Tile[width,height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				Tile t = Instantiate (tile, new Vector3 (x + this.transform.position.x, y + this.transform.position.y, 0), Quaternion.identity, this.transform).GetComponent<Tile> ();
				t.x = x;
				t.y = y;
				if (x == width / 2 && y == height / 2) {
					grid [x, y] = t;
					continue;
				}
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					t.sr.sprite = grassLand;
					t.terrainCost = 1;
				} else {
					float type = Random.Range (0.0f, 1.0f);
					if (type <= mountainProb) {
						t.sr.sprite = mountain;
						t.terrainCost = -1;
						t.gameObject.tag = "Wall";
						t.gameObject.layer = 9;
					} else if (type <= mountainProb + lakeProb) {
						t.sr.sprite = lake;
						t.terrainCost = 4;
					} else if (type <= mountainProb + lakeProb + forestProb) {
						t.sr.sprite = forest;
						t.terrainCost = 3;
					} else if (type <= mountainProb + lakeProb + forestProb + hillProb) {
						t.sr.sprite = hill;
						t.terrainCost = 2;
					} else {
						t.sr.sprite = grassLand;
						t.terrainCost = 1;
					}
				}
					grid [x, y] = t;
			}
		}
	}

	public List<Tile> getNeighbours(Tile t) {
		List<Tile> neighbours = new List<Tile> ();

		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				if (x == 0 && y == 0)
					continue;

				int newX = t.x + x;
				int newY = t.y + y;

				if (newX >= 0 && newX < width && newY >= 0 && newY < height && grid[newX, newY].terrainCost != -1) {
					if(x != 0 && y != 0) {
						if(grid[newX, newY-y].terrainCost != -1 && grid[newX-x, newY].terrainCost != -1)
							neighbours.Add (grid [newX, newY]);
					}
					else
						neighbours.Add (grid [newX, newY]);
				}
			}
		}

		return neighbours;
	}

	public Tile getSpawningTile() {
		int x, y;
		if (Random.Range (0, 2) < 1) {
			if (Random.Range (0, 2) < 1)
				x = 0;
			else
				x = width - 1;
			y = Random.Range (0, height);
		} else {
			if (Random.Range (0, 2) < 1)
				y = 0;
			else
				y = height - 1;
			x = Random.Range (0, width);
		}

		return grid [x, y];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
