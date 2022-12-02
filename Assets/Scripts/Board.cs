using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
   //やること
    //二次原配列の作成
    private Transform[,] grid;

    //関数の作成//
    //CheckPositionに追記
    
    
    
    //変数の作成//
    //ボード基盤用の四角枠格納用
    //ボードの高さ
    //ボードの幅
    //ボードの高さ調整用数値

    [SerializeField]
    private Transform emptySprite;

    [SerializeField]
    private int height = 30, width=10, header=8;

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreatrBoard();
    }

    //関数の作成//
    //ボードを生成する関数の作成

    void CreatrBoard()
    {
        if (emptySprite)
        {
            for (int y = 0; y < height -header; y++)         
            {
                for (int x = 0; x < width; x++)
                {
                Transform clone = Instantiate(emptySprite, new Vector3(x,y,0),Quaternion.identity);
                clone.transform.parent=transform;
                }  
            }
            
        }
    }

    //ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block. transform)
        {
            Vector2 pos = Rounding. Round(item.position);
            if (!BoardOutCheck((int)pos. x, (int)pos. y))
            {
                return false;
            }
            if (BlockCheck((int)pos. x, (int)pos. y, block))
            {
                return false;
            }
        }
        return true;
    }

    //枠内にあるのか判定する関数
    bool BoardOutCheck(int x, int y)
    {
        //x軸は０以上width未満　y軸は０以上
        return(x>=0 && x<width && y>=0);
    }

    //移動先にブロックがないか判定する関数
    bool BlockCheck(int x, int y, Block block)
    {
        //二次元配列が空でないのは他のブロックがあるとき
        //親が違うのは他のブロックがあるとき
        return(grid[x,y] !=null && grid[x, y]. parent !=block.transform);

    }

    //ブロックが落ちたポジションを記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block. transform)
        {
            Vector2 pos =Rounding. Round(item.position);
            grid[(int)pos.x, (int)pos.y]=item;
        }
    }

    //関数の作成//
    //全ての行をチャックして、埋まっていれば削除する関数
    public void ClearAllRows()
    {
        for(int y = 0; y < height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y + 1);
                
                y--;
            }
        }
    }

    //全ての行をチェックする関数
    bool IsComplete(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid[x,y]==null)
            {
                return false; 
            }
        }
        
        FindObjectOfType<GameManager>().AddScore();
        return true;
    }
    
    //削除する関数

    void ClearRow(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid[x,y] != null)
            {
                Destroy(grid[x, y]. gameObject);
            }
            grid[x,y] = null;
        }
        
    }
    //上にあるブロックを1段下げる関数
    void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x,y] !=null)
                {
                    grid[x, y - 1] = grid[x,y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    


    


    public bool OverLimit(Block block)
    {
        foreach (Transform item in block. transform)
        {
            if (item.transform.position.y >= height-header)
            {
                return true;
            }
        }
        return false;
    }
}
