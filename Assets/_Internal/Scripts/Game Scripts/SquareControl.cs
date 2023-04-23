using DG.Tweening;
using TMPro;
using UnityEngine;

public class SquareControl : MonoBehaviour
{
    [SerializeField] private bool canClick = false;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMPro.TMP_Text numberTxt;

    public TMP_Text NumberTxt { get => numberTxt; set => numberTxt = value; }

    public delegate void SetCanClickEvent(bool value);
    public static SetCanClickEvent SetCanClickHandle;


    public bool CanClick
    {
        set
        {
            canClick = value;
        }
    }

    

    private void OnEnable()
    {
        SetCanClickHandle += SetCanClick;
    }

    private void OnDisable()
    {
        SetCanClickHandle -= SetCanClick;
    }

    private void SetCanClick(bool value)
    {
        CanClick = value;
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;
    }


    public void CLicked()
    {
        if (canClick)
            TableManager.MoveSquareHandle?.Invoke(rectTransform);
    }
}
