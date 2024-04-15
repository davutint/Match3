using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GemType { earth, jupiter, mars, neptune, uranus, venus,bomb, stone,planet1,planet2,planet3,planet4,planet5,planet6}
[RequireComponent(typeof(BoxCollider2D))]
public class Gem : MonoBehaviour
{
   //[HideInInspector]
	public Vector2Int PosIndex;
	//[HideInInspector]
	public Board board;


	private Vector2 _firstTouchPosition;
	private Vector2 _finalTouchPosition;

	private bool _mousePressed;
	private float _swipeAngle = 0;

	private Gem _otherGem;

	
	public GemType Type;

	public bool IsMatched;
	
	private Vector2Int _previousPos;

	public GameObject DestroyEffect;

	public int BlastSize = 2;

	public int ScoreValue = 10;

   
	void Update()
	{
		if (Vector2.Distance(transform.position, PosIndex) > .01f)
		{
			transform.position = Vector2.Lerp(transform.position, PosIndex, board.GemSpeed * Time.deltaTime);
		} 
		else
		{
			transform.position = new Vector3(PosIndex.x, PosIndex.y, 0f);
			board.AllGems[PosIndex.x, PosIndex.y] = this;
		}

		if(_mousePressed && Input.GetMouseButtonUp(0))
		{
			_mousePressed = false;

			if(board.CurrentState == BoardState.move && board.RoundMan.RoundTime > 0)
			{
				_finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				CalculateAngle();
			}
		}
	}

	public void SetupGem(Vector2Int pos, Board theBoard)
	{
		PosIndex = pos;
		board = theBoard;
	}

	
	private void OnMouseDown()
	{
		if(board.CurrentState == BoardState.move && board.RoundMan.RoundTime > 0)//Board.BoardState.move olarak değiştirirsin eğer çalışmazsa
	  	{
			_firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePressed = true;
	  	}
	}

	private void CalculateAngle()
	{
	  	_swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x);
	 	_swipeAngle = _swipeAngle * 180 / Mathf.PI;
	  	Debug.Log(_swipeAngle);

	  	if(Vector3.Distance(_firstTouchPosition, _finalTouchPosition) > .5f)
   		{
			MovePieces();
	 	}
	}

	private void MovePieces()
	{
		_previousPos = PosIndex;

		if(_swipeAngle < 45 && _swipeAngle > -45 && PosIndex.x < board.Width - 1)
		{
			_otherGem = board.AllGems[PosIndex.x + 1, PosIndex.y];
			_otherGem.PosIndex.x--;
			PosIndex.x++;
		}
		else if(_swipeAngle > 45 && _swipeAngle <= 135 && PosIndex.y < board.Height - 1)
		{
			_otherGem = board.AllGems[PosIndex.x, PosIndex.y + 1];
			_otherGem.PosIndex.y--;
			PosIndex.y++;
		}
		else if(_swipeAngle < -45 && _swipeAngle >= -135 && PosIndex.y > 0)
		{
			_otherGem = board.AllGems[PosIndex.x, PosIndex.y - 1];
			_otherGem.PosIndex.y++;
			PosIndex.y--;
		}
		else if(_swipeAngle > 135 || _swipeAngle < -135 && PosIndex.x > 0)
		{
			_otherGem = board.AllGems[PosIndex.x - 1, PosIndex.y];
			_otherGem.PosIndex.x++;
			PosIndex.x--;
		}

		board.AllGems[PosIndex.x, PosIndex.y] = this;
		board.AllGems[_otherGem.PosIndex.x, _otherGem.PosIndex.y] = _otherGem;

		StartCoroutine(CheckMoveCo());
	}

	public IEnumerator CheckMoveCo()
	{
		board.CurrentState = BoardState.wait;

		yield return new WaitForSeconds(.5f);

		board.MatchFind.FindAllMatches();

		if(_otherGem != null)
		{
			if(!IsMatched && !_otherGem.IsMatched)
			{
				_otherGem.PosIndex = PosIndex;
				PosIndex = _previousPos;

				board.AllGems[PosIndex.x, PosIndex.y] = this;
				board.AllGems[_otherGem.PosIndex.x, _otherGem.PosIndex.y] = _otherGem;

				yield return new WaitForSeconds(.5f);

				board.CurrentState = BoardState.move;
			} 
			else
			{
				board.DestroyMatches();
			}
		}
	}
}
