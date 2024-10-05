using Godot;
using System;
using Godot.Collections;
using System.Linq;

[GlobalClass]
public partial class ShipComponent: Area2D
{
    [Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

    [Export]
    private DefenseInfo defenseInfo;

    [Export]
    private Node2D[] vertices;

    [Export]
    public bool TopAttachable { get; private set; }
    [Export]
    public bool BottomAttachable { get; private set; }
    [Export]
    public bool RightAttachable { get; private set; }
    [Export]
    public bool LeftAttachable { get; private set; }

    private bool destroyed = false;

    DefenseInfo GetDefenseInfo() => defenseInfo;
    public bool IsDestroyed() => destroyed;

    public virtual Dictionary GetInfo()
    {
        return new(){
            {nameof(defenseInfo), defenseInfo},
        };
    }

    public virtual void SetInfo(Dictionary info)
    {
        defenseInfo = (DefenseInfo)info[nameof(defenseInfo)];
    }

    public void CollideWithBody(Node2D node2D)
    {
        if(node2D is IDamager)
        {
            IDamager damager = node2D as IDamager;
            TakeDamage(damager.GetDamageInfo());
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        GD.Print("hit by bullet");
        int newHealth = defenseInfo.currentHealth + defenseInfo.defense;
        newHealth -= damageInfo.damage;
        if(newHealth < defenseInfo.currentHealth)
        {
            defenseInfo.currentHealth = newHealth;
            if(defenseInfo.currentHealth <= 0) { Destroyed(); }
        }
    }

    public Vector2[] GetVertices()
    {
        Vector2[] v2Vertices = new Vector2[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            Vector2 v2 = vertices[i].GlobalPosition;
            v2Vertices[i] = v2;
        }
        return v2Vertices;
    }

    public void RotateRight()
    {
        bool newTop = false;
        bool newRight = false;
        bool newBottom = false;
        bool newLeft = false;
        newRight = TopAttachable;
        newBottom = RightAttachable;
        newLeft = BottomAttachable;
        newTop = LeftAttachable;
        TopAttachable = newTop;
        RightAttachable = newRight;
        BottomAttachable = newBottom;
        LeftAttachable = newLeft;
        RotationDegrees += 90;
    }

    public void RotateLeft()
    {
        bool newTop = false;
        bool newLeft = false;
        bool newBottom = false;
        bool newRight = false;
        newRight = BottomAttachable;
        newBottom = LeftAttachable;
        newLeft = TopAttachable;
        newTop = RightAttachable;
        TopAttachable = newTop;
        RightAttachable = newRight;
        BottomAttachable = newBottom;
        LeftAttachable = newLeft;
        RotationDegrees -= 90;
    }

    private void Destroyed()
    {
        destroyed = true;
        CollisionLayer = 0;
        CollisionMask = 0;
        Hide();
        EmitSignal(SignalName.OnDestroyed, this);
    }

    private void Revived()
    {
        CollisionLayer = 0b00000000_00000000_00000000_00000001;
        CollisionMask = 0b00000000_00000000_00000000_00000001;
        destroyed = false;
        Visible = true;
    }
}
