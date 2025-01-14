using Godot;
using System;
using System.Collections.Generic;

public partial class ShipBuilderManager : Control
{
	private const string ROTATE_RIGHT_ACTION_NAME = "rotate_part_right";
	private const string ROTATE_LEFT_ACTION_NAME = "rotate_part_left";
	private const string MIRROR_ACTION_NAME = "mirror_part";
	private const string RIGHT_CLICK_ACTION_NAME = "right_click";


	[Export]
	public ShipComponents itemList;

	[Export]
	public GridGenerator gridGenerator;

	[Export]
	public ShipStats shipStats;

	[Export]
	public ColorPickerButton colourpicker;


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
	private bool canDelete = false;
	private Piece deletedPiece;
	private Color pieceColour = new(0.51f, 0.502f, 0.486f, 1);
	private GridTile[] gridSquares;




	public override void _Ready()
	{
		itemList.PopulateItemList();
		shipStats.PopulateList();
		gridSquares = gridGenerator.GenerateGrid();
		foreach (TextureRect cr in gridSquares)
		{
			cr.MouseEntered += () => MouseEnteredSquare(cr);
			cr.MouseExited += MouseExitedSquare;
		}
		gridGenerator.CallDeferred("LoadShip");
		pieceColour = new(0.51f, 0.502f, 0.486f, 1);
	}

	public override void _Process(double delta)
	{
		RotatePiece();
		DropPiece();
		//RemovePiece();
	}

	public void OnColourChanged(Color colour)
	{
		foreach (GridTile gt in gridSquares)
		{
			if (gt.Material is not ShaderMaterial shaderMat) return;
			shaderMat.SetShaderParameter("color", colour);
			pieceColour = colour;
		}
	}

	private void OnItemClicked(int index, Vector2 atPosition, int mouseButtonIndex)
	{
		if (mouseButtonIndex == (int)MouseButton.Left)
		{
			if (isDragging) return;
			for (int i = 0; i < itemList.GetItemCount(); i++)
			{
				Rect2 itemRect = itemList.GetItemRect(i);
				if (itemRect.HasPoint(atPosition))
				{
					ShipComponentData data = itemList.ItemListRef[i];
					draggedPreview = new();
					preview = data;
					draggedPreview.Texture = data.Sprite;

					//ignore for your own sake
					Node parent = GetParent();
					for (int j = 0; j < 20; j++)
					{
						if (parent == null) break;
						if (parent is CanvasLayer) { parent.AddChild(draggedPreview); break; }
						GD.Print(parent.Name);
						parent = parent.GetParent();
					}

					draggedPreview.GlobalPosition = GetGlobalMousePosition();
					initialMousePos = GetGlobalMousePosition();
					isDragging = true;
					break;
				}
			}
		}
	}

	private void MouseEnteredSquare(TextureRect colorRect)
	{
		hoveredRect = colorRect;
		rectPos = gridGenerator.GetCellAt(colorRect);

		if (draggedPreview == null)
		{
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].Coordinate == (Vector2)rectPos)
				{
					canDelete = true;
					deletedPiece = pieces[i];
				}
			}
		}
	}

	private void MouseExitedSquare()
	{
		hoveredRect = null;
		deletedPiece = null;
		canDelete = false;
	}

	private void DropPiece()
	{
		if (Input.IsActionJustReleased("click") && isDragging)
		{
			if (draggedPreview == null)
			{
				isDragging = false;
				return;
			}
			if (hoveredRect == null || gridGenerator.GridCells[rectPos].Texture == gridGenerator.InvalidCell || gridGenerator.GridCells[rectPos].HasComponent)
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
					gridGenerator.GridCells[rectPos].HasComponent = true;
					Piece piece = new Piece(preview, rectPos, isMirrored, currentRotation, pieceColour);
					pieces.Add(piece);
					ResetPiece();
					shipStats.UpdateStats(pieces);

				}
				else if (gridGenerator.GridCells[rectPos].IsValid /*|| gridGenerator.GridCells[rectPos].HasComponent*/)
				{
					for (int i = 0; i < pieces.Count; i++)
					{
						if (pieces[i].Coordinate == gridGenerator.GetCellAt(hoveredRect))
						{
							pieces.Remove(pieces[i]);
							gridGenerator.ChangeCellText(hoveredRect, draggedPreview.Texture);
							Piece piece = new Piece(preview, rectPos, isMirrored, currentRotation, pieceColour);
							pieces.Add(piece);
							ResetPiece();
							shipStats.UpdateStats(pieces);
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
		if (!noComponents)
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
		else
		{
			foreach (GridTile tile in gridGenerator.GridCells.Values)
			{
				tile.IsValid = false;
				tile.Texture = gridGenerator.FreeCell;
			}
		}
	}

	private void RotatePiece()
	{
		if (Input.IsActionJustPressed(ROTATE_RIGHT_ACTION_NAME) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.Rotate90(ClockDirection.Clockwise);
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			currentRotation += 90;
		}
		else if (Input.IsActionJustPressed(ROTATE_LEFT_ACTION_NAME) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.Rotate90(ClockDirection.Counterclockwise);
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			currentRotation -= 90;
		}
		else if (Input.IsActionJustPressed(MIRROR_ACTION_NAME) && isDragging)
		{
			var image = draggedPreview.Texture.GetImage();
			image.FlipX();
			draggedPreview.Texture = ImageTexture.CreateFromImage(image);
			isMirrored = true;
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
				(bool top, bool right, bool bottom, bool left) attachable = GetAttachableSides(p);

				if (gridGenerator.GridCells.ContainsKey(vRight))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && attachable.right) { gridGenerator.GridCells[vRight].IsValid = true; }
				}
				if (gridGenerator.GridCells.ContainsKey(vLeft))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && attachable.left) { gridGenerator.GridCells[vLeft].IsValid = true; }
				}
				if (gridGenerator.GridCells.ContainsKey(vDown))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && attachable.bottom) { gridGenerator.GridCells[vDown].IsValid = true; }
				}
				if (gridGenerator.GridCells.ContainsKey(vUp))
				{
					if (p.Coordinate == gridGenerator.GetCellAt(gridGenerator.GridCells[coord]) && attachable.top) { gridGenerator.GridCells[vUp].IsValid = true; }
				}
			}
		}
	}


	private (bool top, bool right, bool bottom, bool left) GetAttachableSides(Piece p)
	{
		bool flippedX = p.IsMirrored;
		float normalizedRotation = p.LocalRotation % 360;
		if (normalizedRotation < 0) normalizedRotation += 360;

		switch (normalizedRotation, flippedX)
		{
			case (0, false):
				return (p.ComponentData.TopAttachable, p.ComponentData.RightAttachable, p.ComponentData.BottomAttachable, p.ComponentData.LeftAttachable);

			case (0, true):
				return (p.ComponentData.TopAttachable, p.ComponentData.LeftAttachable, p.ComponentData.BottomAttachable, p.ComponentData.RightAttachable);

			case (90, false):
				return (p.ComponentData.LeftAttachable, p.ComponentData.TopAttachable, p.ComponentData.RightAttachable, p.ComponentData.BottomAttachable);

			case (90, true):
				return (p.ComponentData.LeftAttachable, p.ComponentData.BottomAttachable, p.ComponentData.RightAttachable, p.ComponentData.TopAttachable);

			case (180, false):
				return (p.ComponentData.BottomAttachable, p.ComponentData.LeftAttachable, p.ComponentData.TopAttachable, p.ComponentData.RightAttachable);

			case (180, true):
				return (p.ComponentData.BottomAttachable, p.ComponentData.RightAttachable, p.ComponentData.TopAttachable, p.ComponentData.LeftAttachable);

			case (270, false):
				return (p.ComponentData.RightAttachable, p.ComponentData.BottomAttachable, p.ComponentData.LeftAttachable, p.ComponentData.TopAttachable);

			case (270, true):
				return (p.ComponentData.RightAttachable, p.ComponentData.TopAttachable, p.ComponentData.LeftAttachable, p.ComponentData.BottomAttachable);


			default:
				throw new InvalidOperationException("Invalid rotation value.");
		}
	}

	// private void RemovePiece()
	// {
	// 	if (canDelete && Input.IsActionJustPressed(RIGHT_CLICK_ACTION_NAME))
	// 	{
	// 		if (pieces.Count > 0)
	// 		{
	// 			for (int i = 0; i < pieces.Count; i++)
	// 			{
	// 				if (deletedPiece == pieces[i])
	// 				{
	// 					pieces.Remove(deletedPiece);
	// 					gridGenerator.GridCells[(Vector2I)deletedPiece.Coordinate].HasComponent = false;
	// 					gridGenerator.GridCells[(Vector2I)deletedPiece.Coordinate].IsValid = false;
	// 				}
	// 				break;
	// 			}
	// 		}
	// 		deletedPiece = null;
	// 		UpdateAvailability();

	// 	}
	// }

	private void OnRemoveAllPressed()
	{
		pieces.Clear();
		foreach (GridTile gt in gridGenerator.GridCells.Values)
		{
			gt.IsValid = false;
			gt.HasComponent = false;
			gt.Texture = gridGenerator.FreeCell;
		}

	}

	private void ResetPiece()
	{
		draggedPreview.QueueFree();
		draggedPreview = null;
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
