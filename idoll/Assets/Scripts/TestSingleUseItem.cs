using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TestSingleUseItem : SingleUseItem
{
    public TestSingleUseItem()
        : base("test_single_use_item", 20)
    {
        
    }

    public TestSingleUseItem(TestSingleUseItem item)
        : base(item)
    {

    }

    /*public override Item Copy()
    {
        return (Item) ScriptableObject.CreateInstance("TestSingleUseItem");
    }*/
}
