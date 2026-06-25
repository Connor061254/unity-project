using System;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum AbilityType
{
    splitshot,
    speedIncrease,
    bleeder,
    none
}
public class SpecialAbility : MonoBehaviour
{
    private AbilityType currentAbility = AbilityType.none;

    private PlayerController playerController;
    [SerializeField] private float specialRockSpeedBuff = 4f;
    public AbilityType GetAbilityType(CharacterType character)
    {
        return character switch
        {
            CharacterType.BeanstalkBill => AbilityType.splitshot,

            CharacterType.CutlassKate => AbilityType.bleeder,

            CharacterType.TubbsMcGee => AbilityType.speedIncrease,

            _ => AbilityType.none
        };
    }

    public void InitalizeRock(CharacterType character, PlayerController player)
    {
        playerController = player;
        currentAbility = GetAbilityType(character);

        if(currentAbility == AbilityType.speedIncrease)
        {
            increaseSpeed();
        }
    }
    public void Ability()
    {

        switch (currentAbility)
        {
            case AbilityType.splitshot:
            SplitShot();
            break;

            case AbilityType.bleeder:
            Bleeder();
            break;
        }
    }

    private void SplitShot()
    {
        
    }

    public void increaseSpeed()
    {
        if(playerController != null)
        {
            playerController.currentSpeed += specialRockSpeedBuff;
        }
    }

    public void ReduceSpeed()
    {
        if(currentAbility == AbilityType.speedIncrease)
        {
            playerController.currentSpeed -= specialRockSpeedBuff;
        }
    }

    private void Bleeder()
    {
        
    }
}
