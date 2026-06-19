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

    public void InitalizeRock(CharacterType character)
    {
        currentAbility = GetAbilityType(character);
    }
    public void Ability()
    {

        switch (currentAbility)
        {
            case AbilityType.splitshot:
            SplitShot();
            break;

            case AbilityType.speedIncrease:
            increaseSpeed();
            break;

            case AbilityType.bleeder:
            Bleeder();
            break;
        }
    }

    private void SplitShot()
    {
        
    }

    private void increaseSpeed()
    {
        
    }

    private void Bleeder()
    {
        
    }
}
