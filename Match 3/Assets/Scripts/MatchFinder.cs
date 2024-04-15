using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board _board;
    public List<Gem> CurrentMatches = new List<Gem>();

    private void Awake()
    {
        _board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        CurrentMatches.Clear();

        for (int x = 0; x < _board.Width; x++)//seçili Gem in x ve y düzleminde etrafında aynı enum türünden obje olup olmadığını kontrol ediyoruz.
        {
            for (int y = 0; y < _board.Height; y++)
            {

                Gem currentGem = _board.AllGems[x, y];
                if(currentGem != null)
                {
                    if(x > 0 && x < _board.Width - 1)
                    {
                        Gem leftGem = _board.AllGems[x - 1, y];
                        Gem rightGem = _board.AllGems[x + 1, y];
                        if(leftGem != null && rightGem != null)
                        {
                            if(leftGem.Type == currentGem.Type && rightGem.Type == currentGem.Type && currentGem.Type != GemType.stone)
                            {
                                currentGem.IsMatched = true;
                                leftGem.IsMatched = true;
                                rightGem.IsMatched = true;

                                CurrentMatches.Add(currentGem);
                                CurrentMatches.Add(leftGem);
                                CurrentMatches.Add(rightGem);
                            }
                        }
                    }

                    if (y > 0 && y < _board.Height - 1)
                    {
                        Gem aboveGem = _board.AllGems[x, y+1];
                        Gem belowGem = _board.AllGems[x, y-1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.Type == currentGem.Type && belowGem.Type == currentGem.Type && currentGem.Type != GemType.stone)
                            {
                                currentGem.IsMatched = true;
                                aboveGem.IsMatched = true;
                                belowGem.IsMatched = true;

                                CurrentMatches.Add(currentGem);
                                CurrentMatches.Add(aboveGem);
                                CurrentMatches.Add(belowGem);
                            }
                        }
                    }
                }
            }
        }

        if(CurrentMatches.Count > 0)
        {
            CurrentMatches = CurrentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    public void CheckForBombs()
    {
        for(int i = 0; i < CurrentMatches.Count; i++)
        {
            Gem gem = CurrentMatches[i];

            int x = gem.PosIndex.x;
            int y = gem.PosIndex.y;

            if(gem.PosIndex.x > 0)
            {
                if(_board.AllGems[x-1, y] != null)
                {
                    if(_board.AllGems[x-1, y].Type == GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x-1, y), _board.AllGems[x-1,y]);
                    }
                }
            }

            if (gem.PosIndex.x < _board.Width - 1)
            {
                if (_board.AllGems[x + 1, y] != null)
                {
                    if (_board.AllGems[x + 1, y].Type == GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), _board.AllGems[x + 1, y]);
                    }
                }
            }

            if (gem.PosIndex.y > 0)
            {
                if (_board.AllGems[x, y - 1] != null)
                {
                    if (_board.AllGems[x, y - 1].Type == GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), _board.AllGems[x, y - 1]);
                    }
                }
            }

            if (gem.PosIndex.y < _board.Height - 1)
            {
                if (_board.AllGems[x, y + 1] != null)
                {
                    if (_board.AllGems[x, y + 1].Type == GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), _board.AllGems[x, y + 1]);
                    }
                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        for(int x = bombPos.x - theBomb.BlastSize; x <= bombPos.x + theBomb.BlastSize; x++)
        {
            for(int y = bombPos.y - theBomb.BlastSize; y <= bombPos.y + theBomb.BlastSize; y++)
            {
                if(x >= 0 && x < _board.Width && y >= 0 && y < _board.Height)
                {
                    if(_board.AllGems[x,y] != null)
                    {
                        _board.AllGems[x, y].IsMatched = true;
                        CurrentMatches.Add(_board.AllGems[x, y]);
                    }
                }
            }
        }

        CurrentMatches = CurrentMatches.Distinct().ToList();
    }
}
