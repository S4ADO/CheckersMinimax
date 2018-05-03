using System.Collections.Generic;

public class Tree
{
	//Game state
	public MainGame node;
	public Move move;
	public int heuristicValue;
	public List<Tree> children;
	public enum Layer {max, min}
	public Layer layer;
	public Tree parent;

	//Constructor
	public Tree(MainGame node, Tree parent = null)
	{
		if (parent == null)
		{
			layer = Layer.max;
		}
		else if (parent.layer == Layer.max)
		{
			layer = Layer.min;
		}
		else
		{
			layer = Layer.max;
		}
		this.node = node;
		this.parent = parent;
	}
	
	//Adds a child tree to this one
	public void addChild(Tree child)
	{
		children.Add(child);
	}
}