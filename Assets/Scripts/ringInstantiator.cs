using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ringInstantiator : MonoBehaviour {

	public GameObject ring;
	int[] range_x, range_y;
	public float spawn_cooldown = 1f;


	void Start () {
		
		range_x = new int[2] {-20, 20};
		range_y = new int[2] {-20, 20};

		StartCoroutine (Spawner ());

	}

	IEnumerator Spawner () {

		float position_x = Random.Range (range_x [0], range_x [1]);
		float position_y = Random.Range (range_y [0], range_y [1]);

		Instantiate (ring, new Vector3 (position_x, position_y, transform.position.z), Quaternion.identity);

		yield return new WaitForSeconds (spawn_cooldown);
		StartCoroutine (Spawner ());

	}
}
