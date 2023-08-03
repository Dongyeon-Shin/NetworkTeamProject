using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public TMP_Text power_Text;
    public TMP_Text speed_Text;
    public TMP_Text bomb_Text;

    // 테스트용
    private int power = 1;
    private int bomb = 1;
    private int speed = 1;

    public int Power { get { return power; } set {  power += value; } }
    public int Bomb { get {  return bomb; } set {  bomb += value; } }
    public int Speed {  get { return speed; } set {  speed += value; } }

    public void ItemInterfaceSet()
    {
        power_Text.text = $"{power-1}";
        speed_Text.text = $"{speed - 1}";
        bomb_Text.text = $"{bomb - 1}";
    }
}
