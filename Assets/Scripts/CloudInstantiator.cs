using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudInstantiator : MonoBehaviour {

	public GameObject[] clouds;
	int[] range_x, range_y;
	public float spawn_cooldown = 0f;


	void Start () {
		range_x = new int[2] {-1000, 1000};
		range_y = new int[2] {-100, 65};
		StartCoroutine (Spawner());
	}

	
	IEnumerator Spawner () {

		float position_x = Random.Range (range_x [0], range_x [1]);
		float position_y = Random.Range (range_y [0], range_y [1]);
		int n = Random.Range (0, 2);

		Instantiate (clouds[n], new Vector3 (position_x, position_y, transform.position.z), Quaternion.identity);

		yield return new WaitForSeconds (spawn_cooldown);
		StartCoroutine (Spawner ());

	}
}
