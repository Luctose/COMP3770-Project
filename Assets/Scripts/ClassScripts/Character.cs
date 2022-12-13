using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Class info
    protected string className; // Name
    protected string classDesc; // Description

    // Stats
    protected int level; // Character level
    protected int exp; // Experience 
    protected int expCap; // Experience to hit for next level
    protected int maxHp; // Max hit-points
    protected int hp; // Current hit-points
    protected int movement; // Map spaces a unit can move
    protected int range; // Attack range
    protected int speed; // Combat speed
    protected int attackDamage; // Combat physical attack
    protected int magicDamage; // Combat magic attack
    protected int physicalResistance; // Combat physical defense
    protected int magicResistance; // Combat magic defense

    // Other character attributes
    // protected Inventory characterInventory; <-- Commented out as the Inventory class needs to be made first
    protected Weapon equipped;

    // Base Constructor
    /* Constructor initializes values to minimum values
     * Does not include combat stats (Character base class only)
     */
    public Character(){
        level = 1;
        exp = 0;
        expCap = 100;
    }

    /* Copy constructor
     */
    public Character(Character copy){
        level = copy.level;
        exp = copy.exp;
        expCap = copy.expCap;
        hp = copy.hp;
        movement = copy.movement;
        speed = copy.speed;
        attackDamage = copy.attackDamage;
        magicDamage = copy.magicDamage;
        physicalResistance = copy.physicalResistance;
        magicResistance = copy.magicResistance;
    }

    // Getters and Setters
    public string ClassName{
        get{return className;}
        set{className = value;}
    }

    public string ClassDesc{
        get{return classDesc;}
        set{classDesc = value;}
    }

    public int Level{
        get{return level;}
        set{level = value;}
    }

    public int Exp{
        get{return exp;}
        set{exp = value;}
    }

    public int Hp{
        get{return hp;}
        set{hp = value;}
    }

    public int Movement{
        get{return movement;}
        set{movement = value;}
    }

    public int Speed{
        get{return speed;}
        set{speed = value;}
    }

    public int AttackDamage{
        get{return attackDamage;}
        set{attackDamage = value;}
    }

    public int MagicDamage{
        get{return magicDamage;}
        set{magicDamage = value;}
    }

    public int PhysicalResistance{
        get{return physicalResistance;}
        set{physicalResistance = value;}
    }

    public int MagicResistance{
        get{return magicResistance;}
        set{magicResistance = value;}
    }

    public Weapon Equipped{
        get{return equipped;}
        set{equipped = value;}
    }

    /* This method levels up a characters stats and resets thier experience points
    */
    public bool LevelUp(){
        // Check if Character has enough exp
        if(exp < expCap){
            return false; // Exit function did not level up
        }

        // Increase level
        ++level;
        exp = 0; // Reset experience points
        expCap = (int)(expCap * 1.1); // Make the next level harder

        // Return level up successful
        return true;

        // Increase stats
        // (Does not occur in base class child calls base.LevelUp())
    }

    /* The character will gain exp
     * Params: The enemy defeated
     */
    public void GainExp(Character enemy){
        exp += (20 * enemy.Level) / this.Level;
    }
}
