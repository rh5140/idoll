using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDurabilityItem : DurabilityItem
{
    public TestDurabilityItem()
        : base("test_dur_item", null, 100, 100)
    {

    }

    public override DurabilityItem Clone()
    {
        return new TestDurabilityItem();
    }
}
