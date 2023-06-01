using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TestDurabilityItem : DurabilityItem
{
    public static Sprite s;
    public TestDurabilityItem()
        : base("test_dur_item", 10, 10, "This is a test durability item")
    {

    }

    public TestDurabilityItem(TestDurabilityItem item)
        : base(item)
    {

    }

    /*public override Item Copy()
    {
        return (Item) ScriptableObject.CreateInstance("TestDurabilityItem");
    }*/
}
