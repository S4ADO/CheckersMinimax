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
		setGame(Board.GameType.hvs);
	}

	//Human vs Tactical AI
	public void play2()
	{
		setGame(Board.GameType.hvt);
	}

	//AI vs AI
	public void play3()
	{
		setGame(Board.GameType.tvs);
	}

	void setGame(Board.GameType type)
	{
		game.gameObject.SetActive(true);
		game.init(type);
		b1.gameObject.SetActive(false);
		b2.gameObject.SetActive(false);
		b3.gameObject.SetActive(false);
	}
}
