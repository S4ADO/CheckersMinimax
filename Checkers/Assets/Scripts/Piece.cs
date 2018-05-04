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
	public MainGame mainGame;
	public LayerMask touchInputMask = 2;

	//Init
	void Start()
	{
		transform.position = cell.transform.position + new Vector3(0.001f, 0.001f, 0);
	}

	public void movePiece(Cell newCell)
	{
		cell.piece = null;
		cell = newCell;
		cell.piece = this;
		transform.position = cell.transform.position + new Vector3(0.001f, 0.001f, 0);
	}

	void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),
			Vector2.zero, touchInputMask);
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.transform.gameObject == gameObject)
				{
					OnMouseDown();
				}
			}
		}
	}

	public void remove()
	{
		transform.position = new Vector3(20, 20, 0);
		cell.piece = null;
		isActive = false;
		cell = null;
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
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
