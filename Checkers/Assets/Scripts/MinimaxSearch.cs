/**
 * This class implements the minimax algorithm with alpha-beta pruning
 * it uses a simple version of the heursitic evaluation function
 * Author: Saad Musejee
 * */
using System.Collections.Generic;
using UnityEngine;

public class MinimaxSearch : MonoBehaviour
{
	public static int depth = 3;
	public static Board board;

	//Simple minimax with AB pruning
	public static Move minimaxStart()
	{
		bool maxPlayer = true;
		double alpha = double.NegativeInfinity, beta = double.PositiveInfinity;

		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.black);
		List<double> evalFunction = new List<double>();

		//Clone the board at each stage of the loop and make the initiatl possible move
		Board clone = null;
		for (int i = 0; i < possibleMoves.Count; i++)
		{
			clone = board.setCloneBoard();
			if (possibleMoves[i].getJumped() != null)
			{
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
					clone.findEquivilantCell(possibleMoves[i].getCell()), clone.findEquivilantPiece(possibleMoves[i].getJumped()));
			}
			else
			{
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
					clone.findEquivilantCell(possibleMoves[i].getCell()));
			}
			evalFunction.Add(alphabeta(clone, depth - 1, !maxPlayer, alpha, beta));
			Destroy(clone.gameObject);
		}

		//Get the maximum evaluation
		double maxEvalFunction = double.NegativeInfinity;
		for (int i = evalFunction.Count - 1; i >= 0; i--)
		{
			if (evalFunction[i] >= maxEvalFunction)
			{
				maxEvalFunction = evalFunction[i];
			}
		}
		//Remove all evaluations that are less than the maximum
		for (int i = 0; i < evalFunction.Count; i++)
		{
			if (evalFunction[i] < maxEvalFunction)
			{
				evalFunction.Remove(evalFunction[i]);
				possibleMoves.Remove(possibleMoves[i]);
				i--;
			}
		}
		//Return a random move with the max evaluation
		return possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
	}

	//Recursive algorithm to search child board states for the specified depth
	private static double alphabeta(Board board, int depth, bool maxPlayer, double alpha, double beta)
	{
		if (depth == 0)
		{
			int h = getSimpleEval(board, Piece.Type.black);
			if (h == 0)
			{
				Destroy(board.gameObject);
			}
			return h;
		}
		Piece.Type type;

		if (maxPlayer == true)
		{
			type = Piece.Type.black;
		}
		else
		{
			type = Piece.Type.white;
		}

		List<Move> possibleMoves = board.getAllValidMoves(type);

		double result;
		Board clone = null;
		//Recursively find the child depth and all resultant depths and return its evaluation with AB pruning
		if (maxPlayer)
		{
			result = alpha;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				clone = board.setCloneBoard();
				if (possibleMoves[i].getJumped() != null)
				{
					clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
						clone.findEquivilantCell(possibleMoves[i].getCell()), clone.findEquivilantPiece(possibleMoves[i].getJumped()));
				}
				else
				{
					clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
						clone.findEquivilantCell(possibleMoves[i].getCell()));
				}
				//Debug.Log("moved 2 " + moved);
				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);
				result = System.Math.Max(eval, result);
				alpha = System.Math.Max(alpha, result);
				if (alpha >= beta)
				{
					Debug.Log("breakhit");
					break;
				}
			}
			return result;
		}
		//minimizing
		else
		{
			result = beta;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				clone = board.setCloneBoard();
				if (possibleMoves[i].getJumped() != null)
				{
					clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
						clone.findEquivilantCell(possibleMoves[i].getCell()), clone.findEquivilantPiece(possibleMoves[i].getJumped()));
				}
				else
				{
					clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
						clone.findEquivilantCell(possibleMoves[i].getCell()));
				};
				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);
				result = System.Math.Min(eval, result);
				beta = System.Math.Min(beta, result);
				if (alpha >= beta)
				{
					Debug.Log("breakhit2");
					break;
				}
			}
			return result;
		}
	}

	//Retursn number of pieces left for a player
	private static int getSimpleEval(Board board, Piece.Type type)
	{
		int numPieceForPlayer = 0;
		int numPieceForOpp = 0;
		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in FindObjectsOfType<Piece>())
		{
			if (p.cell.mainBoard == board)
			{
				boardPieces.Add(p);
			}
		}
		foreach (Piece p in boardPieces)
		{
			if (p.type == type && p.isActive)
			{
				numPieceForPlayer++;
				if (p.isKing)
				{
					numPieceForPlayer++;
				}
			}
			else if (p.type != type && p.isActive)
			{
				numPieceForOpp++;
				if (p.isKing)
				{
					numPieceForOpp++;
				}
			}
		}
		board.thisHeurusic = numPieceForPlayer - numPieceForOpp;
		return numPieceForPlayer - numPieceForOpp;
	}
}
