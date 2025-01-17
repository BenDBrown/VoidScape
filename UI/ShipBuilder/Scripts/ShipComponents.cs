using Godot;
using System;
using System.Collections.Generic;

public partial class ShipComponents : ItemList
{
    [Export]
    private ShipComponentData[] datas;
    [Export]
    private Label name, health, defense, description;
    [Export]
    private BoxContainer infobox;

    public Dictionary<int, ShipComponentData> ItemListRef { get; private set; } = new();


    public void PopulateItemList()
    {
        Clear();
        for (int i = 0; i < datas.Length; i++)
        {
            ShipComponentData data = datas[i];
            if (data != null)
            {
                int index = AddItem(data.Name, data.Sprite);
                ItemListRef[index] = data;
            }
        }
    }

    public void OnItemSelected(int index)
    {
        infobox.Visible = true;
        ShipComponentData data = ItemListRef[index];
        name.Text = data.Name;
        health.Text = "Health: " + data.MaxHealth.ToString();
        defense.Text = "Defense: " + data.Defense.ToString();
        description.Text = "Description: " + data.Description;
    }
}
