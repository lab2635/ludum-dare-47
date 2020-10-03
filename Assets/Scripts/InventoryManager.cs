using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class InventoryManager : MonoBehaviour
{
    public UIView Key1View;
    public UIView Key2View;
    public UIView Key3View;

    public HashSet<InventoryItems> Inventory { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        this.Inventory = new HashSet<InventoryItems>();

        UIView inventoryContainer = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<UIView>();

        this.Key1View = inventoryContainer.GetComponentsInChildren<UIView>()[1];
        this.Key2View = inventoryContainer.GetComponentsInChildren<UIView>()[2];
        this.Key3View = inventoryContainer.GetComponentsInChildren<UIView>()[3];
        this.Key1View.Hide();
        this.Key2View.Hide();
        this.Key3View.Hide();

        GameManager.OnReset += ResetState;
    }

    public void GetItem(InventoryItems contents)
    {
        switch (contents)
        {
            case InventoryItems.Key1:
                this.Inventory.Add(InventoryItems.Key1);
                this.Key1View.Show();
                break;
            case InventoryItems.Key2:
                this.Inventory.Add(InventoryItems.Key2);
                this.Key2View.Show();
                break;
            case InventoryItems.Key3:
                this.Inventory.Add(InventoryItems.Key3);
                this.Key3View.Show();
                break;
            case InventoryItems.Gun:
                this.Inventory.Add(InventoryItems.Gun);
                this.gameObject.GetComponent<CreatureController>().AcquireGun();
                break;
            default: break;
        }
    }

    public void RemoveItem(InventoryItems contents)
    {
        switch (contents)
        {
            case InventoryItems.Key1:
                this.Inventory.Remove(InventoryItems.Key1);
                this.Key1View.Hide();
                break;
            case InventoryItems.Key2:
                this.Inventory.Remove(InventoryItems.Key2);
                this.Key2View.Hide();
                break;
            case InventoryItems.Key3:
                this.Inventory.Remove(InventoryItems.Key3);
                this.Key3View.Hide();
                break;
            default: break;
        }
    }

    private void ResetState()
    {
        this.Inventory = new HashSet<InventoryItems>();
        this.Key1View.Hide();
        this.Key2View.Hide();
        this.Key3View.Hide();
    }
}

public enum InventoryItems
{
    None,
    Key1,
    Key2,
    Key3,
    Remote,
    Gun
}
