using UnityEngine;
//https://unity2dpuzzle.jimdofree.com/home/match-3-puzzle/

public class GameTile : MonoBehaviour
{
    public GameObject[] StonePrehubs;
    public int width = 6;
    public int height = 6;
    public int MaxRange;
    public GameObject[,]tileStone = new GameObject[6,6];
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
        ChackStartset();
    }
    void ChackStartset()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width - 2; j++) ;
        }
    }
}
