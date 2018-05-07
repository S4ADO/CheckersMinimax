/**
 * This class describes the move made at each turn. It contains
 * a reference to the moving piece, where it is moving to and
 * if the move means any of the opponent's pieces have been jumped over
 * Author: Saad Musejee
 * */
public class Move
{
	private Piece piece;
	private Cell cell;
	private Piece jumped;

	//Constructor
	public Move(Piece p, Cell c, Piece jumpPiece = null)
	{
		piece = p;
		cell = c;
		jumped = jumpPiece;
	}

	//Getters
	public Piece getPiece()
	{
		return piece;
	}

	public Cell getCell()
	{
		return cell;
	}

	public Piece getJumped()
	{
		return jumped;
	}
}
