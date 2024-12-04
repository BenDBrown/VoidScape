using Godot;
using System;

public partial class GridTile : TextureRect
{
    public bool IsValid { get; private set; }

    public void ChangeValidity(bool valid)
    {
        IsValid = valid;
    }
}
