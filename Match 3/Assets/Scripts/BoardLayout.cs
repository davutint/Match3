using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayout : MonoBehaviour
{
    public LayoutRow[] AllRows;

    public Gem[,] GetLayout()//eğer levele özel bir başlangıç dizilimi yapmak istiyorsak kullanılabilir.
    {
        Gem[,] theLayout = new Gem[AllRows[0].gemsInRow.Length, AllRows.Length];

        for(int y = 0; y < AllRows.Length; y++)
        {
            for(int x = 0; x < AllRows[y].gemsInRow.Length; x++)
            {
                if(x < theLayout.GetLength(0))
                {
                    if(AllRows[y].gemsInRow[x] != null)
                    {
                        theLayout[x, AllRows.Length - 1 - y] = AllRows[y].gemsInRow[x];
                    }
                }
            }
        }



        return theLayout;
    }
}

[System.Serializable]
public class LayoutRow
{
    public Gem[] gemsInRow;
}
