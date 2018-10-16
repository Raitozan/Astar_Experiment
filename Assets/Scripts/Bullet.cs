using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 direction;
	public float speed;
	public float lifetime;
	public bool go = false;
	public string target;
	public float damage;

	void FixedUpdate(){
		if (!GameManager.instance.paused) {
			if (go) {
				lifetime -= Time.deltaTime;
				if (lifetime <= 0.0f)
					Destroy (gameObject);

				transform.Translate (direction * speed * Time.fixedDeltaTime);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag ("Wall"))
			Destroy (gameObject);
		if (target == "AI") {
			if (col.gameObject.CompareTag ("AI")) {
				col.gameObject.GetComponent<AI> ().hp-=damage;
				Destroy (gameObject);
			}
		} else if (target == "Guard") {
			if (col.gameObject.CompareTag ("Guard")) {
				col.gameObject.GetComponent<Guard> ().hp-=damage;
				Destroy (gameObject);
			}
		}
	}
}
