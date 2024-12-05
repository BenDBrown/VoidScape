using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class DragDropManager : ItemList
{
	[Export]
	public ShipComponentData[] datas;
	[Export]
	private Label name, health, defense, description;
	[Export]
	private BoxContainer infobox;
	[Export]
	private GridGenerator gridGenerator;
	[Export]
	private Control menu;

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
		List<GridTile> gridSquares = gridGenerator.GenerateGrid();
		foreach (TextureRect cr in gridSquares)
		{
			cr.MouseEntered += () => MouseEnteredSquare(cr);
			cr.MouseExited += MouseExitedSquare;
		}
	}

	public override void _Process(double delta)
	{
		RotatePiece();
		DropPiece();
	}

	// When an item is selected
	public void OnItemSelected(int index)
	{
		infobox.Visible = true;
		ShipComponentData data = itemListRef[index];
		name.Text = data.Name;
		health.Text = "Health: " + data.MaxHealth.ToString();
		defense.Text = "Defense: " + data.Defense.ToString();
		description.Text = "Description: " + data.Description;
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
					menu.AddChild(draggedPreview);
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

	// method that rotates the pieces
	private void RotatePiece()
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
	}

	// method to place the piece on the grid (DROP)
	private void DropPiece()
	{
		if (Input.IsActionJustReleased("click") && isDragging)
		{
			if (draggedPreview == null)
			{
				isDragging = false;
				return;
			}
			if (hoveredRect == null || gridGenerator.GridCells[rectPos].Texture == gridGenerator.InvalidCell)
			{
				var tween = GetTree().CreateTween();
				tween.TweenProperty(draggedPreview, "global_position", initialMousePos, 0.7f);
				tween.Finished += () => ResetPiece();
				return;
			}
			else
			{
				if (gridGenerator.GridCells[rectPos].IsValid || gridGenerator.GridCells[rectPos].Texture == gridGenerator.FreeCell)
				{
					gridGenerator.ChangeCellText(hoveredRect, draggedPreview.Texture);
					gridGenerator.GridCells[rectPos].ChangeComponent(true);
					Piece piece = new Piece(preview, rectPos, isMirrored, currentRotation);
					pieces.Add(piece);
					ResetPiece();

				}
				else if (gridGenerator.GridCells[rectPos].IsValid || gridGenerator.GridCells[rectPos].HasComponent)
				{
					for (int i = 0; i < pieces.Count; i++)
					{
						if (pieces[i].Coordinate == gridGenerator.GetCellAt(hoveredRect))
						{
							pieces.Remove(pieces[i]);
							gridGenerator.ChangeCellText(hoveredRect, draggedPreview.Texture);
							Piece piece = new Piece(preview, rectPos, isMirrored, currentRotation);
							pieces.Add(piece);
							ResetPiece();
						}
					}
				}
				UpdateAvailability();
				return;
			}
		}
		else if (isDragging)
		{
			draggedPreview.Position = GetGlobalMousePosition();
		}
	}
	private void UpdateAvailability()
	{
		bool noComponents = true;
		if (pieces.Count > 0) noComponents = false;

		if (noComponents)
		{
			foreach (GridTile tile in gridGenerator.GridCells.Values)
			{
				tile.ChangeValidity(true);
			}
		}
		else if (!noComponents)
		{
			CheckAvailability();
			foreach (Vector2I coord in gridGenerator.GridCells.Keys)
			{
				if (gridGenerator.GridCells[coord].IsValid)
				{
					if (gridGenerator.GridCells[coord].HasComponent) { continue; }
					else if (!gridGenerator.GridCells[coord].HasComponent) { gridGenerator.GridCells[coord].Texture = gridGenerator.ValidCell; }
				}
				else if (!gridGenerator.GridCells[coord].IsValid)
				{
					if (gridGenerator.GridCells[coord].HasComponent) { continue; }
					else { gridGenerator.GridCells[coord].Texture = gridGenerator.InvalidCell; }
				}
			}
		}
	}

	private void CheckAvailability()
	{
		foreach (Vector2I coord in gridGenerator.GridCells.Keys)
		{
			Vector2I vRight = new(coord.X + 1, coord.Y);
			Vector2I vLeft = new(coord.X - 1, coord.Y);
			Vector2I vUp = new(coord.X, coord.Y - 1);
			Vector2I vDown = new(coord.X, coord.Y + 1);

			foreach (Piece p in pieces)
			{
				if (gridGenerator.GridCells.ContainsKey(vRight))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && p.ComponentData.RightAttachable) { gridGenerator.GridCells[vRight].ChangeValidity(true); }
				}
				if (gridGenerator.GridCells.ContainsKey(vLeft))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && p.ComponentData.LeftAttachable) { gridGenerator.GridCells[vLeft].ChangeValidity(true); }
				}
				if (gridGenerator.GridCells.ContainsKey(vDown))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && p.ComponentData.BottomAttachable) { gridGenerator.GridCells[vDown].ChangeValidity(true); }
				}
				if (gridGenerator.GridCells.ContainsKey(vUp))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && p.ComponentData.TopAttachable) { gridGenerator.GridCells[vUp].ChangeValidity(true); }
				}
			}
		}
	}

	private void ResetPiece()
	{
		draggedPreview.QueueFree();
		currentRotation = 0;
		isDragging = false;
		isMirrored = false;
	}

	private void OnBuildPressed()
	{
		var resolve = Game.Instance.BuildShip(pieces.ToArray());
		GD.Print(resolve);
	}

}
