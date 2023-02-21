using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleUseItem : SingleUseItem
{
    public TestSingleUseItem()
        : base("test_single_item", null)
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
