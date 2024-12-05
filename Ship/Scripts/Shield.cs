using Godot;
using System;

[GlobalClass]
public partial class Shield : Node2D
{
    [Signal]
    public delegate void ShieldHitEventHandler(int powerDraw);

    [Export]
    private Sprite2D shieldSprite;

    [Export]
    private CollisionShape2D shieldCollider;

    [Export]
    private int powerDraw = 10; 

    public int PowerDraw => GetPowerDraw();

    public bool Shielding {get; private set;} = false;

    public void StartShielding() 
    {
        Shielding = true;
        shieldCollider.Disabled = false;
        shieldSprite.Visible = true;
    }
    public void StopShielding() 
    {
        Shielding = false;
        shieldCollider.Disabled = true;
        shieldSprite.Visible = false;
    }

    public void BodyEntered(Node2D node2D)
    {
        if(!Shielding) return;
        if(node2D is Bullet bullet)
        {
            bullet.QueueFree();
            EmitSignal(SignalName.ShieldHit, powerDraw);
        }
    }

    public int GetPowerDraw()
    {
        if(Shielding) return powerDraw;
        return 0;
    }
}
