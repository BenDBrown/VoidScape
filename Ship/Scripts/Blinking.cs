using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class Blinking : Node2D
{
	[Signal]
	public delegate void BlinkEventHandler(int power);

	[Signal]
	public delegate void BodyEnteredEventHandler(Node2D body);
	private Vector2 center = new Vector2(0,0);

	[Export]
	private int blinkDist = 150;
	[Export]
	private PlayerShip playerShip;

	[Export]
	private bool DirectionByMovement = false;
	private Dictionary<Vector2 , Vector2> ComponentVectors = new Dictionary<Vector2, Vector2>();
	private bool isBoosting = false;
	private int power = 15;
	private bool isAllowedToBlink = true;
	private Area2D area = new Area2D();

	public async void Async_PerformBlink()
	{  	

		if (ComponentVectors.Count == 0)
		{
			FillDictionary();
		}

		
		Async_PreformDirectionBlink();
		
		await ToSignal(GetTree().CreateTimer(1),"timeout");
		isAllowedToBlink = true;
		isBoosting = false;
		}

	public int GetPowerDraw()
	{
		if(isBoosting){ ; return power;}
		return 0;
	}

	private void FillDictionary()
	{
		foreach (var childer in playerShip.GetChildren())
		{
			if (childer is ShipComponent component)
			{
				foreach(var child in component.GetChildren())
				{ 
					if(child is Area2D)
					{
						Area2D Area2d = (Area2D)child;
						CollisionShape2D CS = (CollisionShape2D)Area2d.GetChild(0);
						RectangleShape2D RS = CS.GetShape() as RectangleShape2D;
						var pos  = component.Position;
						var upRight  = new Vector2(pos.X +(RS.Size.X/2), pos.Y + (RS.Size.Y/2));
						var downLeft = new Vector2(pos.X +(RS.Size.X/2), pos.Y + (RS.Size.Y/2));
						var upLeft= new Vector2(pos.X +(RS.Size.X/2), pos.Y + (RS.Size.Y/2));
						var downRight= new Vector2(pos.X +(RS.Size.X/2), pos.Y + (RS.Size.Y/2));

						if(pos.X >= 0 && pos.Y >= 0)
						{
							ComponentVectors.Add(downRight,Vector2.Zero);
						}
						else if(pos.X <= 0 && pos.Y >= 0)
						{
							
							ComponentVectors.Add(downLeft,Vector2.Zero);
						}
						else if(pos.X <= 0 && pos.Y <= 0)
						{
							ComponentVectors.Add(upLeft,Vector2.Zero);
						}
						else if(pos.X >= 0 && pos.Y <= 0)
						{
							ComponentVectors.Add(upRight,Vector2.Zero);
						}
					}
				}
			}
		}
		
	}

	private ConvexPolygonShape2D CreateConvexPolygon()
	{
		Vector2[] points = new Vector2[ComponentVectors.Count];
		var convexPolygon = new ConvexPolygonShape2D();
		int i = 0;

		foreach (var item in ComponentVectors)
		{
			points[i] = item.Key;
			i++;
		}
		convexPolygon.SetPointCloud(points);
		return convexPolygon;
	}

	private bool AllowedToBlink(Transform2D transform)
	{
		if (!isAllowedToBlink) {return false;}
		area = new Area2D();
		var col = new CollisionShape2D();
		col.Shape = CreateConvexPolygon();
		area.AddChild(col);
		GetParent().AddChild(area);
		area.Monitoring = true;
		area.AreaEntered += AreaEntered;
		area.BodyEntered += BodyEntereds;
		area.Transform = area.Transform.Translated(new Vector2(0,-(blinkDist*2)));
		return isAllowedToBlink;
		
		
	}

	protected void AreaEntered(Area2D area2d){
		isAllowedToBlink = false;
		area.QueueFree();
	}

	protected void BodyEntereds(Node2D body){
		isAllowedToBlink = false;
		area.QueueFree();
	}

	private async void Async_PreformDirectionBlink() 
	{
		Transform2D transForm;
		area = new Area2D();
		if (playerShip.Velocity.Length() > 0.1f)
		{
			float angle = Mathf.Atan2(playerShip.Velocity.Y, playerShip.Velocity.X); // angle in [-PI, PI]
			if (Mathf.Abs(angle) < 0.25f * Mathf.Pi)
			{

				transForm = playerShip.Transform.Translated(new Vector2(blinkDist,0));
				if(AllowedToBlink(transForm))
				{
					
					await ToSignal(GetTree().CreateTimer(0.05),"timeout");
					if(isAllowedToBlink){ 
					playerShip.KillMomentum();
					playerShip.Transform = transForm; isBoosting = true;
					}
				}
			}
			else if (Mathf.Abs(angle) > 0.75f * Mathf.Pi)
			{	
				transForm = playerShip.Transform.Translated(new Vector2(-blinkDist,0));
				if(AllowedToBlink(transForm))
				{
					await ToSignal(GetTree().CreateTimer(0.05),"timeout");
					if(isAllowedToBlink){
					playerShip.KillMomentum(); 
					playerShip.Transform = transForm; isBoosting = true;
					}
				}
			}
			else if (angle > 0.0f)
			{
				transForm = playerShip.Transform.Translated(new Vector2(0,blinkDist));
				if(AllowedToBlink(transForm))
				{
					await ToSignal(GetTree().CreateTimer(0.05),"timeout");
					if(isAllowedToBlink){ 
					playerShip.KillMomentum();
					playerShip.Transform = transForm; isBoosting = true;
					}
				}
			}
			else
			{
				transForm = playerShip.Transform.Translated(new Vector2(0,-blinkDist));
				if(AllowedToBlink(transForm))
				{
					await ToSignal(GetTree().CreateTimer(0.05),"timeout");
					if(isAllowedToBlink){ 
					playerShip.KillMomentum();
					playerShip.Transform = transForm; isBoosting = true;
					}
				}
			}
		}
		area.QueueFree();
		await ToSignal(GetTree().CreateTimer(1),"timeout");
		isAllowedToBlink = true;
		isBoosting = false;
	}
}
