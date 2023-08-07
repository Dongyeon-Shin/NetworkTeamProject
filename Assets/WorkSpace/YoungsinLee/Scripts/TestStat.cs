using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStat : MonoBehaviour
{
    private int power = 1;
    public int bomb = 1;
    public int speed = 1;

    public int Power { get { return power; } set { power += value; } }
    public int Bomb { get { return bomb; } set { bomb += value; } }
    public int Speed { get { return speed; } set { speed += value; } }
}
