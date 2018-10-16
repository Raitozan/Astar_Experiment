using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float speed;

	public GameObject bullet;
	public float reloadTime;
	public float reloadCooldown;
	public bool reloading;

	public int hp;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate ()
	{
		if (reloading) {
			reloadCooldown -= Time.deltaTime;
			if (reloadCooldown <= 0)
				reloading = false;
		}
		if (Input.GetButton("Fire1") && !reloading) {
			Bullet b = Instantiate (bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
			Vector3 mp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mp.z = 0.0f;
			b.direction = mp - transform.position;
			b.direction.Normalize ();
			b.go = true;
			reloadCooldown = reloadTime;
			reloading = true;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
		float moveVertical = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

		transform.position = new Vector3 (transform.position.x + moveHorizontal, transform.position.y + moveVertical, 0.0f);
	}
}
