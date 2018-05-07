/**
 * This class describes the properties of the main board and
 * all clones that result from the main board
 * Author: Saad Musejee
 * */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	//For clones keep log off board heuristic
	public int thisHeurusic = 0;
	//Keep log off moves made
	public string movesMade = "";
	//Type of game (human vs AI / AI vs AI)
	public enum GameType { hvs, hvt, tvs }
	public GameType gameType;
	public bool gameOver = false;
	public bool firstMove = true;
	//Player turn
	public enum Turn { black, white }
	public Turn turn;
	public Text turnText;
	//Selected piece and cell
	public Cell selectedCell;
	public Piece selectedPiece;
	//The clone game to run simulations on
	public Board clone;
	public MainGame theMainGame;

	//Initialise the game state
	public void init(GameType type)
	{
		theMainGame = GameObject.Find("MainGame").GetComponent<MainGame>();
		gameType = type;
		//Assign random first turn
		//turn = (int)Time.time % 2 == 0 ? Turn.black : Turn.white;
		turn = Turn.white;
		turnText.text = turn == Turn.white ? "Turn: White" : "Turn: Black";
		//Assign cells their adjacents
		Cell[] cells = FindObjectsOfType<Cell>();
		foreach (Cell cell in cells)
		{
			//This cell's IDs
			int cellRow = cell.row;
			int cellCol = cell.col;
			//Adjacent IDs
			int tlR, tlC, trR, trC, blR, blC, brR, brC;
			tlR = cellRow - 1;
			tlC = cellCol + 1;
			trR = cellRow + 1;
			trC = cellCol + 1;
			blR = cellRow - 1;
			blC = cellCol - 1;
			brR = cellRow + 1;
			brC = cellCol - 1;
			//Check if a cell with these IDs exists
			foreach (Cell cellCheck in cells)
			{
				//Assign top left
				if (cellCheck.row == tlR && cellCheck.col == tlC)
				{
					cell.topLeft = cellCheck;
				}
				//Assign top right
				else if (cellCheck.row == trR && cellCheck.col == trC)
				{
					cell.topRight = cellCheck;
				}
				//Assign bottom left
				else if (cellCheck.row == blR && cellCheck.col == blC)
				{
					cell.bottomLeft = cellCheck;
				}
				//Assign bottom right
				else if (cellCheck.row == brR && cellCheck.col == brC)
				{
					cell.bottomRight = cellCheck;
				}
			}
		}
	}

	//Get all valid moves for a given piece
	public List<Move> getAllValidMoves(Piece.Type type)
	{
		List<Move> validMoves = new List<Move>();
		Piece[] pieces = FindObjectsOfType<Piece>();
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in pieces)
		{
			if (p.cell.mainBoard == GetComponent<Board>())
			{
				boardPieces.Add(p);
			}
		}
		bool canEat = false;
		foreach (Piece piece in boardPieces)
		{
			//Right colour
			if (piece.isActive && piece.type == type)
			{
				//Moving down
				if (piece.type == Piece.Type.black && !piece.isKing)
				{
					//Check if cell is occupied BL
					if (piece.cell.bottomLeft != null)
					{
						if (!(piece.cell.bottomLeft.piece == null))
						{
							if (piece.cell.bottomLeft.bottomLeft != null && piece.cell.bottomLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.bottomLeft.bottomLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomLeft.bottomLeft, piece.cell.bottomLeft.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.bottomLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied BR
					if (piece.cell.bottomRight != null)
					{
						if (!(piece.cell.bottomRight.piece == null))
						{
							if (piece.cell.bottomRight.bottomRight != null && piece.cell.bottomRight.piece.type != piece.type)
							{
								if (piece.cell.bottomRight.bottomRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomRight.bottomRight, piece.cell.bottomRight.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.bottomRight);
							validMoves.Add(move2);
						}
					}
				}
				//Moving up
				else if (piece.type == Piece.Type.white && !piece.isKing)
				{
					if (piece.cell.topLeft != null)
					{
						if (!(piece.cell.topLeft.piece == null))
						{
							if (piece.cell.topLeft.topLeft != null && piece.cell.topLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.topLeft.topLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.topLeft.topLeft, piece.cell.topLeft.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.topLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied TR
					if (piece.cell.topRight != null)
					{
						if (!(piece.cell.topRight.piece == null))
						{
							if (piece.cell.topRight.topRight != null && piece.cell.topRight.piece.type != piece.type)
							{
								if (piece.cell.topRight.topRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.topRight.topRight, piece.cell.topRight.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.topRight);
							validMoves.Add(move2);
						}
					}
				}
				//King movement
				else if (piece.isKing)
				{
					if (piece.cell.topLeft != null)
					{
						if (!(piece.cell.topLeft.piece == null))
						{
							if (piece.cell.topLeft.topLeft != null && piece.cell.topLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.topLeft.topLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.topLeft.topLeft, piece.cell.topLeft.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.topLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied TR
					if (piece.cell.topRight != null)
					{
						if (!(piece.cell.topRight.piece == null))
						{
							if (piece.cell.topRight.topRight != null && piece.cell.topRight.piece.type != piece.type)
							{
								if (piece.cell.topRight.topRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.topRight.topRight, piece.cell.topRight.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.topRight);
							validMoves.Add(move2);
						}
					}
					//Check if cell is occupied BL
					if (piece.cell.bottomLeft != null)
					{
						if (!(piece.cell.bottomLeft.piece == null))
						{
							if (piece.cell.bottomLeft.bottomLeft != null && piece.cell.bottomLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.bottomLeft.bottomLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomLeft.bottomLeft, piece.cell.bottomLeft.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.bottomLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied BR
					if (piece.cell.bottomRight != null)
					{
						if (!(piece.cell.bottomRight.piece == null))
						{
							if (piece.cell.bottomRight.bottomRight != null && piece.cell.bottomRight.piece.type != piece.type)
							{
								if (piece.cell.bottomRight.bottomRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomRight.bottomRight, piece.cell.bottomRight.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.bottomRight);
							validMoves.Add(move2);
						}
					}
				}
			}
		}
		//Remove all other moves if can eat
		if (canEat)
		{
			validMoves.RemoveAll(m => m.getJumped() == null);
		}
		return validMoves;
	}

	//Get reference to equivilant piece on the clone board
	public Piece findEquivilantPiece(Piece piece)
	{
		Piece toRet = null;
		Piece[] pieces = FindObjectsOfType<Piece>();
		foreach (Piece p in pieces)
		{
			if (p.cell.mainBoard == GetComponent<Board>())
			{
				if (p.cell.row == piece.cell.row &&
					p.cell.col == piece.cell.col &&
					p.type == piece.type && p.isKing == piece.isKing)
				{
					toRet = p;
					break;
				}
			}
		}
		return toRet;
	}

	//Get reference to equivilant cell on the clone board
	public Cell findEquivilantCell(Cell cell)
	{
		Cell toRet = null;
		Cell[] cells = FindObjectsOfType<Cell>();
		foreach (Cell c in cells)
		{
			if (c.mainBoard == GetComponent<Board>())
			{
				if (c.row == cell.row && c.col == cell.col)
				{
					toRet = c;
					break;
				}
			}
		}
		return toRet;
	}

	//Make the move
	public bool makeMove(Piece sPiece = null, Cell sCell = null, Piece jumped = null)
	{
		bool isAI = false;
		//For AI
		if (sPiece != null && sCell != null)
		{
			isAI = true;
			selectedCell = sCell;
			selectedPiece = sPiece;
		}

		bool ate = false;
		bool moved = false;

		if (!isAI)
		{
			List<Move> validMoves = null;
			validMoves = new List<Move>();
			if (turn == Turn.black)
			{
				validMoves = getAllValidMoves(Piece.Type.black);
			}
			else if (turn == Turn.white)
			{
				validMoves = getAllValidMoves(Piece.Type.white);
			}

			Piece curPiece = selectedPiece;
			foreach (Move move in validMoves)
			{
				if (move.getCell() == (selectedCell) && move.getPiece() == (selectedPiece))
				{
					movesMade = movesMade + selectedPiece.name + selectedCell.name + ";";
					selectedPiece.movePiece(selectedCell);

					//Check if king has been made
					if ((selectedCell.col == 8 && selectedPiece.type == Piece.Type.white) ||
						(selectedCell.col == 1 && selectedPiece.type == Piece.Type.black))
					{
						selectedPiece.makeKing();
					}
					moved = true;
					firstMove = false;
					//Remove jumped over piece
					if (move.getJumped() != null)
					{
						move.getJumped().remove();
						ate = true;
					}
				}
			}

			if (ate)
			{
				if (turn == Turn.black)
				{
					validMoves = getAllValidMoves(Piece.Type.black);
				}
				else if (turn == Turn.white)
				{
					validMoves = getAllValidMoves(Piece.Type.white);
				}
				Move newMove = new Move(curPiece, null);
				foreach (Move move in validMoves)
				{
					if (newMove.getPiece() == move.getPiece() && move.getJumped() != null)
					{
						selectedCell = null;
						return true;
					}
				}
			}
		}
		//Is AI
		else
		{
			selectedPiece.movePiece(selectedCell);
			//Check if king has been made
			if ((selectedCell.col == 8 && selectedPiece.type == Piece.Type.white) ||
				(selectedCell.col == 1 && selectedPiece.type == Piece.Type.black))
			{
				selectedPiece.makeKing();
			}
			moved = true;
			firstMove = false;
			//Remove jumped over piece
			if (jumped != null)
			{
				Debug.Log("hit");
				jumped.remove();
				ate = true;
			}

			if (ate)
			{
				List<Move> validMoves = null;
				if (turn == Turn.black)
				{
					validMoves = getAllValidMoves(Piece.Type.black);
				}
				else if (turn == Turn.white)
				{
					validMoves = getAllValidMoves(Piece.Type.white);
				}
				Move newMove = new Move(selectedPiece, null);
				foreach (Move move in validMoves)
				{
					if (newMove.getPiece() == move.getPiece() && move.getJumped() != null)
					{
						selectedCell = null;
						return true;
					}
				}
			}
		}

		if (moved)
		{
			selectedCell = null;
			turn = turn == Turn.black ? Turn.white : Turn.black;
			turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
			return true;
		}
		else
		{
			Debug.Log("Invalid move of piece " + selectedPiece.name + " to: " + selectedCell.name);
			return false;
		}
	}

	//Create a clone board to run simulations on
	public Board setCloneBoard()
	{
		//Debug
		if (clone != null)
		{
			float posY = clone.transform.position.y - 13;
			if (clone.thisHeurusic == 0)
			{
				Destroy(clone.gameObject);
			}
			clone = Instantiate(this);
			clone.transform.position = new Vector3(0, posY, 0);
		}
		else
		{
			clone = Instantiate(this);
			clone.transform.position = new Vector3(0, -13, 0);
		}
		return clone;
	}
}