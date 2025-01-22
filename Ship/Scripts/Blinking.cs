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
	private int blinkDist = 300;
	[Export]
	private PlayerShip playerShip;

	[Export]
	private bool DirectionByMovement = false;
	private Dictionary<Vector2 , Vector2> ComponentVectors = new Dictionary<Vector2, Vector2>();
	private bool isBoosting = false;
	private int power = 15;
	private bool isAllowedToBlink = true;
	private Area2D area = new Area2D();

	private Camera2D camera;
	private Control hud;

	public  void Async_PerformBlink()
	{  	

		if (ComponentVectors.Count == 0)
		{
			find_blink_hud();
			FillDictionary();
		}
		

		Async_PreformDirectionBlink();
		
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

	
	protected void AreaEntered(Area2D area2d){
		isAllowedToBlink = false;
	}

	protected void BodyEntereds(Node2D body){
		isAllowedToBlink = false;
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
				if(!playerShip.TestMove(playerShip.Transform, new Vector2(blinkDist,0)))
				{
					
					if(isAllowedToBlink)
					{ 
						playerShip.KillMomentum();	player_tween(transForm); isBoosting = true;
						isAllowedToBlink = false;
						hud_notice(isAllowedToBlink);
					}
				}
				else
				{
					isAllowedToBlink = false;
					hud_notice(isAllowedToBlink);
				}
				
	
			}
			else if (Mathf.Abs(angle) > 0.75f * Mathf.Pi)
			{	
				transForm = playerShip.Transform.Translated(new Vector2(-blinkDist,0));
				if(!playerShip.TestMove(playerShip.Transform, new Vector2(-blinkDist,0)))
				{
				
					
					if(isAllowedToBlink)
					{
						playerShip.KillMomentum(); 	player_tween( transForm); isBoosting = true;
						isAllowedToBlink = false;
						hud_notice(isAllowedToBlink);
					}
				}
				else
				{
					isAllowedToBlink = false;
					hud_notice(isAllowedToBlink);
				}
			}
			else if (angle > 0.0f)
			{
				transForm = playerShip.Transform.Translated(new Vector2(0,blinkDist));
				if(!playerShip.TestMove(playerShip.Transform, new Vector2(0,blinkDist)))
				{
					
					if(isAllowedToBlink)
					{ 
						playerShip.KillMomentum();	player_tween( transForm); isBoosting = true;
						isAllowedToBlink = false;
						hud_notice(isAllowedToBlink);
					}
				}
				else
				{
					isAllowedToBlink = false;
					hud_notice(isAllowedToBlink);
				}
			}
			else
			{
				transForm = playerShip.Transform.Translated(new Vector2(0,-blinkDist));
				if(!playerShip.TestMove(playerShip.Transform, new Vector2(0,-blinkDist)))
				{
					if(isAllowedToBlink)
					{ 
						player_tween( transForm); isBoosting = true;
						playerShip.KillMomentum();
						isAllowedToBlink = false;
						hud_notice(isAllowedToBlink);
					}
				}
				else
				{
					isAllowedToBlink = false;
					hud_notice(isAllowedToBlink);
				}
			}
		}
		//area.QueueFree();
		await ToSignal(GetTree().CreateTimer(3),"timeout");
		isAllowedToBlink = true;
		isBoosting = false;
		hud_notice(isAllowedToBlink);
	}

	private void player_tween(Transform2D trans){
		
		Tween tween = GetTree().CreateTween().BindNode(playerShip).SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(playerShip, "transform",trans,0.10f );	
	}

	private void hud_notice(bool visible){
		hud.Visible =visible;
		
	}

	private void find_blink_hud(){
	foreach (var child in Game.Instance.Hud.GetChildren())
	{
		if (child.Name == "BlinkNotice")
		{
			hud = (Control)child;
		}
		
	}
	}
}
