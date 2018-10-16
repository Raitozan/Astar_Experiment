using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard : MonoBehaviour {
	
	public List<Tile> path;
	public Tile townTile;
	public Tile assignmentTile;
	public Tile nextTile;
	public int next;
	public float speed;
	public bool moving;

	public GameObject preview;

	public float radius;
	public LayerMask aiLM;
	public LayerMask wallLM;
	public bool firing;
	public Transform target;
	public GameObject bullet;
	public float damage;

	public float reloadTime;
	public float reloadCooldown;
	public bool reloading;

	public int caution;
	public bool normal;
	public int ammo;
	public bool goingBack;

	public SpriteRenderer sr;
	public Sprite hp100;
	public Sprite hp75;
	public Sprite hp50;
	public Sprite hp25;
	public int MaxHp;
	public float hp;


	// Use this for initialization
	void Start () {
	}

	void FixedUpdate ()
	{
		if (!GameManager.instance.paused) {
			checkSprite ();
			if (hp <= 0) {
				assignmentTile.guarded = false;
				Destroy (preview.gameObject);
				Destroy (gameObject);
			}

			if (reloading) {
				reloadCooldown -= Time.deltaTime;
				if (reloadCooldown <= 0)
					reloading = false;
			}

			RaycastHit2D[] hits = Physics2D.CircleCastAll (transform.position, radius, transform.forward, radius, aiLM);
			List<Transform> t = new List<Transform> ();
			foreach (RaycastHit2D hit in hits) {
				if (!Physics2D.Raycast (transform.position, hit.collider.transform.position - transform.position, (hit.collider.transform.position - transform.position).magnitude, wallLM)) {
					t.Add (hit.collider.transform);
				}
			}
			if (t.Count > 0) {
				float min = -1;
				Transform minT = null;
				foreach (Transform tr in t) {
					if (min == -1 || (tr.transform.position - transform.position).magnitude < min) {
						min = (tr.transform.position - transform.position).magnitude;
						minT = tr;
					}
				} 
				target = minT;
				firing = true;
			} else {
				firing = false;
			}
			if (firing && !reloading && target != null) {
				Bullet b = Instantiate (bullet, transform.position, Quaternion.identity).GetComponent<Bullet> ();
				b.target = "AI";
				b.direction = target.position - transform.position;
				b.direction.Normalize ();
				b.go = true;
				b.damage = damage;
				reloadCooldown = reloadTime;
				reloading = true;
				ammo--;
			}

			if (moving) {
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, nextTile.transform.position, step);
				if (transform.position == nextTile.transform.position) {
					if (next == path.Count) {
						if (goingBack) {
							if(!normal)
								GameManager.instance.gold+=caution;
							Destroy (gameObject);
						}
						moving = false;
						Destroy (preview.gameObject);
					}
					else
						nextTile = path [next++];
				}
			} else {
				if (ammo <= 0) {
					goingBack = true;
					path.Reverse ();
					next = 0;
					nextTile = path [next++];
					moving = true;
					assignmentTile.guarded = false;
				}
			}
		}
	}

	public void init() {
		a_Star (townTile, assignmentTile);
		next = 0;
		nextTile = path [next++];
		moving = true;
	}

	public void a_Star(Tile startTile, Tile endTile) {
		path = new List<Tile> ();
		List<Tile> openSet = new List<Tile> ();
		Dictionary<Tile, int> costToGo = new Dictionary<Tile, int>();
		Dictionary<Tile, int> totalCost = new Dictionary<Tile, int>();
		List<Tile> closedSet = new List<Tile> ();
		Dictionary<Tile, Tile> predecessor = new Dictionary<Tile, Tile>();

		openSet.Add (startTile);
		costToGo.Add (startTile, 0);
		totalCost.Add(startTile, getDistance(startTile, endTile));

		while (openSet.Count > 0) {
			Tile currentTile = lowestCost (openSet, totalCost);
			openSet.Remove (currentTile);
			closedSet.Add (currentTile);

			if (currentTile == endTile) {
				while(currentTile != startTile){
					path.Add(currentTile);
					currentTile = predecessor[currentTile];
				}
				path.Add(currentTile);
				path.Reverse();
				return;
			}

			foreach (Tile t in GameManager.instance.grid.getNeighbours(currentTile)) {
				if (closedSet.Contains (t))
					continue;

				int togo = costToGo [currentTile] + getDistance (currentTile, t);
				int total = togo + getDistance (t, endTile);
				if (!openSet.Contains (t)) {
					openSet.Add (t);
					costToGo.Add (t, togo);
					totalCost.Add (t, total);
					predecessor [t] = currentTile;
				} else if (togo < costToGo [t]) {
					costToGo [t] = togo;
					totalCost [t] = total;
					predecessor [t] = currentTile;
				}
			}
		}
	}

	public Tile lowestCost(List<Tile> tiles, Dictionary<Tile, int> cost) {
		Tile lowest = tiles[0];
		int minCost = -1;
		for(int i=0; i<tiles.Count; i++){
			if (cost [tiles[i]] < minCost || minCost == -1) {
				lowest = tiles [i];
				minCost = cost[tiles[i]];
			}
		}
		return lowest;
	}

	public int getDistance(Tile tileA, Tile tileB) {
		int distX = Mathf.Abs (tileA.x - tileB.x);
		int distY = Mathf.Abs (tileA.y - tileB.y);

		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		else
			return 14 * distX + 10 * (distY - distX);
	}

	public void checkSprite() {
		if (hp <= 0.25 * MaxHp)
			sr.sprite = hp25;
		else if (hp <= 0.5 * MaxHp)
			sr.sprite = hp50;
		else if (hp <= 0.75 * MaxHp)
			sr.sprite = hp75;
		else
			sr.sprite = hp100;
	}
}
