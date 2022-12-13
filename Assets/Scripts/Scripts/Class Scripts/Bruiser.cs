using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : Character
{
    /* Constructor
    */
    public Bruiser() : base(){
        maxHp = 25;
        hp = maxHp;
        movement = 3;
        range = 1;
        speed = 1;
        attackDamage = 2;
        magicDamage = 0;
        physicalResistance = 3;
        magicResistance = 3;
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