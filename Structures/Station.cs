using Godot;


public partial class Station : Sprite2D
{
	private bool isOnBody = false;

	private Tween tween;

	private Vector2 OriginalScale;

	private Vector2 spritePos;

	[Export]
	public Menu menu;
	[Export]
	public Control popup;

	private PlayerShip playerShip { get => Game.Instance.PlayerShip; }


	public override void _Ready()
	{
		CallDeferred("Init");
	}

	public void Init()
	{
		OriginalScale = playerShip.GlobalScale;
		spritePos = Position;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact") && isOnBody)
		{
			Game.Instance.IsUiOpen = true;
			ShrinkShip();
		}
	}

	public void OnBodyEntered(Node2D node2D)
	{
		if (node2D == playerShip)
		{
			popup.Visible = true;
			isOnBody = true;
		}
	}

	public void OnBodyExited(Node2D node2D)
	{
		if (node2D == playerShip)
		{
			popup.Visible = false;
			isOnBody = false;
		}
	}

	public void FinishedTweening()
	{
		menu.QuitPressed += OnQuitPressed;
		menu.Visible = true;
		popup.Visible = false;
		GetTree().Paused = true;
	}

	public void OnQuitPressed()
	{
		menu.QuitPressed -= OnQuitPressed;
		menu.Visible = false;
		GrowShip();
		Game.Instance.IsUiOpen = false;
	}

	private void ShrinkShip()
	{
		tween = GetTree().CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(playerShip, "position", spritePos, 1f).SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(playerShip, "scale", Vector2.Zero, 0.8f).SetTrans(Tween.TransitionType.Linear);
		tween.Finished += FinishedTweening;
	}
	private void GrowShip()
	{
		Tween tween = GetTree().CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(playerShip, "scale", OriginalScale, 0.8f).SetTrans(Tween.TransitionType.Linear);
		tween.Finished += () => OnGrowFinished(tween);
	}
	private void OnGrowFinished(Tween t)
	{
		popup.Visible = isOnBody;
		GetTree().Paused = false;
	}

}
