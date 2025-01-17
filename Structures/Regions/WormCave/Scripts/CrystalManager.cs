using Godot;
using System;
using System.Collections.Generic;

public partial class CrystalManager : Node
{
    [Signal]
    public delegate void AllCrystalsActivatedEventHandler();
    
    [Export]
    private Crystal[] crystals;

    public override void _Ready()
    {
        base._Ready();
        foreach(Crystal crystal in crystals)
        {
            crystal.CrystalHit += CrystalHit;
        }
    }

    public void CrystalHit(Crystal crystal)
    {
        foreach(Crystal connectedCrystal in crystal.ConnectedCrystals)
        {
            connectedCrystal.Flip();
        }
        CheckCrystals();
    }

    private void CheckCrystals()
    {
        bool allCrystalsActive = true;
        foreach (Crystal crystal in crystals)
        {
            if(!crystal.Active) 
            {
                allCrystalsActive = false;
                break;
            }
        }
        if(!allCrystalsActive) return;

        foreach (Crystal crystal in crystals)
        {
            crystal.SetDeferred("monitoring", false);
        }
        EmitSignal(SignalName.AllCrystalsActivated);
    }

}
