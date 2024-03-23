using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffect
{
    public void ADPlus(PlayerBase pb)
    {
        pb.ATK += 1;
    }

    public void ADMinus(PlayerBase pb)
    {
        pb.ATK -= 1;
    }
}
