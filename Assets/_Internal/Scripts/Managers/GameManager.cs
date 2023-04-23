using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField] private Queue<string> modifiersQueue = new Queue<string>();
    [SerializeField] private int minNumber;
    [SerializeField] private int maxNumber;
    [SerializeField] private float timerInMinutes;
    [SerializeField] private float timerPlusCorrectAwnserInMinutes;
    private float timer;
    private int level;
    private int correctAwnser;

    public Queue<string> Modifiers { get => modifiersQueue; set => modifiersQueue = value; }

    public delegate void SetModifiersEvent(List<string> modifiers);
    public static SetModifiersEvent SetModifiersHandle;

    public delegate void CheckAwnserEvent(string awnser);
    public static CheckAwnserEvent CheckAwnserHandle;

    void Start()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else
        {
            if (gameManager != this)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    gameManager.SetQuestion();
                }
                Destroy(gameObject);
            }
        }
    }      
        

    private void OnEnable()
    {
        SetModifiersHandle += SetModifiers;
        CheckAwnserHandle += CheckAwnser;
    }

    private void OnDisable()
    {
        SetModifiersHandle -= SetModifiers;
        CheckAwnserHandle -= CheckAwnser;
    }

    private void SetModifiers(List<string> modifiers)
    {
        modifiersQueue.Clear();
        foreach (string item in modifiers)
        {
            modifiersQueue.Enqueue(item);
        }
        if (modifiersQueue.Count != 0)
        {
            SceneManager.LoadScene(1);
            level = 1;
            timer = timerInMinutes * 60;
            StartCoroutine("CountdownTimer");
        }
    }

    private void SetQuestion()
    {
        string[] question;
        switch (modifiersQueue.Peek())
        {
            case "Addition":
                {
                    question = QuestionManager.Addition(minNumber,maxNumber).Split('/');
                    break;
                }
            case "Subtraction":
                {
                    question = QuestionManager.Subtraction(minNumber, maxNumber).Split('/');
                    break;
                }
            default:
                {
                    question = QuestionManager.Addition(minNumber, maxNumber).Split('/');
                    break;
                }
        }
        correctAwnser = int.Parse(question[2]);
        UiManager.SetSignHandle?.Invoke(modifiersQueue.Peek());
        UiManager.SetQuestionHandle?.Invoke(int.Parse(question[0]), int.Parse(question[1]));
        modifiersQueue.Enqueue(modifiersQueue.Dequeue());
        TableManager.SpawnSquaresHandle?.Invoke(int.Parse(question[2]));
    }

    IEnumerator CountdownTimer()
    {
        do
        {
            timer--;
            UiManager.UpdateTimerHandle?.Invoke(timer);
            yield return new WaitForSecondsRealtime(1);
        } while (timer > 0);
        UiManager.GameOverHandle?.Invoke();
        StopCoroutine("CountdownTimer");
    }

    public void CheckAwnser(string awnser)
    {
        if (int.Parse(awnser) == correctAwnser)
        {
            level++;
            UiManager.UpdateLevelHandle?.Invoke(level.ToString());
            UiManager.UpdateAwnserHandle?.Invoke("");
            timer += timerPlusCorrectAwnserInMinutes * 60;
            SetQuestion();
        }
        else
        {
            StopCoroutine("CountdownTimer");
            UiManager.GameOverHandle?.Invoke();
        }
    }
}
