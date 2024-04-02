using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BoardState {  wait, move}
public  class Board : MonoBehaviour
{
	public int width;
	public int height;

	public GameObject bgTilePrefab;

	public Gem[] gems;

	public Gem[,] allGems;

	public float gemSpeed;

	[HideInInspector]
	public MatchFinder matchFind;

   
	public BoardState currentState = BoardState.move;

	public Gem bomb;
	public float bombChance = 2f;

	[HideInInspector]
	public RoundManager roundMan;

	private float _bonusMulti;//ard arda patlattığımızda puanımız bu çarpana göre çarpılıp artıyor
	public float bonusAmount = .5f;

	private BoardLayout _boardLayout;
	private Gem[,] _layoutStore;

	private void Awake()
	{
		matchFind = FindObjectOfType<MatchFinder>();
		roundMan = FindObjectOfType<RoundManager>();
		_boardLayout = GetComponent<BoardLayout>();
	}

	// Start is called before the first frame update
	void Start()
	{
		
		allGems = new Gem[width, height];

		_layoutStore = new Gem[width, height];

		Setup();
	}
	

	private void Update()
	{
		//matchFind.FindAllMatches();

		if(Input.GetKeyDown(KeyCode.S))
		{
			ShuffleBoard();
		}
	}

	private void Setup()
	{
		if(_boardLayout != null)
		{
			_layoutStore = _boardLayout.GetLayout();
		}


		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Vector2 pos = new Vector2(x, y);
				GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
				bgTile.transform.parent = transform;
				bgTile.name = "BG Tile - " + x + ", " + y;

				if (_layoutStore[x, y] != null)
				{
					SpawnGem(new Vector2Int(x, y), _layoutStore[x, y]);
				}
				else
				{

					int gemToUse = Random.Range(0, gems.Length);

					int iterations = 0;
					while (MatchesAt(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
					{
						gemToUse = Random.Range(0, gems.Length);
						iterations++;
					}

					SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
				}
			}
		}

		
	}

	private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
	{
		if(Random.Range(0f, 100f) < bombChance)
		{
			gemToSpawn = bomb;
		}

		Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + height, 0f), Quaternion.identity);
		gem.transform.parent = transform;
		gem.name = "Gem - " + pos.x + ", " + pos.y;
		allGems[pos.x, pos.y] = gem;

		gem.SetupGem(pos, this);
	}

	bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
	{
		if(posToCheck.x > 1)
		{
			if(allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
			{
				return true;
			}
		}

		if (posToCheck.y > 1)
		{
			if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
			{
				return true;
			}
		}

		return false;
	}

	private void DestroyMatchedGemAt(Vector2Int pos)//eşleşen objeleri ses ve particle effect eşliğinde yok ediyoruz
	{
		if(allGems[pos.x, pos.y] != null)
		{
			if(allGems[pos.x, pos.y].isMatched)
			{
				if(allGems[pos.x, pos.y].type == GemType.bomb)
				{
					SFXManager.instance.PlayExplode();
				} else if (allGems[pos.x, pos.y].type == GemType.stone)
				{
					SFXManager.instance.PlayStoneBreak();
				} else
				{
					SFXManager.instance.PlayGemBreak();
				}

				Instantiate(allGems[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
				roundMan.roundTime+=0.6f;
				Destroy(allGems[pos.x, pos.y].gameObject);
				allGems[pos.x, pos.y] = null;
			}
		}
	}

	public void DestroyMatches()
	{
		for(int i = 0; i < matchFind.currentMatches.Count; i++)
		{
			if(matchFind.currentMatches[i] != null)
			{
				ScoreCheck(matchFind.currentMatches[i]);

				DestroyMatchedGemAt(matchFind.currentMatches[i].posIndex);
			}
		}

		StartCoroutine(DecreaseRowCo());
	}

	private IEnumerator DecreaseRowCo()
	{
		yield return new WaitForSeconds(.2f);

		int nullCounter = 0;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if(allGems[x,y] == null)
				{
					nullCounter++;
				} else if(nullCounter > 0)
				{
					allGems[x, y].posIndex.y -= nullCounter;
					allGems[x, y - nullCounter] = allGems[x, y];
					allGems[x, y] = null;
				}

			}

			nullCounter = 0;
		}

		StartCoroutine(FillBoardCo());
	}

	private IEnumerator FillBoardCo()
	{
		yield return new WaitForSeconds(.5f);
		RefillBoard();

		yield return new WaitForSeconds(.5f);

		matchFind.FindAllMatches();

		if(matchFind.currentMatches.Count > 0)
		{
			_bonusMulti++;

			yield return new WaitForSeconds(.5f);
			DestroyMatches();
		} else
		{
			yield return new WaitForSeconds(.5f);
			currentState = BoardState.move;

			_bonusMulti = 0f;
		}
	}

	private void RefillBoard()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (allGems[x, y] == null)
				{
					int gemToUse = Random.Range(0, gems.Length);

					SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
				}
			}
		}

		CheckMisplacedGems();
	}

	private void CheckMisplacedGems()
	{
		List<Gem> foundGems = new List<Gem>();

		foundGems.AddRange(FindObjectsOfType<Gem>());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if(foundGems.Contains(allGems[x,y]))
				{
					foundGems.Remove(allGems[x, y]);
				}
			}
		}

		foreach(Gem g in foundGems)
		{
			Destroy(g.gameObject);
		}
	}

	public void ShuffleBoard()// Gemleri karıştırma yapıyoruz,butona bağlanacak
	{
		if (currentState != BoardState.wait)
		{
			currentState = BoardState.wait;

			List<Gem> gemsFromBoard = new List<Gem>();

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					gemsFromBoard.Add(allGems[x, y]);
					allGems[x, y] = null;
				}
			}

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int gemToUse = Random.Range(0, gemsFromBoard.Count);

					int iterations = 0;
					while(MatchesAt(new Vector2Int(x,y), gemsFromBoard[gemToUse]) && iterations < 100 && gemsFromBoard.Count > 1)
					{
						gemToUse = Random.Range(0, gemsFromBoard.Count);
						iterations++;
					}

					gemsFromBoard[gemToUse].SetupGem(new Vector2Int(x, y), this);
					allGems[x, y] = gemsFromBoard[gemToUse];
					gemsFromBoard.RemoveAt(gemToUse);
				}
			}

			StartCoroutine(FillBoardCo());
		}
	}

	public void ScoreCheck(Gem gemToCheck)//highscoru ve firebase e skor yollamayı bu fonksiyon altında yapcağız
	{
		roundMan.currentScore += gemToCheck.scoreValue;

		if(_bonusMulti > 0)
		{
			float bonusToAdd = gemToCheck.scoreValue * _bonusMulti * bonusAmount;
			roundMan.currentScore += Mathf.RoundToInt(bonusToAdd);
		}
		//highscoreCheck
		//sendscoreFirebase
		//Scene scene=SceneManager.GetActiveScene();
		//switc case ile scene'in case durumlarını tespit edip hangi sahne de hangi yüksek skor tutulacağına bu şekilde bakacağız
	}
	
	
	
	
	
	
	
	
	public float CameraX()
	{
		float x=(float)width/2-0.5f;
		return x;
	}
	public float CameraY()
	{
		float y=(float)height/2-0.5f;
		return y;
	}
}