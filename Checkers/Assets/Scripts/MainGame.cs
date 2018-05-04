﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
	//UI to be used when game is over
	public StartGameUI UI;
	public bool gameOver = false;
	//Reference to main board
	public Board mainBoard;

	//Initialise the game state
	public void init(Board.GameType type)
	{
		mainBoard.init(type);
	}

	//AI functions here
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.M))
		{
			startAI();
		}
	}

	public void getHumanMove()
	{
		InputField IF = GameObject.Find("InputMove").GetComponent<InputField>();
		string move = IF.text;
		string[] moveArr = move.Split(',');
		Piece[] pieces = FindObjectsOfType<Piece>();
		Piece piece = null;
		foreach (Piece p in pieces)
		{
			if (p.cell.mainBoard == mainBoard)
			{
				if (p.cell.row == int.Parse(moveArr[0]) && p.cell.col == int.Parse(moveArr[1]))
				{
					piece = p;
					Debug.Log(p);
				}
			}
		}
		Cell[] Cells = FindObjectsOfType<Cell>();
		Cell cell = null;
		foreach (Cell c in Cells)
		{
			if (c.mainBoard == mainBoard)
			{
				if (c.row == int.Parse(moveArr[2]) && c.col == int.Parse(moveArr[3]))
				{
					cell = c;
					Debug.Log(c);
				}
			}
		}
		mainBoard.makeMove(piece, cell);
	}

	void startAI()
	{
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		MinimaxSearch.depth = 2;
		MinimaxSearch.board = mainBoard;
		Move m = MinimaxSearch.minimaxStart();
		stopwatch.Stop();
		Debug.Log("Time taken overall:  " + (stopwatch.Elapsed));
		stopwatch.Reset();
		mainBoard.makeMove(m.getPiece(), m.getCell());
		Board[] mg = FindObjectsOfType<Board>();
		foreach (Board mgg in mg)
		{
			if (mgg.thisHeurusic == 0 && mgg != mainBoard)
			{
				Destroy(mgg.gameObject);
			}
		}
	}

	//Check if pieces are left
	void checkWin()
	{
		Piece[] pieces = FindObjectsOfType<Piece>();
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in pieces)
		{
			if (p.cell.mainBoard == mainBoard)
			{
				boardPieces.Add(p);
			}
		}
		bool blackExists = false;
		bool whiteExists = false;
		//Check black 
		foreach (Piece piece in boardPieces)
		{
			if (piece.isActive && piece.type == Piece.Type.black)
			{
				blackExists = true;
			}
		}
		//Check white
		foreach (Piece piece in boardPieces)
		{
			if (piece.isActive && piece.type == Piece.Type.white)
			{
				whiteExists = true;
			}
		}
		if (!whiteExists)
		{
			Debug.Log("Black wins");
			GameOver();
		}
		if (!blackExists)
		{
			Debug.Log("White wins");
			GameOver();
		}
	}

	void GameOver()
	{
		gameOver = true;
	}
}