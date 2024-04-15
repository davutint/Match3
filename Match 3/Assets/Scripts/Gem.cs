using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GemType { earth, jupiter, mars, neptune, uranus, venus,bomb, stone,planet1,planet2,planet3,planet4,planet5,planet6}
[RequireComponent(typeof(BoxCollider2D))]
public class Gem : MonoBehaviour
{
   //[HideInInspector]
	public Vector2Int posIndex;
	//[HideInInspector]
	public Board board;


	private Vector2 _firstTouchPosition;
	private Vector2 _finalTouchPosition;

	private bool _mousePressed;
	private float _swipeAngle = 0;

	private Gem _otherGem;

	
	public GemType type;

	public bool isMatched;
	
	private Vector2Int _previousPos;

	public GameObject destroyEffect;

	public int blastSize = 2;

	public int scoreValue = 10;

   
	void Update()
	{
		if (Vector2.Distance(transform.position, posIndex) > .01f)
		{
			transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
		} 
		else
		{
			transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
			board.allGems[posIndex.x, posIndex.y] = this;
		}

		if(_mousePressed && Input.GetMouseButtonUp(0))
		{
			_mousePressed = false;

			if(board.currentState == BoardState.move && board.roundMan.RoundTime > 0)
			{
				_finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				CalculateAngle();
			}
		}
	}

	public void SetupGem(Vector2Int pos, Board theBoard)
	{
		posIndex = pos;
		board = theBoard;
	}

	
	private void OnMouseDown()
	{
		if(board.currentState == BoardState.move && board.roundMan.RoundTime > 0)//Board.BoardState.move olarak değiştirirsin eğer çalışmazsa
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
		_previousPos = posIndex;

		if(_swipeAngle < 45 && _swipeAngle > -45 && posIndex.x < board.width - 1)
		{
			_otherGem = board.allGems[posIndex.x + 1, posIndex.y];
			_otherGem.posIndex.x--;
			posIndex.x++;
		}
		else if(_swipeAngle > 45 && _swipeAngle <= 135 && posIndex.y < board.height - 1)
		{
			_otherGem = board.allGems[posIndex.x, posIndex.y + 1];
			_otherGem.posIndex.y--;
			posIndex.y++;
		}
		else if(_swipeAngle < -45 && _swipeAngle >= -135 && posIndex.y > 0)
		{
			_otherGem = board.allGems[posIndex.x, posIndex.y - 1];
			_otherGem.posIndex.y++;
			posIndex.y--;
		}
		else if(_swipeAngle > 135 || _swipeAngle < -135 && posIndex.x > 0)
		{
			_otherGem = board.allGems[posIndex.x - 1, posIndex.y];
			_otherGem.posIndex.x++;
			posIndex.x--;
		}

		board.allGems[posIndex.x, posIndex.y] = this;
		board.allGems[_otherGem.posIndex.x, _otherGem.posIndex.y] = _otherGem;

		StartCoroutine(CheckMoveCo());
	}

	public IEnumerator CheckMoveCo()
	{
		board.currentState = BoardState.wait;

		yield return new WaitForSeconds(.5f);

		board.matchFind.FindAllMatches();

		if(_otherGem != null)
		{
			if(!isMatched && !_otherGem.isMatched)
			{
				_otherGem.posIndex = posIndex;
				posIndex = _previousPos;

				board.allGems[posIndex.x, posIndex.y] = this;
				board.allGems[_otherGem.posIndex.x, _otherGem.posIndex.y] = _otherGem;

				yield return new WaitForSeconds(.5f);

				board.currentState = BoardState.move;
			} 
			else
			{
				board.DestroyMatches();
			}
		}
	}
}
