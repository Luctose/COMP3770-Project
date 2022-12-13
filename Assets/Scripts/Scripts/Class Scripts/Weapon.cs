using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    protected string type;
    protected int damage;
    protected int durability;

    public Weapon(string t, int d){
        type = t;
        damage = d;
        durability = 30;
    }

    // Getters and Setters
    public string Type{
        get{return type;}
        set{type = value;}
    }

    public int Damage{
        get{return damage;}
        set{damage = value;}
    }

    public int Durability{
        get{return durability;}
        set{durability = value;}
    }
}