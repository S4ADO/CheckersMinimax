﻿/**
 * This class describes the properties of each piece on a given board
 * where each board starts with 24 pieces
 * Author: Saad Musejee
 * */
using UnityEngine;

public class Piece : MonoBehaviour
{
	//Declare piece properties
	public enum Type { black, white }
	public Type type;
	public Cell cell;
	public bool isKing = false;
	public Sprite kingSprite;
	public bool isActive = true;
	public Board mainBoard;
	public LayerMask touchInputMask = 2;

	//Init
	void Start()
	{
		transform.position = cell.transform.position + new Vector3(0.001f, 0.001f, 0);
	}

	//Move to new cell
	public void movePiece(Cell newCell)
	{
		cell.piece = null;
		cell = newCell;
		cell.piece = this;
		transform.position = cell.transform.position + new Vector3(0.001f, 0.001f, 0);
	}

	//For humans moving
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

	//Remove piece from board
	public void remove()
	{
		transform.position = new Vector3(20, 20, 0);
		cell.piece = null;
		isActive = false;
		cell = null;
		gameObject.SetActive(false);
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
			if (mainBoard.turn == Board.Turn.black)
			{
				mainBoard.selectedPiece = this;
			}
		}
		else if (type == Type.white)
		{
			if (mainBoard.turn == Board.Turn.white)
			{
				mainBoard.selectedPiece = this;
			}
		}
	}
}
