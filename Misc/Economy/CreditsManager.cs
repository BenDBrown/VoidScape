using Godot;
using System;

[GlobalClass]
public partial class CreditsManager : Node
{
    [Signal]
    public delegate void CreditsChangedEventHandler(float totalCredits);

    public float TotalCredits {get; private set;}

    public bool TryDecreaseCredits(float decreaseAmount){
        if(HasEnoughMoney(decreaseAmount)){
            TotalCredits -= decreaseAmount;
            GD.Print(TotalCredits);
            EmitSignal(SignalName.CreditsChanged, TotalCredits);
            return true;
        }
        else{
            return false;
        }
    }

    public void AddCredits(float increaseAmount){
        TotalCredits += increaseAmount;
        EmitSignal(SignalName.CreditsChanged, TotalCredits);
    }

    public bool HasEnoughMoney(float priceToCheck){
        return priceToCheck <= TotalCredits;
    }
}
