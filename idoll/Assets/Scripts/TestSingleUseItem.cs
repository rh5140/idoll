using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleUseItem : SingleUseItem
{
    public static Sprite s;
    public TestSingleUseItem()
        : base("test_single_use_item", s)
    {
        
    }

    public TestSingleUseItem(TestSingleUseItem item)
        : base(item)
    {

    }

    public override Item Copy()
    {
        return new TestSingleUseItem(this);
    }
}
