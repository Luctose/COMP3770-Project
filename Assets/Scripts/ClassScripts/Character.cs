using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Class info
    private string className; // Name
    private string classDesc; // Description

    // Stats
    private int level; // Character level
    private int exp; // Experience 
    private int hp; // Hit-points
    private int movement; // Map spaces a unit can move
    private int speed; // Combat speed
    private int attackDamage; // Combat physical attack
    private int magicDamage; // Combat magic attack
    private int physicalResistance; // Combat physical defense
    private int magicResistance; // Combat magic defense

    // Other character attributes
    // private Inventory characterInventory; <-- Commented out as the Inventory class needs to be made first
    // private Item Equipped; <-- Same as above

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
}
