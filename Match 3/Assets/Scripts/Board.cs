using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BoardState {  wait, move}
public  class Board : MonoBehaviour
{
	public int Width;
	public int Height;

	public GameObject BgTilePrefab;

	public Gem[] Gems;

	public Gem[,] AllGems;

	public float GemSpeed;

	[HideInInspector]
	public MatchFinder MatchFind;

   
	public BoardState CurrentState = BoardState.move;

	public Gem Bomb;
	public float BombChance = 2f;

	[HideInInspector]
	public RoundManager RoundMan;

	private float _bonusMulti;//ard arda patlattığımızda puanımız bu çarpana göre çarpılıp artıyor
	public float BonusAmount = .5f;

	private BoardLayout _boardLayout;
	private Gem[,] _layoutStore;

	private void Awake()
	{
		MatchFind = FindObjectOfType<MatchFinder>();
		RoundMan = FindObjectOfType<RoundManager>();
		_boardLayout = GetComponent<BoardLayout>();
	}

	// Start is called before the first frame update
	void Start()
	{
		
		AllGems = new Gem[Width, Height];

		_layoutStore = new Gem[Width, Height];

		Setup();
	}
	

	private void Setup()
	{
		if(_boardLayout != null)
		{
			_layoutStore = _boardLayout.GetLayout();
		}


		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
				Vector2 pos = new Vector2(x, y);
				GameObject bgTile = Instantiate(BgTilePrefab, pos, Quaternion.identity);
				bgTile.transform.parent = transform;
				bgTile.name = "BG Tile - " + x + ", " + y;

				if (_layoutStore[x, y] != null)
				{
					SpawnGem(new Vector2Int(x, y), _layoutStore[x, y]);
				}
				else
				{

					int gemToUse = Random.Range(0, Gems.Length);

					int iterations = 0;
					while (MatchesAt(new Vector2Int(x, y), Gems[gemToUse]) && iterations < 100)
					{
						gemToUse = Random.Range(0, Gems.Length);
						iterations++;
					}

					SpawnGem(new Vector2Int(x, y), Gems[gemToUse]);
				}
			}
		}

		
	}

	private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
	{
		if(Random.Range(0f, 100f) < BombChance)
		{
			gemToSpawn = Bomb;
		}

		Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + Height, 0f), Quaternion.identity);
		gem.transform.parent = transform;
		gem.name = "Gem - " + pos.x + ", " + pos.y;
		AllGems[pos.x, pos.y] = gem;

		gem.SetupGem(pos, this);
	}

	bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
	{
		if(posToCheck.x > 1)
		{
			if(AllGems[posToCheck.x - 1, posToCheck.y].Type == gemToCheck.Type && AllGems[posToCheck.x - 2, posToCheck.y].Type == gemToCheck.Type)
			{
				return true;
			}
		}

		if (posToCheck.y > 1)
		{
			if (AllGems[posToCheck.x, posToCheck.y - 1].Type == gemToCheck.Type && AllGems[posToCheck.x, posToCheck.y - 2].Type == gemToCheck.Type)
			{
				return true;
			}
		}

		return false;
	}

	private void DestroyMatchedGemAt(Vector2Int pos)//eşleşen objeleri ses ve particle effect eşliğinde yok ediyoruz
	{
		if(AllGems[pos.x, pos.y] != null)
		{
			if(AllGems[pos.x, pos.y].IsMatched)
			{
				if(AllGems[pos.x, pos.y].Type == GemType.bomb)
				{
					SFXManager.Instance.PlayExplode();
				} else if (AllGems[pos.x, pos.y].Type == GemType.stone)//şimdilik gerek yok belki sonra ekleriz
				{
					SFXManager.Instance.PlayStoneBreak();
				} else
				{
					SFXManager.Instance.PlayGemBreak();
				}

				Instantiate(AllGems[pos.x, pos.y].DestroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
				//roundMan.roundTime+=0.6f; oyun sonsuza kadar sürüyor, iptal..
				Destroy(AllGems[pos.x, pos.y].gameObject);
				AllGems[pos.x, pos.y] = null;
			}
		}
	}

	public void DestroyMatches()
	{
		for(int i = 0; i < MatchFind.CurrentMatches.Count; i++)
		{
			if(MatchFind.CurrentMatches[i] != null)
			{
				ScoreCheck(MatchFind.CurrentMatches[i]);

				DestroyMatchedGemAt(MatchFind.CurrentMatches[i].PosIndex);
			}
		}

		StartCoroutine(DecreaseRowCo());
	}

	private IEnumerator DecreaseRowCo()
	{
		yield return new WaitForSeconds(.2f);

		int nullCounter = 0;

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if(AllGems[x,y] == null)
				{
					nullCounter++;
				} else if(nullCounter > 0)
				{
					AllGems[x, y].PosIndex.y -= nullCounter;
					AllGems[x, y - nullCounter] = AllGems[x, y];
					AllGems[x, y] = null;
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

		MatchFind.FindAllMatches();

		if(MatchFind.CurrentMatches.Count > 0)
		{
			_bonusMulti++;

			yield return new WaitForSeconds(.5f);
			DestroyMatches();
		} else
		{
			yield return new WaitForSeconds(.5f);
			CurrentState = BoardState.move;

			_bonusMulti = 0f;
		}
	}

	private void RefillBoard()
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if (AllGems[x, y] == null)
				{
					int gemToUse = Random.Range(0, Gems.Length);

					SpawnGem(new Vector2Int(x, y), Gems[gemToUse]);
				}
			}
		}

		CheckMisplacedGems();
	}

	private void CheckMisplacedGems()
	{
		List<Gem> foundGems = new List<Gem>();

		foundGems.AddRange(FindObjectsOfType<Gem>());

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				if(foundGems.Contains(AllGems[x,y]))
				{
					foundGems.Remove(AllGems[x, y]);
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
		if (CurrentState != BoardState.wait)
		{
			CurrentState = BoardState.wait;

			List<Gem> gemsFromBoard = new List<Gem>();

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					gemsFromBoard.Add(AllGems[x, y]);
					AllGems[x, y] = null;
				}
			}

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					int gemToUse = Random.Range(0, gemsFromBoard.Count);

					int iterations = 0;
					while(MatchesAt(new Vector2Int(x,y), gemsFromBoard[gemToUse]) && iterations < 100 && gemsFromBoard.Count > 1)
					{
						gemToUse = Random.Range(0, gemsFromBoard.Count);
						iterations++;
					}

					gemsFromBoard[gemToUse].SetupGem(new Vector2Int(x, y), this);
					AllGems[x, y] = gemsFromBoard[gemToUse];
					gemsFromBoard.RemoveAt(gemToUse);
				}
			}

			StartCoroutine(FillBoardCo());
		}
	}

	public void ScoreCheck(Gem gemToCheck)
	{
		RoundMan.CurrentScore += gemToCheck.ScoreValue;

		if(_bonusMulti > 0)
		{
			float bonusToAdd = gemToCheck.ScoreValue * _bonusMulti * BonusAmount;
			RoundMan.CurrentScore += Mathf.RoundToInt(bonusToAdd);
		}
		
	}
	
	
	
	
	
	
	
	
	public float CameraX()//şu an için gerekli değil
	{
		float x=(float)Width/2-0.5f;
		return x;
	}
	public float CameraY()
	{
		float y=(float)Height/2-0.5f;
		return y;
	}
}
