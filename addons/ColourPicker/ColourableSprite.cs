using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ColourableSprite : Sprite2D
{
    private const uint MAX_AMOUNT_OF_TARGET_COLOURS = 10;

    [Export]
    Color colour;

    [Export]
    Color[] targetColours = new Color[MAX_AMOUNT_OF_TARGET_COLOURS];

    [Export]
    ShaderMaterial shaderMat;

    public override void _Ready()
    {
        base._Ready();
        SetMaterial(shaderMat);
        SetColour(colour);
        shaderMat.SetShaderParameter("shades", targetColours);
    }

    public void SetColour(Color colour)
    {
        shaderMat.SetShaderParameter("color", colour);
    }

    public void SetTargetColours(List<Color> targetColours)
    {
        if(targetColours.Count > MAX_AMOUNT_OF_TARGET_COLOURS)
        {
            List<Color> newTargets = new();
            for (int i = 0; i < MAX_AMOUNT_OF_TARGET_COLOURS; i++)
            {
                newTargets.Add(targetColours[i]);
            }
            shaderMat.SetShaderParameter("shades", newTargets.ToArray());
        }
        shaderMat.SetShaderParameter("shades", targetColours.ToArray());
    }
}
