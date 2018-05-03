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
