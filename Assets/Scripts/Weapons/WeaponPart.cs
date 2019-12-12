using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Part", menuName = "Weapons/New Part")]
public class WeaponPart : Item
{
    [Header("Quiver")]
    public bool isQuiver;
    public uint magazineSize;
    public uint[] magezineSizes = new uint[] {
        100, 50, 10
    };

    [Header("Barrel")]
    public bool isBarrel;
    public Bullet bulletPrefab;

    [Header("Stock")]
    public bool isStock;
    // Max movement added to the visor on shoot
    public float recoil;
    public float[] recoils = new float[] {
        0f, 1f, 5f
    };

    [Header("Sight")]
    public bool isSight;
    [Range (0f, 1f)]
    // 0 means no pertubation added to shoots
    // 1 means maximalPertubation is added to shoots
    public float precision; 
    public float[] precisions = new float[] {
        0f, 0.05f, 0.5f
    };

    [Header("Handle")]
    public bool isHandle;
    public float reloadTime;
    public float[] reloadTimes = new float[] {
        0f, 2f, 5f
    };

    [Header("String")]
    public bool isString;
    public float fireRate;  // Time between every shot
    public float lifespan;  // Time before decaying
    
    public float[] fireRates = new float[] {
        0.1f, 0.5f, 1f
    };

    public void RandomizeQuiver()
    {
        isQuiver = true;
        sprite = Utility.RandomElement(GameController.Instance.quiverSprites);

        magazineSize = Utility.RandomElement(magezineSizes);
        
        GenerateName(GetQualifier(magazineSize, magezineSizes));
    }

    public void RandomizeBarrel()
    {
        isBarrel = true;
        sprite = Utility.RandomElement(GameController.Instance.barrelSprites);

        Bullet[] bullets = Resources.LoadAll<Bullet>("Bullets");
        bulletPrefab = Utility.RandomElement(bullets);
        
        GenerateName(GetQualifier(bulletPrefab, bullets));
    }

    public void RandomizeStock()
    {
        isStock = true;
        sprite = Utility.RandomElement(GameController.Instance.stockSprites);

        recoil = Utility.RandomElement(recoils);
        
        GenerateName(GetQualifier(recoil, recoils));
    }

    public void RandomizeSight()
    {
        isSight = true;
        sprite = Utility.RandomElement(GameController.Instance.sightSprites);

        precision = Utility.RandomElement(precisions);
        
        GenerateName(GetQualifier(precision, precisions));
    }

    public void RandomizeHandle()
    {
        isHandle = true;
        sprite = Utility.RandomElement(GameController.Instance.handleSprites);

        reloadTime = Utility.RandomElement(reloadTimes);
        
        GenerateName(GetQualifier(reloadTime, reloadTimes));
    }

    public void RandomizeString()
    {
        isString = true;
        sprite = Utility.RandomElement(GameController.Instance.stringSprites);

        fireRate = Utility.RandomElement(fireRates);
        lifespan = 3.0f;

        GenerateName(GetQualifier(fireRate, fireRates));
    }

    public override PartType GetPartType()
    {
        if (isQuiver) {
            return PartType.QUIVER;
        }
        if (isBarrel) {
            return PartType.BARREL;
        }
        if (isHandle) {
            return PartType.HANDLE;
        }
        if (isSight) {
            return PartType.SIGHT;
        }
        if (isStock) {
            return PartType.STOCK;
        }
        if (isString) {
            return PartType.STRING;
        }

        // This should never be possible
        Assert.IsTrue(false);
        return PartType.NONE;
    }

    public override bool Use()
    {
        if (!CraftingUI.Instance.IsActive()) {
            return false;
        }

        return CraftingUI.Instance.AddItem(this);
    }

    /**
     * How many different part type exists
     */
    public static int GetPartTypeCount()
    {
        return System.Enum.GetValues(typeof(PartType)).Cast<int>().Max();
    }

    string GetPartName()
    {
        string[] partNames = new string[] {
            "Unknown",
            "Quiver",
            "Sight",
            "Barrel",
            "String",
            "Stock",
            "Handle"
        };

        return partNames[(int) GetPartType()];
    }

    /**
     * Assumes value at the array start are better than value at the array end
     */
    string GetQualifier<T>(T value, T[] array)
    {
        int index = System.Array.IndexOf(array, value);

        if (index == 0) {
            return "Godly";
        }
        else if (index == array.Length - 1) {
            return "Awful";
        }

        return "";
    }
    
    void GenerateName(string qualifier)
    {
        itemName = qualifier + " " + GetPartName();
    }
}

public enum PartType
{
    NONE = 0,
    QUIVER,
    SIGHT,
    BARREL,
    STRING,
    STOCK,
    HANDLE
}
