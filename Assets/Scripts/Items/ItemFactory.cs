using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : Singleton<ItemFactory>
{
    public ItemBase ItemBasePrefab;

    private Dictionary<ITEM_TYPE, Func<ItemBase, Item>> itemCreators = new Dictionary<ITEM_TYPE, Func<ItemBase, Item>>
    {
        { ITEM_TYPE.GreenCube, (itemBase) => CreateCubeItem(itemBase, MATCH_TYPE.Green) },
        { ITEM_TYPE.BlueCube, (itemBase) => CreateCubeItem(itemBase, MATCH_TYPE.Blue) },
        { ITEM_TYPE.RedCube, (itemBase) => CreateCubeItem(itemBase, MATCH_TYPE.Red) },
        { ITEM_TYPE.YellowCube, (itemBase) => CreateCubeItem(itemBase, MATCH_TYPE.Yellow) },
        { ITEM_TYPE.Box, CreateBoxItem },
        { ITEM_TYPE.Stone, CreateStoneItem },
        { ITEM_TYPE.Vase01, CreateVaseItem },
        { ITEM_TYPE.Vase02, CreateVaseItem },
        { ITEM_TYPE.HorizontalRocket, CreateHRocket },
        { ITEM_TYPE.VerticalRocket, CreateVRocket }
    };

    public Item CreateItem(ITEM_TYPE itemType, Transform parent)
    {
        if(itemType == ITEM_TYPE.None) return null;

        var itemBase = Instantiate(ItemBasePrefab, Vector3.zero, Quaternion.identity, parent);
        itemBase.Type = itemType;

        if (!itemCreators.TryGetValue(itemType, out var createItem))
        {
            Debug.LogWarning("Can not create item: " + itemType);
            return null;
        }

        return createItem(itemBase);
    }

    private static Item CreateCubeItem(ItemBase itemBase, MATCH_TYPE matchType)
    {
        var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
        cubeItem.PrepareCubeItem(itemBase, matchType);
        return cubeItem;
    }

    private static Item CreateBoxItem(ItemBase itemBase)
    {
        var boxItem = itemBase.gameObject.AddComponent<BoxItem>();
        boxItem.PrepareBoxItem(itemBase);
        return boxItem;
    }

    private static Item CreateStoneItem(ItemBase itemBase)
    {
        var stoneItem = itemBase.gameObject.AddComponent<StoneItem>();
        stoneItem.PrepareStoneItem(itemBase);
        return stoneItem;
    }

    private static Item CreateVaseItem(ItemBase itemBase)
    {
        var vaseItem = itemBase.gameObject.AddComponent<VaseItem>();
        vaseItem.PrepareVaseItem(itemBase);
        return vaseItem;
    }

    private static Item CreateHRocket(ItemBase itemBase)
    {
        var rocketItem = itemBase.gameObject.AddComponent<RocketItem>();
        rocketItem.PrepareRocketItem(itemBase, ROCKET_DIRECTION.Horizontal);
        return rocketItem;
    }

    private static Item CreateVRocket(ItemBase itemBase)
    {
        var rocketItem = itemBase.gameObject.AddComponent<RocketItem>();
        rocketItem.PrepareRocketItem(itemBase, ROCKET_DIRECTION.Vertical);
        return rocketItem;
    }
}
