using UnityEngine;
using System.Collections;

public class Coin : Drop {

    public int value = 1;

    public override void AddValue(PCInputManager player)
    {
        player.AddValue(value);
    }


}
