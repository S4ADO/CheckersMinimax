using UnityEngine;

public class Piece : MonoBehaviour
{
	//Declate piece properties
	public enum type { black, white }
	public type pieceType;
	public Cell position;
	public bool isKing = false;
	public Sprite kingSprite;
	public bool isActive = true;
	private MainGame mainGame;

	//Init
	void Start ()
	{
		mainGame = GameObject.Find("Board").GetComponent<MainGame>();
		transform.position = position.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isActive && position != null)
		{
			Debug.LogError("Inactive piece with none-null position");
		}
	}

	//Create king
	public void makeKing()
	{
		isKing = true;
		GetComponent<SpriteRenderer>().sprite = kingSprite;
	}

	//For humans playing
	void OnMouseDown()
	{
		if (pieceType == type.black)
		{
			if (mainGame.turn == MainGame.Turn.black)
			{
				mainGame.selectedPiece = this;
			}
		}
		else if (pieceType == type.white)
		{
			if (mainGame.turn == MainGame.Turn.white)
			{
				mainGame.selectedPiece = this;
			}
		}
	}
}
