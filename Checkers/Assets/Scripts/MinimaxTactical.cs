using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxTactical : MonoBehaviour
{
	public static int depth, loopCount = 0;
	public static Board board;

	//Simple search minimax with AB pruning
	/**
	 * TODO: personalise code
	 * */
	public static Move minimaxStart()
	{
		bool maxPlayer = true;
		double alpha = double.NegativeInfinity;
		double beta = double.PositiveInfinity;

		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.black);
		List<double> heuristics = new List<double>();

		Board clone = null;
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		for (int i = 0; i < possibleMoves.Count; i++)
		{
			System.Diagnostics.Stopwatch stopwatchin = new System.Diagnostics.Stopwatch();
			stopwatchin.Start();

			loopCount++;
			clone = board.setCloneBoard();
			clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));
			heuristics.Add(minimax(clone, depth - 1, !maxPlayer, alpha, beta));
			Destroy(clone.gameObject);

			stopwatchin.Stop();
			Debug.Log("Time taken for individual loop:  " + (stopwatchin.Elapsed));
			stopwatchin.Reset();
		}
		stopwatch.Stop();
		Debug.Log("Time taken for minimax loop:  " + (stopwatch.Elapsed));
		stopwatch.Reset();

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
		Debug.Log("Loop count: " + loopCount);
		return possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
	}

	private static double minimax(Board board, int depth, bool maxPlayer, double alpha, double beta)
	{
		if (depth == 0)
		{
			int h = getTacticalHeuristic(board, Piece.Type.black);
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

		double initial = 0;
		Board clone = null;
		if (maxPlayer)
		{
			initial = double.NegativeInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				loopCount++;
				clone = board.setCloneBoard();
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));

				double result = minimax(clone, depth - 1, !(maxPlayer), alpha, beta);

				initial = System.Math.Max(result, initial);
				alpha = System.Math.Max(alpha, initial);
				Destroy(clone.gameObject);

				if (alpha >= beta)
				{
					break;
				}
			}
		}
		//minimizing
		else
		{
			initial = double.PositiveInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				loopCount++;
				clone = board.setCloneBoard();
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));

				double result = minimax(clone, depth - 1, !(maxPlayer), alpha, beta);

				initial = System.Math.Min(result, initial);
				alpha = System.Math.Min(alpha, initial);

				Destroy(clone.gameObject);
				if (alpha >= beta)
				{
					break;
				}
			}
		}
		return initial;
	}

	//Return number of pieces left for a player
	private static int getTacticalHeuristic(Board board, Piece.Type type)
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
