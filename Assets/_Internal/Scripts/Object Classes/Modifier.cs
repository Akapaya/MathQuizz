using System;
using UnityEngine;
[Serializable]
public class Modifier 
{
    [SerializeField] string name;
    [SerializeField] bool unlocked;
    [SerializeField] string example;
    [SerializeField] string howUnlock;
    [SerializeField] bool active;

    public string Name { get => name; set => name = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public string Example { get => example; set => example = value; }
    public string HowUnlock { get => howUnlock; set => howUnlock = value; }
    public bool Active { get => active; set => active = value; }

    public Modifier(string name, bool unlocked, string example, string howUnlock, bool active)
    {
        this.Name = name;
        this.Unlocked = unlocked;
        this.Example = example;
        this.HowUnlock = howUnlock;
        this.Active = active;
    }
}
