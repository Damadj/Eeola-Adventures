using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Items/Gold", order = 3)]
public class Gold : Item
{
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0}", MyPrice);
    }
}
