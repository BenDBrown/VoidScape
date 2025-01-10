using Godot;
using System;
using System.Collections.Generic;

public partial class ShipComponents : ItemList
{
    [Export]
    public ShipComponentData[] datas;
    [Export]
    private Label name, health, defense, description;
    [Export]
    private BoxContainer infobox;

    private Dictionary<int, ShipComponentData> itemListRef = new();


    public override void _Ready()
    {
        Clear();
        PopulateItemList();
    }

    public void PopulateItemList()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            ShipComponentData data = datas[i];
            if (data != null)
            {
                int index = AddItem(data.Name, data.Sprite);
                itemListRef[index] = data;
            }
        }
    }

    public void OnItemSelected(int index)
    {
        infobox.Visible = true;
        ShipComponentData data = itemListRef[index];
        name.Text = data.Name;
        health.Text = "Health: " + data.MaxHealth.ToString();
        defense.Text = "Defense: " + data.Defense.ToString();
        description.Text = "Description: " + data.Description;
    }
}
