using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModifierPanel : MonoBehaviour
{
    [SerializeField] TMP_Text operationName;
    [SerializeField] TMP_Text example;
    [SerializeField] Toggle toggleActive;
    [SerializeField] Image lockImage;

    [SerializeField] Modifier modifier;

    public void SetOperation(ref Modifier modifier)
    {
        this.modifier = modifier;
        this.operationName.text = modifier.Name;
        if (modifier.Unlocked)
        {
            this.example.text = modifier.Example;
            toggleActive.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            this.example.text = modifier.HowUnlock;
        }
    }

    public void SetActiveModifier()
    {
        modifier.Active = toggleActive.isOn;
    }
}
