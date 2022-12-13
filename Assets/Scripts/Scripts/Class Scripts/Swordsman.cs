using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Character
{
    /* Constructor
    */
    public Swordsman() : base(){
        className = "Swordsman";
        maxHp = 20;
        hp = maxHp;
        movement = 5;
        range = 1;
        speed = 2;
        attackDamage = 3;
        magicDamage = 0;
        physicalResistance = 1;
        magicResistance = 1;

        equipped = new Weapon("Physical", 10);
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