using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
	//UI to be used when game is over
	public StartGameUI UI;
	//Type of game (human vs AI / AI vs AI)
	public enum GameType { hvs, hvt, tvs }
	public GameType gameType;
	public bool gameOver = false;
	//Player turn
	public enum Turn { black, white }
	public Turn turn;
	public Text turnText;
	//Selected piece and cell
	public Cell selectedCell;
	public Piece selectedPiece;
	//The clone game to run simulations on
	private MainGame clone;
	private bool isClone = false;
	private bool AImoving = false;

	//Initialise the game state
	public void init(GameType type)
	{
		gameType = type;
		//Assign random first turn
		//turn = (int)Time.time % 2 == 0 ? Turn.black : Turn.white;
		turn = Turn.white;
		turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
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
	private List<Move> getAllValidMoves(Piece.Type type)
	{
		List<Move> validMoves = new List<Move>();
		Piece[] pieces = FindObjectsOfType<Piece>();
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in pieces)
		{
			if (p.transform.parent.parent.GetComponent<MainGame>() == gameObject.GetComponent<MainGame>())
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

	//Make the move
	public void makeMove(Piece sPiece = null, Cell sCell = null)
	{
		//For AI
		if (sPiece != null && sCell != null)
		{
			selectedCell = sCell;
			selectedPiece = sPiece;
		}

		bool ate = false;
		bool moved = false;

		List<Move> validMoves = new List<Move>();
		if (turn == Turn.black)
		{
			validMoves = getAllValidMoves(Piece.Type.black);
		}
		else if (turn == Turn.white)
		{
			validMoves = getAllValidMoves(Piece.Type.white);
		}
		Move currentMove = new Move(selectedPiece, selectedCell);
		Piece curPiece = selectedPiece;
		foreach (Move move in validMoves)
		{
			if (move.getCell() == selectedCell && move.getPiece() == selectedPiece)
			{
				selectedPiece.movePiece(selectedCell);
				//Check if king has been made
				if ((selectedCell.col == 8 && selectedPiece.type == Piece.Type.white) ||
					(selectedCell.col == 1 && selectedPiece.type == Piece.Type.black))
				{
					selectedPiece.makeKing();
				}
				moved = true;
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
					AImoving = false;
					return;
				}
			}
		}

		if (moved)
		{
			selectedCell = null;
			turn = turn == Turn.black ? Turn.white : Turn.black;
			turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
			checkWin();
			AImoving = false;
		}
		else
		{
			Debug.Log("Not in the list of valid moves");
			return;
		}
		//TODO: Make king if cell to move to is end row
	}

	IEnumerator startAI()
	{
		yield return new WaitForSeconds(1.5f);
		//AI only moves in main game
		AImoving = true;
		if ((gameType == GameType.hvs) && turn == Turn.black)
		{
			Move m = minimaxStart(this, 1);
			makeMove(m.getPiece(), m.getCell());
		}
	}

	//AI functions here
	void Update()
	{
		if (!gameOver && !AImoving && GameObject.Find("Board").GetComponent<MainGame>() == GetComponent<MainGame>())
		{
			StartCoroutine("startAI");
		}
	}

	private Move minimaxStart(MainGame board, int depth)
	{
		bool maxPlayer = true;
		double alpha = double.NegativeInfinity;
		double beta = double.PositiveInfinity;

		List<Move> possibleMoves = getAllValidMoves(Piece.Type.black);
		List<double> heuristics = new List<double>();

		MainGame clone = null;
		for (int i = 0; i < possibleMoves.Count; i++)
		{
			clone = board.setCloneBoard();
			clone.makeMove(possibleMoves[i].getPiece(), possibleMoves[i].getCell());
			heuristics.Add(minimax(clone, depth - 1, !maxPlayer, alpha, beta));
			Destroy(clone.gameObject);
		}

		double maxHeuristics = double.NegativeInfinity;

		for (int i = heuristics.Count - 1; i >= 0; i--)
		{
			if (heuristics[i] >= maxHeuristics)
			{
				maxHeuristics = heuristics[i];
			}
		}
		for (int i = 0; i < heuristics.Count; i++)
		{
			if (heuristics[i] < maxHeuristics)
			{
				heuristics.Remove(heuristics[i]);
				possibleMoves.Remove(possibleMoves[i]);
				i--;
			}
		}
		//check this return
		return possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
	}

	private double minimax(MainGame board, int depth, bool maxPlayer, double alpha, double beta)
	{
		if (depth == 0)
		{
			return getSimpleHeuristic(board, Piece.Type.black);
		}
		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.black);

		double initial = 0;
		MainGame clone = null;
		if (maxPlayer)
		{
			initial = double.NegativeInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				clone = board.setCloneBoard();
				clone.makeMove(possibleMoves[i].getPiece(), possibleMoves[i].getCell());

				double result = minimax(clone, depth - 1, !maxPlayer, alpha, beta);

				initial = System.Math.Max(result, initial);
				alpha = System.Math.Max(alpha, initial);

				Destroy(clone.gameObject);

				if (alpha >= beta)
					break;
			}
		}
		//minimizing
		else
		{
			initial = double.PositiveInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				clone = board.setCloneBoard();
				clone.makeMove(possibleMoves[i].getPiece(), possibleMoves[i].getCell());

				double result = minimax(clone, depth - 1, !maxPlayer, alpha, beta);

				initial = System.Math.Min(result, initial);
				alpha = System.Math.Min(alpha, initial);


				Destroy(clone.gameObject);
				if (alpha >= beta)
					break;
			}
		}

		return initial;
	}

	//Tactical AI
	void minimaxTctical()
	{

	}

	//Search AI
	//Move minimaxSearch(int depth)
	//{
	//	//Create clone for simulation
	//	double alpha = double.NegativeInfinity;
	//	double beta = double.PositiveInfinity;
	//	//Make descision tree
	//	//Root node
	//	MainGame clone = setCloneBoard();
	//	Tree route = new Tree(clone);
	//	Tree child = null;
	//	Tree previous = null;
	//	//For every move, need a new tree (node)
	//	List<Move> possbileMoves = clone.getAllValidMoves(Piece.Type.black);
	//	foreach (Move m in possbileMoves)
	//	{
	//		for (int i = 0; i < depth; i++)
	//		{
	//			List<Move> movesForCurrentClone = clone.getAllValidMoves();
	//			clone.makeMove(m.getPiece(), m.getCell());
	//			child = new Tree(clone);
	//			if (previous == null)
	//			{
	//				route.addChild(child);
	//				previous = child;
	//				child = null;
	//			}
	//			else
	//			{
	//				previous.addChild(child);
	//			}
	//			clone = clone.setCloneBoard();
	//		}
	//	}
	//	//search AI = black always tactical =  white when AI vs AI

	//	return new Move(null, null);
	//}

	//Create a clone board to run simulations on
	MainGame setCloneBoard()
	{
		if (clone != null)
		{
			Destroy(clone.gameObject);
		}
		clone = Instantiate(this);
		clone.transform.position = new Vector3(0, -13, 0);
		Piece[] clonePieces = FindObjectsOfType<Piece>();
		Cell[] cloneCells = FindObjectsOfType<Cell>();
		foreach (Piece cp in clonePieces)
		{
			if (cp.transform.parent.parent.name.Equals("Board(Clone)"))
			{
				cp.tag = "clone";
			}
		}
		foreach (Cell cc in cloneCells)
		{
			if (cc.transform.parent.parent.name.Equals("Board(Clone)"))
			{
				cc.tag = "clone";
			}
		}
		return clone;
	}

	//Return number of pieces left for a player
	int getSimpleHeuristic(MainGame board, Piece.Type type)
	{
		int numPieceForPlayer = 0;
		int numPieceForOpp = 0;
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in FindObjectsOfType<Piece>())
		{
			if (p.transform.parent.parent.GetComponent<MainGame>() == board.GetComponent<MainGame>())
			{
				boardPieces.Add(p);
			}
		}
		foreach (Piece p in boardPieces)
		{
			if (p.type == type && p.isActive)
			{
				numPieceForPlayer++;
			}
			else if (p.type != type && p.isActive)
			{
				numPieceForOpp++;
			}
		}
		return numPieceForPlayer - numPieceForOpp;
	}

	//Check if pieces are left
	void checkWin()
	{
		Piece[] pieces = FindObjectsOfType<Piece>();
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in pieces)
		{
			if (p.transform.parent.parent.GetComponent<MainGame>() == gameObject.GetComponent<MainGame>())
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
