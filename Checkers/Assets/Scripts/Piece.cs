using UnityEngine;

public class Piece : MonoBehaviour
{
	//Declate piece properties
	public enum Type { black, white }
	public Type type;
	public Cell cell;
	public bool isKing = false;
	public Sprite kingSprite;
	public bool isActive = true;
	private MainGame mainGame;

	//Init
	void Start ()
	{
		mainGame = GameObject.Find("Board").GetComponent<MainGame>();
		transform.position = cell.transform.position;
	}

	public void movePiece(Cell newCell)
	{
		cell = newCell;
		transform.position = cell.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isActive && cell != null)
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
		if (type == Type.black)
		{
			if (mainGame.turn == MainGame.Turn.black)
			{
				mainGame.selectedPiece = this;
			}
		}
		else if (type == Type.white)
		{
			if (mainGame.turn == MainGame.Turn.white)
			{
				mainGame.selectedPiece = this;
			}
		}
	}
}
