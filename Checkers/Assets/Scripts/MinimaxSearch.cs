using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxSearch : MonoBehaviour
{
	public static int depth = 2, loopCount = 0, loopCount1 = 0, loopCount2 = 0;
	public static Board board;

	//Simple search minimax with AB pruning
	/**
	 * TODO: personalise code
	 * */
	public static Move minimaxStart()
	{
		bool maxPlayer = true;
		double alpha = double.NegativeInfinity, beta = double.PositiveInfinity;

		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.black);
		List<double> evalFunction = new List<double>();

		Board clone = null;
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		for (int i = 0; i < possibleMoves.Count; i++)
		{
			System.Diagnostics.Stopwatch stopwatchin = new System.Diagnostics.Stopwatch();
			stopwatchin.Start();

			loopCount++;
			clone = board.setCloneBoard();
			bool moved = clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));
			//Debug.Log("moved 1 " + moved);
			evalFunction.Add(alphabeta(clone, depth - 1, !maxPlayer, alpha, beta));
			Destroy(clone.gameObject);

			stopwatchin.Stop();
			//Debug.Log("Time taken for individual loop:  " + (stopwatchin.Elapsed));
			stopwatchin.Reset();
		}
		stopwatch.Stop();
		//Debug.Log("Time taken for minimax loop:  " + (stopwatch.Elapsed));
		stopwatch.Reset();

		double maxEvalFunction = double.NegativeInfinity;
		for (int i = evalFunction.Count - 1; i >= 0; i--)
		{
			if (evalFunction[i] >= maxEvalFunction)
			{
				maxEvalFunction = evalFunction[i];
			}
		}
		for (int i = 0; i < evalFunction.Count; i++)
		{
			if (evalFunction[i] < maxEvalFunction)
			{
				evalFunction.Remove(evalFunction[i]);
				possibleMoves.Remove(possibleMoves[i]);
				i--;
			}
		}
		Debug.Log("Loop count 1: " + loopCount + " Loop count 2: " + loopCount1 + " Loop count 3: " + loopCount2);
		return possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
	}

	private static double alphabeta(Board board, int depth, bool maxPlayer, double alpha, double beta)
	{
		if (depth == 0)
		{
			int h = getSimpleEval(board, Piece.Type.black);
			if (h == 0)
			{
				Destroy(board.gameObject);
			}
			Debug.Log("NOTdefaultvalue");
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

		double value;
		Board clone = null;
		if (maxPlayer)
		{
			value = double.NegativeInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				loopCount1++;
				clone = board.setCloneBoard();
				bool moved = clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));
				//Debug.Log("moved 2 " + moved);

				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);

				value = System.Math.Max(eval, value);
				alpha = System.Math.Max(alpha, value);
				Destroy(clone.gameObject);

				if (beta <= alpha)
				{
					Debug.Log("breakhit");
					break;
				}
			}
		}
		//minimizing
		else
		{
			value = double.PositiveInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				loopCount2++;
				clone = board.setCloneBoard();
				bool moved = clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));
				//Debug.Log("moved 3 " + moved);

				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);

				value = System.Math.Min(eval, value);
				alpha = System.Math.Min(alpha, value);

				Destroy(clone.gameObject);
				if (beta <= alpha)
				{
					Debug.Log("breakhit2");
					break;
				}
			}
		}
		Debug.Log("defaultvalue");
		return value;
	}

	//Return number of pieces left for a player
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
