using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameUI: MonoBehaviour
{
	public MainGame game;
	public Button b1, b2, b3;

	void Start()
	{
		game.gameObject.SetActive(false);
	}

	//Human vs Search AI
	public void play1()
	{
		game.gameType = MainGame.GameType.hvs;
		game.gameObject.SetActive(true);
		b1.gameObject.SetActive(false);
		b2.gameObject.SetActive(false);
		b3.gameObject.SetActive(false);
	}

	//Human vs Tactical AI
	public void play2()
	{
		game.gameType = MainGame.GameType.hvt;
		game.gameObject.SetActive(true);
		b1.gameObject.SetActive(false);
		b2.gameObject.SetActive(false);
		b3.gameObject.SetActive(false);
	}

	//AI vs AI
	public void play3()
	{
		game.gameType = MainGame.GameType.tvs;
		game.gameObject.SetActive(true);
		b1.gameObject.SetActive(false);
		b2.gameObject.SetActive(false);
		b3.gameObject.SetActive(false);
	}
}
