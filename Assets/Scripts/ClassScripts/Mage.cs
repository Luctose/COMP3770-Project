using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{
    /* Constructor
    */
    public Mage() : base(){
        className = "Mage";
        maxHp = 20;
        hp = maxHp;
        movement = 4;
        range = 2;
        speed = 1;
        attackDamage = 0;
        magicDamage = 2;
        physicalResistance = 1;
        magicResistance = 2;
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
