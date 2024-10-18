using Godot;
using System;

public partial class SnapGrid : GridContainer
{
    public override void _Ready()
    {
        var gridContainer = new GridContainer();
        gridContainer.Columns = 15;
        gridContainer.SetSize(new Vector2(32, 32));
        AddChild(gridContainer);
        for (int i = 0; i < 150; i++)
        {
            var colorRect = new ColorRect();
            colorRect.Color = new Color(0.549f, 0.602f, 0.815f, 0.600f);
            colorRect.CustomMinimumSize = new Vector2(32, 32);
            gridContainer.AddChild(colorRect);
        }

        gridContainer.Set("theme_override_constants/h_separation", 1);
        gridContainer.Set("theme_override_constants/v_separation", 1);
    }


}
