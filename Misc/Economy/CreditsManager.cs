using Godot;
using System;

[GlobalClass]
public partial class CreditsManager : Node
{
    [Signal]
    public delegate void CreditsChangedEventHandler(float totalCredits);

    public float TotalCredits {get; private set;}

    /// <summary>
    /// Take credits from the player.
    /// </summary>
    /// <param name="decreaseAmount">The amount of credits that need to be taken away from the player.</param>
    /// <returns>Returns a boolean representing whether the action is succesfully completed or not.</returns>
    public bool TryDecreaseCredits(float decreaseAmount){
        if(HasEnoughMoney(decreaseAmount)){
            TotalCredits -= decreaseAmount;
            EmitSignal(SignalName.CreditsChanged, TotalCredits);
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// Add credits to the total amount.
    /// </summary>
    /// <param name="increaseAmount">The amount that needs to be added to the total.</param>
    public void AddCredits(float increaseAmount){
        TotalCredits += increaseAmount;
        EmitSignal(SignalName.CreditsChanged, TotalCredits);
    }
    
    /// <summary>
    /// This is used to check wether the player has enough credits. This should be used in tandem with the shop when presenting items to buy.
    /// </summary>
    /// <param name="priceToCheck">This amount will be compared to the total credits of the player. This is most of the times a price of an obejct.</param>
    /// <returns>Returns a boolean portraying whether the player has enough credits compared to the given amount or not.</returns>
    public bool HasEnoughMoney(float priceToCheck){
        return priceToCheck <= TotalCredits;
    }
}
