using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDurabilityItem : DurabilityItem
{
    public static Sprite s;
    public TestDurabilityItem()
        : base("test_dur_item", s, 100, 100)
    {

    }

    public TestDurabilityItem(TestDurabilityItem item)
        : base(item)
    {

    }

    public override Item Copy()
    {
        return new TestDurabilityItem(this);
    }
}
