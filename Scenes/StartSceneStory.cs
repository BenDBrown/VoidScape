using Godot;
using System;

public partial class StartSceneStory : CanvasLayer
{
    [Export]
    public AnimationPlayer FrameAnimPlayer { get; private set; }

    [Export]
    public PackedScene OpenScene;


    public override void _Ready()
    {
        base._Ready();
        FrameAnimPlayer.Play("story_scene");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsKeyPressed(Key.F12))
        {
            GetTree().ChangeSceneToPacked(OpenScene);
        }
    }
    public void OnAnimationFinished(StringName animation)
    {
        if (animation == "story_scene")
        {
            GetTree().ChangeSceneToPacked(OpenScene);
        }
    }
}
