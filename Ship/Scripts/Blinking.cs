using Godot;
using System;

public partial class Blinking : Node2D
{
	[Signal]
	public delegate void BlinkedEventHandler(int power);
	[Signal]
	public delegate void IFrameStartedEventHandler();
	private Vector2 center = new Vector2(0, 0);
	[Export]
	private int blinkDist = 300;
	[Export]
	private PlayerShip playerShip;
	[Export]
	private bool DirectionByMovement = false;
	[Export]
	private RayCast2D rayCast2D;

	public int PowerDraw => GetPowerDraw();
	private bool isBoosting = false;
	private int power = 15;
	private bool isAllowedToBlink = true;

	private Control blinkHUD;
	private Timer iFrameTimer;
	private Timer BlinkTimer;

	public override void _Ready()
	{
		base._Ready();
		blinkHUD = Game.Instance.Hud.BlinkHUD;
		CreateBlinkTimer();
		CreateIFrameTimer();
	}
	public void PerformBlink(Vector2 direction)
	{
		if (direction == Vector2.Zero) direction = Vector2.Up;
		if (BlinkTimer == null) CreateBlinkTimer();
		if (!BlinkTimer.IsStopped()) return;

		rayCast2D.TargetPosition = direction * blinkDist;
		if (!rayCast2D.IsColliding())
		{
			Blink(direction);
		}
		BlinkTimer.Start(3);
	}

	public int GetPowerDraw()
	{
		return isBoosting ? power : 0;
	}

	private static float ConvertRadiansToDegrees(double radians) => (float)(180 / Math.PI * radians);

	private void IFrame()
	{
		if (iFrameTimer == null) CreateIFrameTimer();

		if (iFrameTimer.IsStopped())
		{
			iFrameTimer.Start(0.5);
		}
	}

	private void CreateIFrameTimer()
	{
		iFrameTimer = new();
		iFrameTimer.OneShot = true;
		AddChild(iFrameTimer);
		foreach (ShipComponent c in playerShip.ShipComponents)
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
			IFrameStarted += () => HealthSet(health);
			iFrameTimer.Timeout += () => DisableIFrame(health);

		}
	}

	private void CreateBlinkTimer()
	{
		BlinkTimer = new();
		BlinkTimer.OneShot = true;
		BlinkTimer.Timeout += () => Cooldown();
		AddChild(BlinkTimer);
	}

	private void Blink(Vector2 position)
	{
		IFrame();
		//playerShip.KillMomentum();
		Tween tween = GetTree().CreateTween();
		tween.BindNode(playerShip);
		tween.SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(playerShip, "position", playerShip.Position + (position * blinkDist).Rotated(playerShip.Rotation), 0.10f);
		isBoosting = true;
		isAllowedToBlink = false;
		IsBlinkHUDVisible(isAllowedToBlink);
	}

	private void Cooldown()
	{
		isAllowedToBlink = true;
		isBoosting = false;
		IsBlinkHUDVisible(isAllowedToBlink);
	}

	private void IsBlinkHUDVisible(bool visible) => blinkHUD.Visible = visible;
	private void HealthSet(Node health) => health.Set("i_bool", true);
	private void DisableIFrame(Node health) => health.Set("i_bool", false);
	public void SetRaycastScale(Vector2 scale)
	{
		rayCast2D.Scale = new((scale.X + 1) * 23, rayCast2D.Scale.Y);
	}
}
