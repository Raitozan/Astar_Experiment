using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	public Image img;
	public Sprite pause;
	public Sprite play;
	public bool paused;

	public TileGrid grid;

	public GameObject aiPrefab;

	public GameObject guardPrefab;
	public GameObject normalGuard;
	public GameObject tankGuard;
	public GameObject canonGuard;
	public GameObject bruiserGuard;

	public GameObject previewPrefab;
	public Sprite normalPreview;
	public Sprite tankPreview;
	public Sprite canonPreview;
	public Sprite bruiserPreview;

	public Image normalButton;
	public Image tankButton;
	public Image canonButton;
	public Image bruiserButton;

	public Color normalClr;
	public Color selectedClr;

	public Text score;
	public static float survivedTime;

	public int hp;
	public Text hpText;
	public int gold;
	public float miningTime;
	public float mining;
	public int cost;
	public Text goldText;

	public float spawn;
	public float spawnTimer;
	public int difficultyStage;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		survivedTime = 0.0f;
	}

	// Use this for initialization
	void Start () {
		mining = miningTime;
		spawn = spawnTimer;
		SelectNormal ();
		aiPrefab.GetComponent<AI> ().speed = 1f;
		aiPrefab.GetComponent<AI> ().damage = 1f;
		aiPrefab.GetComponent<AI> ().MaxHp = 10f;
		aiPrefab.GetComponent<AI> ().hp = 10f;
		difficultyStage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		goldText.text = gold.ToString ();
		hpText.text = hp.ToString ();
		if (!paused) {
			survivedTime += Time.deltaTime;
			score.text = "Survived Time: " + ((int)survivedTime).ToString ();
			if (hp <= 0) {
				hp = 10;
				SceneManager.LoadScene ("GameOver");
			}
			mining -= Time.deltaTime;
			if (mining <= 0.0f) {
				gold++;
				mining = miningTime;
			}
			spawn -= Time.deltaTime;
			if (spawn <= 0.0f) {
				spawn = spawnTimer;
				if (spawnTimer >= 6.0f)
					spawnTimer -= 0.14f;
				else if (spawnTimer >= 5.0f) {
					if (difficultyStage == 1) {
						aiPrefab.GetComponent<AI> ().speed += 0.1f;
						aiPrefab.GetComponent<AI> ().damage += 0.2f;
						aiPrefab.GetComponent<AI> ().MaxHp += 1f;
						aiPrefab.GetComponent<AI> ().hp += 1f;
						difficultyStage++;
					}
					spawnTimer -= 0.12f;
				} else if (spawnTimer >= 4.0f) {
					if (difficultyStage == 2) {
						aiPrefab.GetComponent<AI> ().speed += 0.1f;
						aiPrefab.GetComponent<AI> ().damage += 0.2f;
						aiPrefab.GetComponent<AI> ().MaxHp += 1f;
						aiPrefab.GetComponent<AI> ().hp += 1f;
						difficultyStage++;
					}
					spawnTimer -= 0.1f;
				} else if (spawnTimer >= 3.0f) {
					if (difficultyStage == 3) {
						aiPrefab.GetComponent<AI> ().speed += 0.1f;
						aiPrefab.GetComponent<AI> ().damage += 0.2f;
						aiPrefab.GetComponent<AI> ().MaxHp += 1f;
						aiPrefab.GetComponent<AI> ().hp += 1f;
						difficultyStage++;
					}
					spawnTimer -= 0.08f;
				} else if (spawnTimer >= 2.0f) {
					if (difficultyStage == 4) {
						aiPrefab.GetComponent<AI> ().speed += 0.1f;
						aiPrefab.GetComponent<AI> ().damage += 0.2f;
						aiPrefab.GetComponent<AI> ().MaxHp += 1f;
						aiPrefab.GetComponent<AI> ().hp += 1f;
						difficultyStage++;
					}
					spawnTimer -= 0.06f;
				} else if (spawnTimer >= 1.0f) {
					if (difficultyStage == 5) {
						aiPrefab.GetComponent<AI> ().speed += 0.1f;
						aiPrefab.GetComponent<AI> ().damage += 0.2f;
						aiPrefab.GetComponent<AI> ().MaxHp += 1f;
						aiPrefab.GetComponent<AI> ().hp += 1f;
						difficultyStage++;
					}
					spawnTimer -= 0.05f;
				}
					
				Tile start = grid.getSpawningTile ();
				AI ai = Instantiate (aiPrefab, start.transform.position, Quaternion.identity).GetComponent<AI> ();
				ai.startTile = start;
				ai.endTile = grid.grid [grid.width / 2, grid.height / 2];
				ai.init ();
			}
		}
	}

	public void buyGuard(int x, int y) {
		if (gold >= cost && !grid.grid[x,y].guarded) {
			Tile startTile = grid.grid [grid.width / 2, grid.height / 2];
			Tile endTile = grid.grid [x, y];

			if (endTile.terrainCost == -1)
				return;
			GameObject p = Instantiate (previewPrefab, endTile.transform.position, Quaternion.identity);
			Guard g = Instantiate (guardPrefab, startTile.transform.position, Quaternion.identity).GetComponent<Guard> ();
			g.townTile = startTile;
			g.assignmentTile = endTile;
			g.preview = p;
			g.caution = cost;
			g.init ();
			g.assignmentTile.guarded = true;

			gold -= cost;
		}
	}

	public void pauseGame() {
		if (!paused) {
			paused = true;
			img.sprite = play;
		} else {
			paused = false;
			img.sprite = pause;
		}
	}

	public void SelectNormal() {
		guardPrefab = normalGuard;
		previewPrefab.GetComponent<SpriteRenderer> ().sprite = normalPreview;
		cost = 10;
		normalButton.color = selectedClr;
		tankButton.color = normalClr;
		canonButton.color = normalClr;
		bruiserButton.color = normalClr;
	}

	public void SelectTank() {
		guardPrefab = tankGuard;
		previewPrefab.GetComponent<SpriteRenderer> ().sprite = tankPreview;
		cost = 15;
		normalButton.color = normalClr;
		tankButton.color = selectedClr;
		canonButton.color = normalClr;
		bruiserButton.color = normalClr;
	}

	public void SelectCanon() {
		guardPrefab = canonGuard;
		previewPrefab.GetComponent<SpriteRenderer> ().sprite = canonPreview;
		cost = 15;
		normalButton.color = normalClr;
		tankButton.color = normalClr;
		canonButton.color = selectedClr;
		bruiserButton.color = normalClr;
	}

	public void SelectBruiser() {
		guardPrefab = bruiserGuard;
		previewPrefab.GetComponent<SpriteRenderer> ().sprite = bruiserPreview;
		cost = 20;
		normalButton.color = normalClr;
		tankButton.color = normalClr;
		canonButton.color = normalClr;
		bruiserButton.color = selectedClr;
	}
}
