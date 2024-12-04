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
        (Material as ShaderMaterial).SetShaderParameter("shades", targetColours);
        TextureChanged += SetUp;
    }

    private void SetUp()
    {
        SetColour(colour);
        (Material as ShaderMaterial).SetShaderParameter("shades", targetColours);
    }

    public void SetColour(Color colour)
    {
        (Material as ShaderMaterial).SetShaderParameter("color", colour);
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
            (Material as ShaderMaterial).SetShaderParameter("shades", newTargets.ToArray());
        }
        (Material as ShaderMaterial).SetShaderParameter("shades", targetColours.ToArray());
    }
}
