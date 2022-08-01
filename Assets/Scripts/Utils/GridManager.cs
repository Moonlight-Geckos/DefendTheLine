using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 10)]
    private int gridWidth;

    [SerializeField]
    [Range(1f, 10)]
    private int gridHeight;

    private static GridManager _instance;
    private Material _gridMaterial;
    private Blob[,] _grid;
    List<int> _emptyCells;

    public static GridManager Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _gridMaterial = GetComponent<Renderer>().sharedMaterial;

            SetupBlobsGrid();
            SetupEmptyCells();

            EventsPool.UserSwipedEvent.AddListener(TryMerge);
        }
    }
    public Tuple<int, int> GetEmptyCell()
    {
        if (_emptyCells.Count == 0)
            return null;
        else
        {
            int randPos = UnityEngine.Random.Range(0, _emptyCells.Count);
            return HashToIndex(_emptyCells[randPos]);
        }
    }
    public void AddBlob(Blob blob, Tuple<int, int> index)
    {
        if (_grid[index.Item1, index.Item2] == null)
            _grid[index.Item1, index.Item2] = blob;
        else
        {
            _grid[index.Item1, index.Item2].Merge();
        }
        _emptyCells.Remove(IndexToHash(index.Item1, index.Item2));
        SetupEmptyCells();
    }
    public void RemoveBlob(Tuple<int, int> index)
    {
        var hash = IndexToHash(index.Item1, index.Item2);
        _grid[index.Item1, index.Item2] = null;
        SetupEmptyCells();
    }
    private void SetupBlobsGrid()
    {
        _emptyCells = new List<int>();
        _grid = new Blob[gridHeight, gridWidth];

        _gridMaterial.SetFloat("_GridWidth", gridWidth);
        _gridMaterial.SetFloat("_GridHeight", gridHeight);

        transform.localScale = new Vector3(0.2f * gridWidth, 1, 0.2f * gridHeight);

    }
    private void SetupEmptyCells()
    {
        _emptyCells.Clear();
        for(int i = 0;i < gridHeight; i++)
        {
            for(int j = 0; j < gridWidth; j++)
                if (_grid[i,j] == null)
                {
                    int hash = IndexToHash(i, j);
                    _emptyCells.Add(hash);
                }
        }
    }
    private void TryMerge(SwipeDirection direction)
    {
        int i_inc = direction == SwipeDirection.Down ? 1 : -1;
        int j_inc = direction == SwipeDirection.Left ? 1 : -1;
        int si = direction == SwipeDirection.Down ? 0 : gridHeight - 1;
        int sj = direction == SwipeDirection.Left ? 0 : gridWidth - 1;
        void MakeMove(int i, int j)
        {
            int orgi = i, orgj = j;
            Tuple<int, int> newPos = null;
            while (CellIsValid(i, j))
            {
                if (direction == SwipeDirection.Down || direction == SwipeDirection.Up)
                {
                    i -= i_inc;
                }
                else
                {
                    j -= j_inc;
                }
                if (!CellIsValid(i, j))
                    break;
                if (_grid[i, j] == null)
                {
                    newPos = new Tuple<int, int>(i, j);
                }
                else if(newPos == null)
                {
                    if (_grid[i, j].Level == _grid[orgi, orgj].Level)
                    {
                        newPos = new Tuple<int, int>(i, j);
                    }
                    break;
                }
            }
            if (newPos != null)
            {
                _grid[orgi, orgj].Move(new Vector3(newPos.Item2, 0, newPos.Item1));
                AddBlob(_grid[orgi, orgj], newPos);
                RemoveBlob(new Tuple<int, int>(orgi, orgj));
            }
        }

        for (int i = si;i<gridHeight && i >= 0; i+=i_inc)
        {
            for (int j = sj; j < gridWidth && j >= 0; j += j_inc)
            {
                if (_grid[i, j] != null)
                {
                    MakeMove(i, j);
                }
            }
        }

    }
    private int IndexToHash(int i, int j)
    {
        return (i * gridWidth) + j;
    }
    private Tuple<int,int> HashToIndex(int hash)
    {
        Tuple<int, int> index = new Tuple<int, int>(hash / gridWidth, hash % gridWidth);
        return index;
    }
    private bool CellIsValid(int i, int j)
    {
        return i< gridHeight && i >= 0 && j < gridWidth && j >= 0;
    }
}