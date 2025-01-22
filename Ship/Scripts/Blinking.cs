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
	public delegate void IFrameStartedEventHandler();
	[Signal]
	public delegate void BodyEnteredEventHandler(Node2D body);
	private Vector2 center = new Vector2(0, 0);
	[Export]
	private int blinkDist = 300;
	[Export]
	private PlayerShip playerShip;
	[Export]
	private bool DirectionByMovement = false;
	public int PowerDraw => GetPowerDraw();
	private Dictionary<Vector2, Vector2> ComponentVectors = new Dictionary<Vector2, Vector2>();
	private bool isBoosting = false;
	private int power = 15;
	private bool isAllowedToBlink = true;

	private Control hud;
	private Timer time;
	private Timer BlinkTimer;

	public void Async_PerformBlink(Vector2 direction)
	{

		if (ComponentVectors.Count == 0)
		{
			FindBlinkHud();
			FillDictionary();
		}


		Async_PreformDirectionBlink(direction);

	}

	public int GetPowerDraw()
	{
		if (isBoosting) {; return power; }
		return 0;
	}

	private void FillDictionary()
	{
		foreach (var childer in playerShip.GetChildren())
		{
			if (childer is ShipComponent component)
			{
				foreach (var child in component.GetChildren())
				{
					if (child is Area2D)
					{
						Area2D Area2d = (Area2D)child;
						CollisionShape2D CS = (CollisionShape2D)Area2d.GetChild(0);
						RectangleShape2D RS = CS.GetShape() as RectangleShape2D;
						var pos = component.Position;
						var upRight = new Vector2(pos.X + (RS.Size.X / 2), pos.Y + (RS.Size.Y / 2));
						var downLeft = new Vector2(pos.X + (RS.Size.X / 2), pos.Y + (RS.Size.Y / 2));
						var upLeft = new Vector2(pos.X + (RS.Size.X / 2), pos.Y + (RS.Size.Y / 2));
						var downRight = new Vector2(pos.X + (RS.Size.X / 2), pos.Y + (RS.Size.Y / 2));

						if (pos.X >= 0 && pos.Y >= 0)
						{
							ComponentVectors.Add(downRight, Vector2.Zero);
						}
						else if (pos.X <= 0 && pos.Y >= 0)
						{

							ComponentVectors.Add(downLeft, Vector2.Zero);
						}
						else if (pos.X <= 0 && pos.Y <= 0)
						{
							ComponentVectors.Add(upLeft, Vector2.Zero);
						}
						else if (pos.X >= 0 && pos.Y <= 0)
						{
							ComponentVectors.Add(upRight, Vector2.Zero);
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


	protected void AreaEntered(Area2D area2d)
	{
		isAllowedToBlink = false;
	}

	protected void BodyEntereds(Node2D body)
	{
		isAllowedToBlink = false;
	}

	private void Async_PreformDirectionBlink(Vector2 direction)
	{
		if(BlinkTimer == null){
			BlinkTimer = new();
			BlinkTimer.OneShot = true;
			BlinkTimer.Timeout += ()=> Cooldown();
			AddChild(BlinkTimer);
				
		}
		if(!BlinkTimer.IsStopped()){
			return;
		}

		Transform2D transForm;

		float angleDeg = ConvertRadiansToDegrees(direction.Angle());
		if (angleDeg == 0)
		{
			

			transForm = playerShip.Transform.TranslatedLocal(new Vector2(blinkDist, 0));
			if (!playerShip.TestMove(playerShip.Transform, new Vector2(blinkDist, 0)))
			{

				if (isAllowedToBlink)
				{	
					GD.Print("blinking");
					Blinked(transForm);
				}
			}
			else
			{
				isAllowedToBlink = false;
				HudNotice(isAllowedToBlink);
			}


		}
		else if (angleDeg == 180)
		{
			transForm = playerShip.Transform.TranslatedLocal(new Vector2(-blinkDist, 0));
			if (!playerShip.TestMove(playerShip.Transform, new Vector2(-blinkDist, 0)))
			{


				if (isAllowedToBlink)
				{
					Blinked(transForm);
				}
			}
			else
			{
				isAllowedToBlink = false;
				HudNotice(isAllowedToBlink);
			}
		}
		else if (angleDeg == 90)
		{
			transForm = playerShip.Transform.TranslatedLocal(new Vector2(0, blinkDist));
			if (!playerShip.TestMove(playerShip.Transform, new Vector2(0, blinkDist)))
			{

				if (isAllowedToBlink)
				{
					Blinked(transForm);

				}
				else
				{
					isAllowedToBlink = false;
					HudNotice(isAllowedToBlink);
				}
			}
		}
		else
			{
				transForm = playerShip.Transform.TranslatedLocal(new Vector2(0, -blinkDist));
				if (!playerShip.TestMove(playerShip.Transform, new Vector2(0, -blinkDist)))
				{
					if (isAllowedToBlink)
					{
						Blinked(transForm);
					}
				}
				else
				{
					isAllowedToBlink = false;
					HudNotice(isAllowedToBlink);
				}
			}

			
			BlinkTimer.Start(3);
			
			
		}
	

	private void PlayerTween(Transform2D trans)
	{

		Tween tween = GetTree().CreateTween().BindNode(playerShip).SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(playerShip, "transform", trans, 0.10f);

	}

	private void HudNotice(bool visible)
	{
		hud.Visible = visible;

	}

	private void FindBlinkHud()
	{
		foreach (var child in Game.Instance.Hud.GetChildren())
		{
			if (child.Name == "BlinkNotice")
			{
				hud = (Control)child;
			}

		}
	}

	private static float ConvertRadiansToDegrees(double radians)
	{
		double degrees = (180 / Math.PI) * radians;
		return (float)(degrees);
	}

	private void IFrame()
	{
		if (time == null)
		{
			time = new();
			time.OneShot = true;
			AddChild(time);
			foreach (ShipComponent c in playerShip.shipParts)
			{
				Node health = null;
				foreach (Node h in c.GetChildren())
				{
					if (h.Name == "HealthComponent")
					{
						health = h;
						break;
					}
				}
				if (health == null)
				{
					continue;
				}
				IFrameStarted += ()=> HealthSet(health);
			time.Timeout += () => DisableIFrame(health);

			}
		}
		if (!time.IsStopped())
		{
			return;
		}
		
		time.Start(2);
	}

	private void DisableIFrame(Node health)
	{
		health.Set("i_bool", false);
	}

	private void HealthSet(Node health){
		health.Set("i_bool", true);
	}

	private void Blinked(Transform2D transForm)
	{
		IFrame();
		playerShip.KillMomentum();
		PlayerTween(transForm);
		isBoosting = true;
		isAllowedToBlink = false;
		HudNotice(isAllowedToBlink);
	}

	private void Cooldown(){
		isAllowedToBlink = true;
		isBoosting = false;
		HudNotice(isAllowedToBlink);
	}

}
