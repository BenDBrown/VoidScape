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

    public void OnAnimationFinished(StringName animation)
    {
        if (animation == "story_scene")
        {
            GetTree().ChangeSceneToPacked(OpenScene);
        }
    }
}
