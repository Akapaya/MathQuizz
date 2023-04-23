using UnityEngine;
using DG.Tweening;
using System;

public class TableManager : MonoBehaviour
{
    [Header("Settings Table")]
    [SerializeField] private int QtCollums = 5;
    [SerializeField] private int QtRows = 4;
    [SerializeField] private int PosXInitial = 100;
    [SerializeField] private int PosYInitial = -100;
    [SerializeField] private int PosXOffset = 150;
    [SerializeField] private int PosYOffset = 150;
    [SerializeField] private float velocityMoveSquare = 0.3f;
    [Header("UiElements")]
    [SerializeField] private GameObject squareObject;
    [SerializeField] private GameObject painelSquaresObjects;
    [Header("Matrix Table")]
    private RectTransform[,] squaresPosition;
    [SerializeField] Vector2 emptyPosition;

    public delegate void MoveSquareEvent(RectTransform square);
    public static MoveSquareEvent MoveSquareHandle;

    public delegate void SpawnSquaresEvent(int result);
    public static SpawnSquaresEvent SpawnSquaresHandle;

    private void OnEnable()
    {
        MoveSquareHandle += MoveSquare;
        SpawnSquaresHandle += SpawnSquares;
    }

    private void OnDisable()
    {
        MoveSquareHandle -= MoveSquare;
        SpawnSquaresHandle -= SpawnSquares;
    }

    void SpawnSquares(int result)
    {
        ClearTable();
        int xPos = UnityEngine.Random.Range(0, squaresPosition.GetLength(0) - 1);
        int yPos = UnityEngine.Random.Range(0, squaresPosition.GetLength(1) - 1);
        for (int i = 0; i < squaresPosition.GetLength(0); i++)
        {
            for (int t = 0; t < squaresPosition.GetLength(1); t++)
            {
                GameObject item;
                if (i == squaresPosition.GetLength(0) - 1 && t == squaresPosition.GetLength(1) - 1)
                {
                    item = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
                    Destroy(item.GetComponent<Transform>());
                    item.AddComponent<RectTransform>();
                }
                else
                {
                    item = Instantiate(squareObject, Vector3.zero, Quaternion.identity);
                }
                item.transform.SetParent(painelSquaresObjects.transform);
                item.name = i + "/" + t;
                squaresPosition[i, t] = item.GetComponent<RectTransform>();
                squaresPosition[i, t].anchoredPosition3D = new Vector3(PosXInitial + (PosXOffset * i), PosYInitial - (PosYOffset * t),0);
                item.transform.localScale = Vector3.one;
                if(i == xPos && t == yPos)
                {
                    if (item.TryGetComponent(out SquareControl component))
                    {
                        component.NumberTxt.text = result.ToString();
                    }
                }
                else
                {
                    if (item.TryGetComponent(out SquareControl component))
                    {
                        component.NumberTxt.text = QuestionManager.RandomApproximately(result).ToString();
                    }
                }
            }
        }
        PossiblesMoves();
    }

    void PossiblesMoves()
    {
        for (int i = 0; i < squaresPosition.GetLength(0); i++)
        {
            for (int t = 0; t < squaresPosition.GetLength(1); t++)
            {
                if (squaresPosition[i, t].childCount == 0)
                {
                    emptyPosition = new Vector2(i,t);
                    try
                    {
                        squaresPosition[i - 1, t].DOScale(1.1f, 0.5f).SetLoops(-1,LoopType.Yoyo);
                        squaresPosition[i - 1, t].GetComponent<SquareControl>().CanClick = true;
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                    try
                    {
                        squaresPosition[i + 1, t].DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                        squaresPosition[i + 1, t].GetComponent<SquareControl>().CanClick = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    try
                    {
                        squaresPosition[i , t - 1].DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                        squaresPosition[i , t - 1].GetComponent<SquareControl>().CanClick = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    try
                    {
                        squaresPosition[i, t + 1].DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                        squaresPosition[i , t +1].GetComponent<SquareControl>().CanClick = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
        }
    }

    public void MoveSquare(RectTransform item)
    {
        SquareControl.SetCanClickHandle?.Invoke(false);
        for (int i = 0; i < squaresPosition.GetLength(0); i++)
        {
            for (int t = 0; t < squaresPosition.GetLength(1); t++)
            {
                if (squaresPosition[i, t] == item)
                {
                    Vector2 temp = item.anchoredPosition;
                    int i2 = i;
                    int t2 = t;
                    item.DOAnchorPos(squaresPosition[(int)emptyPosition.x, (int)emptyPosition.y].anchoredPosition, velocityMoveSquare).OnComplete(() =>{
                        squaresPosition[i2, t2] = squaresPosition[(int)emptyPosition.x, (int)emptyPosition.y];
                        squaresPosition[(int)emptyPosition.x, (int)emptyPosition.y] = item;
                        UpdateAwnser();
                        PossiblesMoves();
                    });
                    squaresPosition[(int)emptyPosition.x, (int)emptyPosition.y].anchoredPosition = temp;
                }
            }
        }
    }

    private void UpdateAwnser()
    {
        if(squaresPosition[QtCollums - 1, QtRows - 1].childCount > 0)
        {
            UiManager.UpdateAwnserHandle?.Invoke(squaresPosition[QtCollums - 1, QtRows - 1].GetChild(0).GetComponent<TMPro.TMP_Text>().text);
        }
        else
        {
            UiManager.UpdateAwnserHandle?.Invoke("");
        }
    }


    private void ClearTable()
    {
        squaresPosition = new RectTransform[QtCollums, QtRows];
        for (int i = 0; i < painelSquaresObjects.transform.childCount; i++)
        {
            if(i!= 0)
            {
                Destroy(painelSquaresObjects.transform.GetChild(i).gameObject);
            }
        }
        /*foreach (RectTransform child in painelSquaresObjects.transform)
        {
            Destroy(child.gameObject);
        }*/
    }
}
