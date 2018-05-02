using System.Collections;
using System.Collections.Generic;
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

	//Init
	void Start ()
	{
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
		
	}
}
