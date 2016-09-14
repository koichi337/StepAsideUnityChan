using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {
	
	// このオブジェクト自身のアニメを操作するための変数
	private Animator myAnimator;

	// Unityちゃんを移動させるコンポーネントを入れる（追加）
	private Rigidbody myRigidbody;
	// 前進するための力
	private float forwardForce = 800.0f;
	// 左右移動のための力
	private float turnForce = 500.0f;
	// ジャンプする力
	private float upForce = 500.0f;
	// 左右移動できる範囲
	private float movableRange = 3.4f;
	// 衝突時に動きを原則させる係数
	private float coefficient = 0.95f;

	// ゲーム終了の判定
	private bool isEnd = false;

	// ゲーム終了時に表示するテキスト操作のための変数
	private GameObject stateText;
	// スコアを表示するテキスト
	private GameObject scoreText;
	// 得点
	private int score = 0;

	// スマホ用の左右ボタン判定変数
	private bool isLButtonDown = false;
	private bool isRButtonDown = false;


	void Start () {
		
		// このオブジェクトにアタッチされたAnimatorコンポーネントを変数に入れて操作できるようにする
		myAnimator = GetComponent<Animator> ();

		// アニメにセットしたパラメーターを変更する(Speedが0.1以上だと歩行、0.8以上だと走るアニメになる設定になっている)
		myAnimator.SetFloat("Speed", 1.0f);

		// 物理演算を使うためにRigidbodyを仕えるようにしておく
		myRigidbody = GetComponent<Rigidbody>();

		// ゲーム終了テキストを操作するためのオブジェクト取得
		stateText = GameObject.Find("GameResultText");

		// スコアのUIオブジェクトを取得しておく
		scoreText = GameObject.Find("ScoreText");
	
	}


	void Update () {

		// 何かに衝突してゲーム終了フラグがtrueになったら、各係数に0.95fを何度もかけて限りなく0に近づけ減速させる
		if(isEnd == true){
			forwardForce *= coefficient;
			turnForce *= coefficient;
			upForce *= coefficient;
			myAnimator.speed *= coefficient;
		}
		
		// transform.forwardでオブジェクトが向いている方向のベクトルを取得できる
		myRigidbody.AddForce (transform.forward * forwardForce);
		// 左右移動
		if ((Input.GetKey (KeyCode.LeftArrow) || isLButtonDown == true) && transform.position.x > -movableRange) {
			// 単位ベクトルにmovableForceをかけた myRigidbody.AddForce(Vector3.left * turnForce); などでも可
			// AddForceは座標ではなく加える力の方向なのでnew Vector3ではない
			myRigidbody.AddForce (-turnForce, 0, 0);
		} else if ((Input.GetKey (KeyCode.RightArrow) || isRButtonDown == true) && transform.position.x < movableRange) {
			myRigidbody.AddForce (turnForce, 0, 0);
		}

		// 現在のアニメーションの状態をGetCurrentAnimatorStateInfo(0)で取得し
		// そのアニメーション名がJumpかどうかをIsName("Jump")で調べる
		if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
			// ジャンプアニメーション中だったら条件をfalseにして移動アニメに戻す
			myAnimator.SetBool("Jump", false);
		}


		// ジャンプしていない時にスペースが押されたらジャンプする
		if(Input.GetKeyDown(KeyCode.Space) && transform.position.y < 0.5f){
			// ジャンプアニメに切り替え
			myAnimator.SetBool ("Jump", true);
			// Unityちゃんに上方向の力を加える
			myRigidbody.AddForce(transform.up * upForce);
		}
	
	}

	void OnTriggerEnter(Collider other){
		// 衝突したオブジェクトが車かコーンだった場合
		if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
			isEnd = true;
			stateText.GetComponent<Text>().text = "GAME OVER";
		}
		// 衝突したオブジェクトがゴールだった場合
		if(other.gameObject.tag == "GoalTag"){
			isEnd = true;
			stateText.GetComponent<Text>().text = "CLEAR!!";
		}

		// 衝突したオブジェクトがコインだった場合
		if(other.gameObject.tag == "CoinTag"){
			// スコアを加算
			score += 10;
			// 加算したテキストに書き換える
			scoreText.GetComponent<Text>().text = "Score " + score + "pt";
			// このオブジェクトに設定したパーティクルを再生
			GetComponent<ParticleSystem> ().Play ();
			//コインオブジェクトを消去
			Destroy(other.gameObject);
		}
	}

	// ジャンプボタンを押した場合の処理(スマホ用)
	public void GetMyJumpButtonDown(){
		if (transform.position.y < 0.5f) {
			myAnimator.SetBool ("Jump", true);
			myRigidbody.AddForce (transform.up * upForce);
		}
	}

	// 左ボタンを押した場合の処理(スマホ用)
	public void GetMyLeftButtonDown(){
		isLButtonDown = true;
	}
	// 左ボタンを離した場合の処理(スマホ用)
	public void GetMyLeftButtonUp(){
		isLButtonDown = false;
	}

	// 右ボタンを押した場合の処理(スマホ用)
	public void GetMyRightButtonDown(){
		isRButtonDown = true;
	}
	// 右ボタンを離した場合の処理(スマホ用)
	public void GetMyRightButtonUp(){
		isRButtonDown = false;
	}


}
