using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    /* Constructor
    */
    public Archer() : base(){
        maxHp = 20;
        hp = maxHp;
        movement = 4;
        range = 3;
        speed = 2;
        attackDamage = 2;
        magicDamage = 0;
        physicalResistance = 2;
        magicResistance = 1;
    }

    new public bool LevelUp(){
        // Call parent LevelUp and check if it can level up
        if(base.LevelUp()){
            // Leveled up increase stats (This is not tailored yet to each class)
            ++maxHp;
            ++speed;
            ++attackDamage;
            ++magicDamage;
            ++physicalResistance;
            ++magicResistance;
            // Level up successful
            return true;
        }

        // Level up failed
        return false;
    }
}