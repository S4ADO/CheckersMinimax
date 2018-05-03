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
		transform.position = new Vector3(transform.position.x, transform.position.y+0.0001f, 0);
	}

	public void movePiece(Cell newCell)
	{
		cell.piece = null;
		cell = newCell;
		cell.piece = this;
		transform.position = cell.transform.position;
	}

	public void remove()
	{
		cell.piece = null;
		isActive = false;
		cell = null;
		gameObject.SetActive(false);
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
		if (!MainGame.mustEat)
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
}
