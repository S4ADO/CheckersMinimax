using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxTactical : MonoBehaviour
{
	public static int depth = 2, loopCount = 0;
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
		List<double> evalFunction = new List<double>();

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
		Debug.Log("Loop count: " + loopCount);
		return possibleMoves[Random.Range(0, possibleMoves.Count - 1)];
	}

	private static double alphabeta(Board board, int depth, bool maxPlayer, double alpha, double beta)
	{
		if (depth == 0)
		{
			int h = getTacticalEval(board, Piece.Type.white);
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

		double value = 0;
		Board clone = null;
		if (maxPlayer)
		{
			value = double.NegativeInfinity;
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				loopCount++;
				clone = board.setCloneBoard();
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));

				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);

				value = System.Math.Max(eval, value);
				alpha = System.Math.Max(alpha, value);

				if (alpha >= beta)
				{
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
				loopCount++;
				clone = board.setCloneBoard();
				clone.makeMove(clone.findEquivilantPiece(possibleMoves[i].getPiece()),
				clone.findEquivilantCell(possibleMoves[i].getCell()));

				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);

				value = System.Math.Min(eval, value);
				alpha = System.Math.Min(alpha, value);

				if (alpha >= beta)
				{
					break;
				}
			}
		}
		return value;
	}

	//Return number of pieces left for a player
	private static int getTacticalEval(Board board, Piece.Type type)
	{
		int evalFunc = 0;
		int evalFuncOpp = 0;
		int numKings = 0, numKingsOpp = 0, numNormal = 0, numNormalOpp = 0;

		List<Piece> boardPieces = new List<Piece>();
		foreach (Piece p in FindObjectsOfType<Piece>())
		{
			if (p.cell.mainBoard == board)
			{
				boardPieces.Add(p);
			}
		}
		//Get amount each side has
		foreach (Piece p in boardPieces)
		{
			if (p.isActive)
			{
				if (p.type == type && p.isKing)
				{
					numKings++;
				}
				else if (p.type == type && !p.isKing)
				{
					numNormal++;
				}
				else if (p.type != type && p.isKing)
				{
					numKingsOpp++;
				}
				else if (p.type != type && !p.isKing)
				{
					numNormalOpp++;
				}
			}
		}
		int numTotal = numNormal + numKings;
		int numTotalOpp = numNormalOpp + numKingsOpp;

		foreach (Piece p in boardPieces)
		{
			//TactialAI
			if (p.type == type && p.isActive)
			{
				//Nearing end game 
				if ((numTotal) > (numTotalOpp + 3) && numKings >= 2)
				{
					evalFunc += p.isKing ? 2 : 1;
				}
				//Normal evaluation
				else
				{
					evalFunc += p.isKing ? 2 : 1;
					//The closer to being king the higher the evaluation
					if (!p.isKing)
					{
						switch (p.cell.col)
						{
							case 5:
								evalFunc += 1;
								break;
							case 6:
								evalFunc += 2;
								break;
							case 7:
								evalFunc += 3;
								break;
						}
						//Pieces on the edge have an advantage of not being able to be eaten
						evalFunc += p.cell.specialPosition == Cell.SpecialPosition.edge ? 1 : 0;
					}
				}
			}
			//Opponent
			else if (p.type != type && p.isActive)
			{
				//Nearing end game 
				if ((numTotalOpp) > (numTotal + 3) && numKingsOpp >= 2)
				{
					evalFuncOpp += p.isKing ? 2 : 1;
				}
				else
				{
					evalFuncOpp += p.isKing ? 2 : 1;
					//The closer to being king the higher the evaluation (black piece moves downwards)
					if (!p.isKing)
					{
						switch (p.cell.col)
						{
							case 4:
								evalFuncOpp += 1;
								break;
							case 3:
								evalFuncOpp += 2;
								break;
							case 2:
								evalFuncOpp += 3;
								break;
						}
						//Pieces on the edge have an advantage of not being able to be eaten
						evalFuncOpp += p.cell.specialPosition == Cell.SpecialPosition.edge ? 1 : 0;
					}
				}
			}
		}
		board.thisHeurusic = evalFunc - evalFuncOpp;
		return evalFunc - evalFuncOpp;
	}
}
