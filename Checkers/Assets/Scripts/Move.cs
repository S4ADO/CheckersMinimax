public class Move
{
	private Piece piece;
	private Cell cell;
	private bool Jump;

	//Constructor
	public Move(Piece p, Cell c, bool jump = false)
	{
		piece = p;
		cell = c;
		jump = true;
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

	public bool getJump()
	{
		return Jump;
	}
}
