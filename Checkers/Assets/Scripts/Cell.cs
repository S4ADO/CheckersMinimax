using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	//Declare cell properties
	public bool isTopOrBottomRow;
	public enum SpecialPosition {edge, corner}
	public SpecialPosition specialPosition;
	public Cell topLeft, topRight, bottomLeft, bottomRight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
