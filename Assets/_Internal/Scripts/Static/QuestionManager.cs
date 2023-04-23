using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestionManager
{
    public static string Addition(int min, int max)
    {
        int num1 = Random.Range(min, max);
        int num2 = Random.Range(min, max);
        int resul = num1 + num2;
        return num1 + "/" + num2 + "/" + resul;
    }
    public static string Subtraction(int min, int max)
    {
        int num1 = Random.Range(min, max);
        int num2 = Random.Range(min, max);
        int resul = num1 - num2;
        return num1 + "/" + num2 + "/" + resul;
    }

    public static int RandomApproximately(int i)
    {
        int resul;
        do
        {
            resul = Random.Range(-1000, 1000);
        } while (resul < (i - 100) || resul > (i + 100));
         
        return resul;
    }
}
