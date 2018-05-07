using System.Collections.Generic;
using UnityEngine;

public class MinimaxTactical : MonoBehaviour
{
	public static int depth = 2;
	public static Board board;

	//Simple search minimax with AB pruning
	/**
	 * TODO: personalise code
	 * */
	public static Move minimaxStart()
	{
		bool maxPlayer = true;
		double alpha = double.NegativeInfinity, beta = double.PositiveInfinity;

		List<Move> possibleMoves = board.getAllValidMoves(Piece.Type.white);
		List<double> evalFunction = new List<double>();

		if (board.firstMove)
		{
			Move m = new Move(GameObject.Find("white (11)").GetComponent<Piece>(),
				GameObject.Find("cell (27)").GetComponent<Cell>());
			return m;
		}
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
		return possibleMoves[0];
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

		double result;
		Board clone = null;
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
				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);
				result = System.Math.Max(eval, result);
				alpha = System.Math.Max(alpha, result);
				if (alpha >= beta)
				{
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
				}
				double eval = alphabeta(clone, depth - 1, !(maxPlayer), alpha, beta);
				Destroy(clone.gameObject);
				result = System.Math.Min(eval, result);
				beta = System.Math.Min(beta, result);
				if (alpha >= beta)
				{
					break;
				}
			}
			return result;
		}
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
				if ((numNormal == 0 && numNormalOpp == 0))
				{
					foreach (Piece opp in boardPieces)
					{
						//Find first opponent king piece and reduce distance from them
						if (opp.type != type)
						#region eval
						{
							int rowDist = Mathf.Abs(opp.cell.row - p.cell.row);
							int colDist = Mathf.Abs(opp.cell.col - p.cell.col);
							int totalDist = colDist + rowDist;
							if (totalDist == 18)
							{
								evalFunc += 1;
							}
							else if (totalDist == 17)
							{
								evalFunc += 2;
							}
							else if (totalDist == 16)
							{
								evalFunc += 3;
							}
							else if (totalDist == 15)
							{
								evalFunc += 4;
							}
							else if (totalDist == 14)
							{
								evalFunc += 5;
							}
							else if (totalDist == 13)
							{
								evalFunc += 6;
							}
							else if (totalDist == 12)
							{
								evalFunc += 7;
							}
							else if (totalDist == 11)
							{
								evalFunc += 8;
							}
							else if (totalDist == 10)
							{
								evalFunc += 9;
							}
							else if (totalDist == 9)
							{
								evalFunc += 10;
							}
							else if (totalDist == 8)
							{
								evalFunc += 11;
							}
							else if (totalDist == 7)
							{
								evalFunc += 12;
							}
							else if (totalDist == 6)
							{
								evalFunc += 13;
							}
							else if (totalDist == 5)
							{
								evalFunc += 14;
							}
							else if (totalDist == 4)
							{
								evalFunc += 15;
							}
							else if (totalDist == 3)
							{
								evalFunc +=16;
							}
							else if (totalDist == 2)
							{
								evalFunc += 17;
							}
							else if (totalDist == 1)
							{
								evalFunc += 18;
							}
							break;
						}
						#endregion
					}
				}
				//Normal evaluation
				else
				{
					evalFunc += p.isKing ? 5 : 4;
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
				if ((numNormal == 0 && numNormalOpp == 0))
				{
					foreach (Piece opp in boardPieces)
					{
						//Find first opponent king piece and reduce distance from them
						if (opp.type == type)
						{
							#region eval opp
							int rowDist = Mathf.Abs(opp.cell.row - p.cell.row);
							int colDist = Mathf.Abs(opp.cell.col - p.cell.col);
							int totalDist = colDist + rowDist;
							if (totalDist == 18)
							{
								evalFuncOpp += 1;
							}
							else if (totalDist == 17)
							{
								evalFuncOpp += 2;
							}
							else if (totalDist == 16)
							{
								evalFuncOpp += 3;
							}
							else if (totalDist == 15)
							{
								evalFuncOpp += 4;
							}
							else if (totalDist == 14)
							{
								evalFuncOpp += 5;
							}
							else if (totalDist == 13)
							{
								evalFuncOpp += 6;
							}
							else if (totalDist == 12)
							{
								evalFuncOpp += 7;
							}
							else if (totalDist == 11)
							{
								evalFuncOpp += 8;
							}
							else if (totalDist == 10)
							{
								evalFuncOpp += 9;
							}
							else if (totalDist == 9)
							{
								evalFuncOpp += 10;
							}
							else if (totalDist == 8)
							{
								evalFuncOpp += 11;
							}
							else if (totalDist == 7)
							{
								evalFuncOpp += 12;
							}
							else if (totalDist == 6)
							{
								evalFuncOpp += 13;
							}
							else if (totalDist == 5)
							{
								evalFuncOpp += 14;
							}
							else if (totalDist == 4)
							{
								evalFuncOpp += 15;
							}
							else if (totalDist == 3)
							{
								evalFuncOpp += 16;
							}
							else if (totalDist == 2)
							{
								evalFuncOpp += 17;
							}
							else if (totalDist == 1)
							{
								evalFuncOpp += 18;
							}
							break;
							#endregion
						}
					}
				}
				else
				{
					evalFuncOpp += p.isKing ? 5 : 4;
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
