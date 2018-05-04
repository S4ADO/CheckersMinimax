using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxTactical : MonoBehaviour
{
	public static int depth = 4, loopCount = 0;
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

		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.white);
		List<double> heuristics = new List<double>();

		if (board.firstMove)
		{
			Move m = new Move(GameObject.Find("white (11)").GetComponent<Piece>(),
				GameObject.Find("cell (27)").GetComponent<Cell>());
			return m;
		}
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
			int h = getTacticalHeuristic(board, Piece.Type.white);
			if (h == 0)
			{
				Destroy(board.gameObject);
			}
			return h;
		}
		Piece.Type type;

		if (maxPlayer == true)
		{
			type = Piece.Type.white;
		}
		else
		{
			type = Piece.Type.black;
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
		//Pawn = 5 + row number king = 10
		int evalFunc = 0;
		int evalFuncOpp = 0;
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
			//TactialAI
			if (p.type == type && p.isActive)
			{
				evalFunc += p.isKing ? 10 : 5;
				//The closer to being king the higher the evaluation 
				if (!p.isKing)
				{
					switch (p.cell.col)
					{
						case 6:
							evalFunc += 1;
							break;
						case 7:
							evalFunc += 2;
							break;
					}
					//Pieces on the edge have an advantage of not being able to be eaten
					evalFunc += p.cell.specialPosition == Cell.SpecialPosition.edge ? 1 : 0;
				}
			}
			//Opponent
			else if (p.type != type && p.isActive)
			{
				evalFuncOpp += p.isKing ? 10 : 5;
				//The closer to being king the higher the evaluation (black piece moves downwards)
				if (!p.isKing)
				{
					switch (p.cell.col)
					{
						case 3:
							evalFuncOpp += 1;
							break;
						case 2:
							evalFuncOpp += 2;
							break;
					}
					//Pieces on the edge have an advantage of not being able to be eaten
					evalFuncOpp += p.cell.specialPosition == Cell.SpecialPosition.edge ? 1 : 0;
				}
			}
		}
		board.thisHeurusic = evalFunc - evalFuncOpp;
		return evalFunc - evalFuncOpp;
	}
}
