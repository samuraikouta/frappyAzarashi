using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour 
{
	//ゲームステート
	enum State
	{
		Ready,
		Play,
		GameOver
	}

	State state;
	int score;

	public AzarashiController azarashi;
	public GameObject blocks;
	public Text scoreLabel;
	public Text stateLabel;
	public Text highScoreLabel;

	void Start()
	{
		//開始と同時にReadyステートに移行
		Ready();
	}

	void LateUpdate()
	{
		//ゲームのステート毎にイベントを開始
		switch(state)
		{
			case State.Ready:
				//タッチしたらゲームスタート
				if (Input.GetButtonDown("Fire1"))
					GameStart();
				break;
			case State.Play:
				//キャラクターが死亡したらゲームオーバー
				if (azarashi.IsDead())
					GameOver();
				break;
			case State.GameOver:
				//タッチしたらシーンをリロード
				if (Input.GetButtonDown("Fire1"))
					Reload();
				if(PlayerPrefs.GetInt("HighScore") < score)
				{
					PlayerPrefs.SetInt("HighScore", score);
				}
				break;
		}
	}

	void Ready()
	{
		state = State.Ready;
		//各オブジェクトを無効状態にする
		azarashi.SetSteerActive(false);
		blocks.SetActive(false);

		//ラベルを更新
		scoreLabel.text = "Score : " + 0;

		stateLabel.gameObject.SetActive(true);
		stateLabel.text = "Ready";

		//ハイスコアを更新
		highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore");
	}

	void GameStart()
	{
		state = State.Play;

		//各オブジェクトを有効にする
		azarashi.SetSteerActive(true);
		blocks.SetActive(true);

		//最初の入力だけゲームコントローラーから渡す
		azarashi.Flap();

		//ラベルを更新
		stateLabel.gameObject.SetActive(false);
		stateLabel.text = "";

		//ハイスコアを更新
		highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore");
	}

	void GameOver()
	{
		state = State.GameOver;

		//シーン中のすべてのScrollObjectコンポーネントを探し出す
		ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();

		//全ScrollObjectのスクロール処理を無効にする
		foreach (ScrollObject so in scrollObjects) 
			so.enabled = false;

		//ラベルを更新
		stateLabel.gameObject.SetActive(true);
		stateLabel.text = "GameOver";

		//ハイスコアを更新
		highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore");
	}

	void Reload()
	{
		//現在読み込んでいるシーンを再読み込み
		Application.LoadLevel(Application.loadedLevel);
	}

	public void IncreaseScore()
	{
		score++;
		scoreLabel.text = "Score : " + score;
	}
}
