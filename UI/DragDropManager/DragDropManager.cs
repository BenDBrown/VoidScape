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
	[Export]
	private GridGenerator gridGenerator;

	private Dictionary<int, ShipComponentData> itemListRef = new();
	private List<Piece> pieces = new();
	private ShipComponentData preview;
	private Sprite2D draggedPreview;
	private bool isDragging = false;
	private Vector2 initialMousePos;
	private TextureRect hoveredRect = null;
	private Vector2I rectPos;
	private bool isMirrored = false;
	private int currentRotation = 0;

	private const string rotateRightActionName = "rotate_part_right";
	private const string rotateLeftActionName = "rotate_part_left";
	private const string mirrorActionName = "mirror_part";


	public override void _Ready()
	{
		Clear();
		PopulateItemList();
		List<TextureRect> gridSquares = gridGenerator.GenerateGrid();
		foreach (TextureRect cr in gridSquares)
		{
			cr.MouseEntered += () => MouseEnteredSquare(cr);
			cr.MouseExited += MouseExitedSquare;
		}
	}

	public override void _Process(double delta)
	{

		if (Input.IsActionJustPressed(rotateRightActionName) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.Rotate90(ClockDirection.Clockwise);
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			currentRotation += 90;
		}
		else if (Input.IsActionJustPressed(rotateLeftActionName) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.Rotate90(ClockDirection.Counterclockwise);
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			currentRotation -= 90;
		}
		else if (Input.IsActionJustPressed(mirrorActionName) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.FlipX();
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			isMirrored = true;
		}


		if (Input.IsActionJustReleased("click") && isDragging)
		{
			if (draggedPreview == null)
			{
				isDragging = false;
				return;
			}
			if (hoveredRect == null)
			{
				var tween = GetTree().CreateTween();
				tween.TweenProperty(draggedPreview, "global_position", initialMousePos, 0.7f);
				tween.Finished += () => ResetPiece();
				return;
			}
			else
			{
				if (hoveredRect.Texture == gridGenerator.GetFreeCell)
				{
					gridGenerator.ChangeCellText(hoveredRect, draggedPreview.Texture);
					pieces.Add(new Piece(preview, rectPos, isMirrored, currentRotation));
					ResetPiece();
				}
				else if (hoveredRect.Texture != gridGenerator.GetFreeCell)
				{
					GD.Print(gridGenerator.GetCellAt(hoveredRect));
					for (int i = 0; i < pieces.Count; i++)
					{
						if (pieces[i].Coordinate == gridGenerator.GetCellAt(hoveredRect))
						{
							pieces.Remove(pieces[i]);
							gridGenerator.ChangeCellText(hoveredRect, draggedPreview.Texture);
							pieces.Add(new Piece(preview, rectPos, isMirrored, currentRotation));
							GD.Print(currentRotation);
							ResetPiece();
						}
					}
				}
				return;
			}
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

	// when selecting an item form the item list, a preview of the part is going to appear
	private void OnItemClicked(int index, Vector2 atPosition, int mouseButtonIndex)
	{
		if (mouseButtonIndex == (int)MouseButton.Left)
		{
			if (isDragging) return;
			for (int i = 0; i < GetItemCount(); i++)
			{
				Rect2 itemRect = GetItemRect(i);
				if (itemRect.HasPoint(atPosition))
				{
					ShipComponentData data = itemListRef[i];
					draggedPreview = new();
					preview = data;
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

	// get the cell that you are hovering
	private void MouseEnteredSquare(TextureRect colorRect)
	{
		hoveredRect = colorRect;
		rectPos = gridGenerator.GetCellAt(colorRect);
	}

	private void MouseExitedSquare() => hoveredRect = null;


	private void OnBuildPressed()
	{
		GD.Print("NEW------------");
		foreach (Piece p in pieces)
		{
			GD.Print("//");
			GD.Print("Piece name: " + p.ComponentData.Name);
			GD.Print("Piece coordinate: " + p.Coordinate);
			GD.Print("Piece mirrored: " + p.IsMirrored);
			GD.Print("Piece local rotation: " + p.LocalRotation);
		}
	}

	private void ResetPiece()
	{
		draggedPreview.QueueFree();
		currentRotation = 0;
		isDragging = false;
		isMirrored = false;
	}


}
