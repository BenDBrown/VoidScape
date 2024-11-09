using Godot;
using System;
using System.Collections.Generic;

public partial class ShipParts : ItemList
{
	[Export]
	public ShipComponentData[] datas;
	[Export]
	private Label name, health, defense, description;
	private Dictionary<int, ShipComponentData> itemListRef = new();
	public override void _Ready()
	{
		Clear();

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
		ShipComponentData data = itemListRef[index];
		name.Text = data.Name;
		health.Text = "Health: " + data.MaxHealth.ToString();
		defense.Text = "Defense: " + data.Defense.ToString();
		description.Text = "Description: " + data.Description;
	}
}
