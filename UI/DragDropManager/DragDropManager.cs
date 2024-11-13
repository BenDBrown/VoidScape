using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class DragDropManager : ItemList
{
	[Export]
	public ShipComponentData[] datas;
	// [Export]
	// private Label name, health, defense, description;
	// [Export]
	// private BoxContainer infobox;
	private Dictionary<int, ShipComponentData> itemListRef = new();

	//
	[Export]
	private GridContainer gridContainer;
	private Sprite2D draggedPreview;
	private Panel[,] gridCells;
	private bool isDragging = false;
	private Vector2 initialMousePos;


	public override void _Ready()
	{
		Clear();

		PopulateItemList();

	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("click") && isDragging)
		{
			if (draggedPreview != null)
			{
				var tween = GetTree().CreateTween();
				tween.TweenProperty(draggedPreview, "global_position", initialMousePos, 0.7f);
				tween.Finished += () => draggedPreview.QueueFree();
				tween.Finished += () => isDragging = false;
				return;
			}
			isDragging = false;
		}
		else if (isDragging)
		{
			draggedPreview.Position = GetGlobalMousePosition();
		}
	}

	// When an item is selected
	public void OnItemSelected(int index)
	{
		// infobox.Visible = true;
		ShipComponentData data = itemListRef[index];
		// name.Text = data.Name;
		// health.Text = "Health: " + data.MaxHealth.ToString();
		// defense.Text = "Defense: " + data.Defense.ToString();
		// description.Text = "Description: " + data.Description;
	}


	// displaying the datas in the item list
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


	private void OnItemClicked(int index, Vector2 atPosition, int mouseButtonIndex)
	{
		if (mouseButtonIndex == (int)MouseButton.Left)
		{
			GD.Print("Item clicked at index " + index + " at position " + atPosition);
			GD.Print("pressed");
			for (int i = 0; i < GetItemCount(); i++)
			{
				Rect2 itemRect = GetItemRect(i);
				if (itemRect.HasPoint(atPosition))
				{
					ShipComponentData data = itemListRef[i];
					draggedPreview = new();
					draggedPreview.Texture = data.Sprite;
					GetTree().CurrentScene.AddChild(draggedPreview);
					draggedPreview.GlobalPosition = GetGlobalMousePosition();
					initialMousePos = GetGlobalMousePosition();
					isDragging = true;
					break;
				}
			}
		}
	}

}
