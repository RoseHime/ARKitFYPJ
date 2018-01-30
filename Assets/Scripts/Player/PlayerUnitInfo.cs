using UnityEngine;

public class PlayerUnitInfo : MonoBehaviour {

    /* Unit Type */
    public enum PlayerUnitType
    {
        PUN_WORKER,
        PUN_MELEE,
        PUN_RANGE,
        PUN_TANK,
        PUN_MAX
    }
    public PlayerUnitType PUN;

    public PlayerUnitType GetUnitType()
    {
        return PUN; // return Unit Type
    }

    /* Unit State */
    public enum PlayerUnitState
    {
        PUS_GUARD,
        PUS_ATTACK,
        PUS_MOVE,
        PUS_HARVEST,
        PUS_MAX_STATES
    }
    public PlayerUnitState PUS;

    public PlayerUnitState GetUnitState()
    {
        return PUS;// return Unit State
    }

    public void SetUnitState(PlayerUnitState pus) { PUS = pus; }


    /* Unit Data */
    public float f_HealthPoint;                                     //Unit Health Point
    public float GetUnitHealth() { return f_HealthPoint; }

    public float f_DetectRange;                                     //Unit Detection Range
    public float GetUnitDetectRange() { return f_DetectRange * f_DetectRange; }

    public float f_UnitAttackRange;                                     //Unit Attack Range
    public float GetUnitAttackRange() { return f_UnitAttackRange * f_UnitAttackRange; }

    public float f_BaseAttackRange;                                     //Base Attack Range
    public float GetBaseAttackRange() { return f_BaseAttackRange * f_BaseAttackRange; }

    private float f_OriginSpeed;                                        //Store origin speed
    public void SetOriginSpeed(float spd) { f_OriginSpeed = spd; }
    public float GetOriginSpeed() { return f_OriginSpeed; } 

    public float f_AttackDamage;                                    //Unit Attack Damage
    public float GetUnitAttackDmg() { return f_AttackDamage; }

    /* Unit Cost */
    public int i_woodCost;          // Unit Wood Cost
    public int i_stoneCost;         // Unit Stone Cost
    public int i_magicStoneCost;    // Unit Bone Cost

}
