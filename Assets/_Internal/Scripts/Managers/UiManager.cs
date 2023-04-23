using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private TMP_Text num1Txt;
    [SerializeField] private TMP_Text num2Txt;
    [SerializeField] private TMP_Text awnserTxt;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Image signImage;
    [SerializeField] private Sprite[] signSprites;
    [SerializeField] private RectTransform gameoverPanel;

    public delegate void UpdateAwnserEvent(string awnser);
    public static UpdateAwnserEvent UpdateAwnserHandle;

    public delegate void UpdateTimerEvent(float time);
    public static UpdateTimerEvent UpdateTimerHandle;

    public delegate void SetQuestionEvent(int num1, int num2);
    public static SetQuestionEvent SetQuestionHandle;

    public delegate void SetSignEvent(string sign);
    public static SetSignEvent SetSignHandle;

    public delegate void UpdateLevelEvent(string level);
    public static UpdateLevelEvent UpdateLevelHandle;

    public delegate void GameOverEvent();
    public static GameOverEvent GameOverHandle;

    private void Start()
    {
        awnserTxt.rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1,LoopType.Yoyo);
    }
    private void OnEnable()
    {
        UpdateAwnserHandle += UpdateAwnser;
        UpdateTimerHandle += UpdateTimer;
        SetQuestionHandle += SetQuestion;
        GameOverHandle += GameOverPanel;
        UpdateLevelHandle += UpdateLevel;
        SetSignHandle += SetSign;
    }

    private void OnDisable()
    {
        UpdateAwnserHandle -= UpdateAwnser;
        UpdateTimerHandle -= UpdateTimer;
        SetQuestionHandle -= SetQuestion;
        GameOverHandle -= GameOverPanel;
        UpdateLevelHandle -= UpdateLevel;
        SetSignHandle -= SetSign;
    }

    private void SetQuestion(int num1, int num2)
    {
        num1Txt.text = num1.ToString();
        num2Txt.text = num2.ToString();
    }

    private void SetSign(string sign)
    {
        switch (sign)
        {
            case "Addition":
                {
                    signImage.sprite = signSprites[0];
                    break;
                }
            case "Subtraction":
                {
                    signImage.sprite = signSprites[1];
                    break;
                }
            default:
                {
                    signImage.sprite = signSprites[0];
                    break;
                }
        }
    }

    private void UpdateAwnser(string awnser)
    {
        awnserTxt.text = awnser;
    }

    private void UpdateLevel(string level)
    {
        levelTxt.text = level;
    }

    private void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void GameOverPanel()
    {
        gameoverPanel.DOAnchorPos(new Vector2(0,0), 1f);
    }

    public void CheckAwnser()
    {
        GameManager.CheckAwnserHandle?.Invoke(awnserTxt.text);
    }

    public void MenuScreen()
    {
        SceneManager.LoadScene(0);
    }
}
