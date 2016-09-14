using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
