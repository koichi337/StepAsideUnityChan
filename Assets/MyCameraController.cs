using UnityEngine;
using System.Collections;

public class MyCameraController : MonoBehaviour {

	// Unityちゃんのオブジェクトを入れて操作できるようにする
	private GameObject unitychan;
	// Unityちゃんとカメラの距離
	private float difference;

	// Use this for initialization
	void Start () {
		// Unityちゃんのオブジェクトを取得
		unitychan = GameObject.Find ("unitychan");
		// Unityちゃんとカメラの位置の差を求める(z軸の奥から手前を引くと距離が出る)
		difference = unitychan.transform.position.z - transform.position.z;
	
	}
	
	// Update is called once per frame
	void Update () {
		// Unityちゃんからdifference分だけ後ろの位置に絶えずカメラを追従させる
		transform.position = new Vector3 (0, transform.position.y, unitychan.transform.position.z - difference);
	
	}
}
