using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersManager : MonoBehaviour
{
    [SerializeField]
    Modifier[] op = new Modifier[] {
        new Modifier("Addition",true,"20 + 10 = 30","Free",true),
        new Modifier("Subtraction",true,"20 - 10 = 10","Free",true),
        new Modifier("Multiply",false,"20 x 10 = 200","Beating level 10 with add modifier",false),
        new Modifier("Division",false,"20 / 10 = 2","Beating level 10 with multiply modifier",false)
    };
        
    [SerializeField] private GameObject ModifierPanel;
    [SerializeField] private GameObject prefabModifierPanel;

    private void Start()
    {
        for (int i = 0; i < op.Length; i++)
        {
            GameObject itemInstantiated = Instantiate(prefabModifierPanel, Vector2.zero, Quaternion.identity);
            itemInstantiated.transform.SetParent(ModifierPanel.transform);
            itemInstantiated.transform.localScale = Vector3.one;
            itemInstantiated.GetComponent<ModifierPanel>().SetOperation(ref op[i]);
        }
    }

    public void SetModifiers()
    {
        List<string> ActivesModifiers = new List<string>();
        foreach (var item in op)
        {
            if(item.Active)
            {
                ActivesModifiers.Add(item.Name);
            }
        }
        GameManager.SetModifiersHandle?.Invoke(ActivesModifiers);
    }
}
