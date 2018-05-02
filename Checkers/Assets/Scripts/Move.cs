public class Move
{
	private Piece piece;
	private Cell cell;

	//Constructor
	public Move(Piece p, Cell c)
	{
		piece = p;
		cell = c;
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
}
