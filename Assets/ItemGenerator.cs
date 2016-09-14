using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour {

	// 各prefabを入れる変数
	public GameObject carPrefab;
	public GameObject coinPrefab;
	public GameObject conePrefab;

	// スタート地点
	private int startPos = -160;
	// ゴール地点
	private int goalPos = 120;
	// アイテムを出すx方向の範囲
	private float posRange = 3.4f;

	// Unityちゃんのオブジェクトを入れる変数(位置取得用)
	public GameObject unityChan;
	// Unityちゃんの現在地を入れる変数
	float unityPos;
	// アイテムの生成予定座標を入れる変数
	float i;


	void Start () {
		// アイテムの生成予定座標に生成開始座標を入れて初期化
		i = startPos;
	}
		
	void Update () {
		// Unityちゃんの現在地を絶えず取得
		unityPos = unityChan.transform.position.z;
		// アイテムの生成予定座標がUnityちゃんの現在地の50m先、かつゴール地点より手前ならアイテム生成開始
		if (i <= unityPos + 50 && i <= goalPos) {
			createItem ();
		}
	}

	// アイテム生成関数(一度だけ生成)
	void createItem(){
		// どのアイテムを出すかランダムに設定(この数字が入っていたらこれを出す、みたいな感じ)
		int num = Random.Range(0, 10);
		// 1以下だったらコーンをx軸方向に一直線に生成して敷き詰める(確率20%)
		if (num <= 1) {
			for (float j = -1; j <= 1; j += 0.4f) {
				GameObject cone = Instantiate (conePrefab) as GameObject;
				cone.transform.position = new Vector3 (4 * j, cone.transform.position.y, i);
			}
			// 1より大きかったらコーン以外のアイテムを生成(４つ)
		} else {
			// ３本のレーン(-1と0と1)にランダムでアイテムを配置
			for (int j = -1; j < 2; j++) {
				// アイテムの種類を決める(1～10)
				int item = Random.Range(1, 11);
				// アイテムを置くz軸座標のオフセットをランダムに設定(15の距離ごとに、その前後-5～5にランダム配置)
				int offsetZ = Random.Range(-5, 6);
				// 60%コイン配置、30%車配置、10%何もなし
				// まずは1以上6以下(60%の確率)の場合にコインを生成
				if (1 <= item && item <= 6) {
					// コインを生成
					GameObject coin = Instantiate (coinPrefab) as GameObject;
					coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, i + offsetZ);
					// 次に7以上9以下(30%の確率)の場合には車を生成
				} else if (7 <= item && item <= 9) {
					GameObject car = Instantiate (carPrefab) as GameObject;
					car.transform.position = new Vector3 (posRange * j, car.transform.position.y, i + offsetZ);
				}
				// itemに10が乱数で入った場合は何も配置しないので何のスクリプトも書かない
			}
		}
		// アイテムの最新の生成座標を更新
		i += 15;
	}

}
