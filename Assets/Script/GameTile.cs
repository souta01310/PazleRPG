using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class GameTile : MonoBehaviour
{
    public GameObject[] StonePrehubs;
    public int width = 6;
    public int height = 6;
    public int MaxRange;
    public GameObject[,]tileStone = new GameObject[6,6];
    private List<GameObject> deleteList = new List<GameObject>();
    private bool isStart;
 
    void Start()
    {
        GenetateStone();
    }
    void GenetateStone()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int r = Random.Range(0, MaxRange);
                var stone = Instantiate(StonePrehubs[r]);
                stone.transform.position = new Vector2(i,j);
                tileStone[i,j] = stone;
            }
        }
        CheckStartset();
    }
    void CheckStartset()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width-2; j++)
            {
                if ((tileStone[j,i].tag == tileStone[j+1,i].tag) 
                    && (tileStone[j,i].tag == tileStone[j+2,i].tag))
                {
                    tileStone[j,i].GetComponent<StoneMove>().isMatching = true;
                    tileStone[j+1, i].GetComponent<StoneMove>().isMatching = true;
                    tileStone[j+2,i].GetComponent<StoneMove>().isMatching=true;
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height-2; j++)
            {
                if ((tileStone[i, j].tag == tileStone[i,j+1].tag) 
                    && (tileStone[i,j].tag == tileStone[i,j+2].tag))
                {
                    tileStone[i,j].GetComponent<StoneMove>().isMatching = true;
                    tileStone[i, j].GetComponent<StoneMove>().isMatching = true;
                    tileStone[i, j].GetComponent<StoneMove>().isMatching = true;
                }
            }
        }
        foreach(var item in tileStone)
        {
            if (item.GetComponent<StoneMove>().isMatching)
            {
                deleteList.Add(item);
            }
        }
        if(deleteList.Count > 0)
        {
            foreach (var item in deleteList)
            {
                tileStone[(int)item.transform.position.x,
                    (int)item.transform.position.y] = null;
                Destroy(item);
            }
            deleteList.Clear();
            SpawnNewStone();
        }
        else
        {
            isStart = true;
        }
    }
    void SpawnNewStone()
    {
        for(int i = 0;i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileStone[i,j] == null)
                {
                    int r = Random.Range(0,5);
                    var stone = Instantiate(StonePrehubs[r]);
                    stone.transform.position = new Vector2(i,j+0.3f);
                    tileStone[i,j] = stone;
                }
            }
        }
        if(isStart == false)
        {
            CheckStartset();
        }
        else
        {
            foreach(var item in tileStone)
            {
                int column = (int)item.transform.position.x;
                int row = (int)item.transform.position.y;
                item.GetComponent<StoneMove>().mypreviousPos = 
                    new Vector2(column, row);
            }
            Invoke("CheckMatching", 0.2f);
        }
    }
    public void CheckMatching()
    {
        for(int i = 0;i < height; i++)
        {
            for(int j = 0; j < width - 2; j++)
            {
                if ((tileStone[i, j].tag == tileStone[j+1,i].tag)&&
                    (tileStone[j, i].tag == tileStone[j + 2, i].tag))
                {
                    tileStone[j,i].GetComponent<StoneMove>().isMatching = true;
                    tileStone[j+1,i].GetComponent<StoneMove>().isMatching = true;
                }
            }
        }
        for(int i = 0;i < width; i++)
        {
            for (int j = 0;j < height-2; j++)
            {
                if ((tileStone[i, j].tag == tileStone[i,j+1].tag)&&
                    (tileStone[i, j].tag == tileStone[i, j + 2].tag))
                {
                    tileStone[i,j].GetComponent<StoneMove>().isMatching = true;
                    tileStone[i,j+1].GetComponent<StoneMove>().isMatching = true;
                    tileStone[i,j+2].GetComponent<StoneMove>().isMatching = true;
                }
            }
        }
        foreach(var item in tileStone)
        {
            if (item.GetComponent<StoneMove>().isMatching)
            {
                item.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                deleteList.Add(item);
            }
        }
        if(deleteList.Count > 0)
        {
            Invoke("DeleteStones", 0.2f);
        }
        if (deleteList.Count > 0)
        {
            Invoke("DeletStones", 0.2f);
        }
        else
        {
            foreach(var item in deleteList)
            {
                item.GetComponent<StoneMove>().BackToPreviouspos();
            }
        }
    }
    void DeleteStones()
    {
        foreach(var item in deleteList)
        {
            Destroy(item);
            tileStone[(int)item.transform.position.x,(int)item.transform.position.y] = null;

        }
        deleteList.Clear();
        Invoke("SpawnNewStone", 1.2f);
    }
    public void StopStones()
    {
        foreach(var item in tileStone)
        {
            item.GetComponent<StoneMove>().isMoving = true;
        }
    }
    void MoveStones()
    {
        foreach(var item in tileStone)
        {
            item.GetComponent<StoneMove>().isMoving = false;
        }
    }
}
