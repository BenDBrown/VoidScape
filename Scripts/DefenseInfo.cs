using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DefenseInfo : Node
{
    [Export]
    public int maxHealth {get; set;}

    public int currentHealth {get; set;}

    [Export]
    public int defense {get; set;}

    public Dictionary<string, object> GetInfo()
    {
        Dictionary<string, object> info = new();
        info.Add(nameof(maxHealth), maxHealth);
        info.Add(nameof(currentHealth), currentHealth);
        info.Add(nameof(defense), defense);
        return info;
    }

    public void SetInfo(Dictionary<string, object> info)
    {
        foreach(string fieldName in typeof(DefenseInfo).GetFields().Select(f => f.Name).ToList())
        {
            if(fieldName == nameof(maxHealth)) 
            { 
                try
                {
                    maxHealth = (int)info[fieldName];
                }
                catch (ArgumentNullException e)
                {
                    GD.Print("field name of '" + fieldName + "' could not be found in info dictionary");
                    GD.Print("info dictionary: " + info.ToString());
                    GD.Print(e.GetType() + "\n"  + e.Message);
                }
            }
            else if(fieldName == nameof(currentHealth))
            {
                try
                {
                    currentHealth = (int)info[fieldName];
                }
                catch (ArgumentNullException e)
                {
                    GD.Print("field name of '" + fieldName + "' could not be found in info dictionary");
                    GD.Print("info dictionary: " + info.ToString());
                    GD.Print(e.GetType() + "\n"  + e.Message);
                }
            }
            else if(fieldName == nameof(defense))
            {
                try
                {
                    defense = (int)info[fieldName];
                }
                catch (ArgumentNullException e)
                {
                    GD.Print("field name of '" + fieldName + "' could not be found in info dictionary");
                    GD.Print("info dictionary: " + info.ToString());
                    GD.Print(e.GetType() + "\n"  + e.Message);
                }
            }
        }
    }

    public override void _Ready()
    {
        currentHealth = maxHealth;
    }

    public override string ToString()
    {
        return "max health: " + maxHealth + ", current health: " + currentHealth + ", defense: " + defense;
    }

}