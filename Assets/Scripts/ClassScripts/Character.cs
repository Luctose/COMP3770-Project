using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{

    private string className;
    private string classDesc;

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
}
