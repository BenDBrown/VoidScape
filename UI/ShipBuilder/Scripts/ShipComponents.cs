using Godot;
using System;
using System.Collections.Generic;

public partial class ShipComponents : ItemList
{
    [Export]
    private Label name, health, defense, description;
    [Export]
    private BoxContainer infobox;
    public Dictionary<int, ShipComponentData> ItemListRefData { get; private set; } = new();

    public void PopulateItemList(ShipComponentData[] datas)
    {
        Clear();
        ItemListRefData.Clear();
        for (int i = 0; i < datas.Length; i++)
        {
            ShipComponentData data = datas[i];
            if (data != null)
            {
                int index = AddItem(data.Name, data.Sprite);
                ItemListRefData.Add(index, data);
            }
        }
    }

    public void OnItemSelected(int index)
    {
        infobox.Visible = true;
        ShipComponentData data = ItemListRefData[index];
        name.Text = data.Name;
        health.Text = "Health: " + data.MaxHealth.ToString();
        defense.Text = "Defense: " + data.Defense.ToString();
        description.Text = "Description: " + data.Description;
    }

    public void OnItemClicked(int index, Vector2 atPosition, int mouseButtonIndex)
    {
        if (mouseButtonIndex == (int)MouseButton.Left)
        {
            GD.Print("Item name: " + ItemListRefData[index].Name);
        }

    }
}
