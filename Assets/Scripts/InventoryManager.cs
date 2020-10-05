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

        if (GameManager.Instance.CheckpointList[(int)Checkpoints.GunRoomComplete])
        {
            this.GetItem(InventoryItems.Gun);
        }

        GameManager.OnReset += ResetState;
    }

    public void GetItem(InventoryItems contents)
    {
        Inventory.Add(contents);
        
        switch (contents)
        {
            case InventoryItems.SpawnRoomKey:
                this.Key1View.Show();
                break;
            case InventoryItems.Room1RewardKey:
                this.Key2View.Show();
                break;
            case InventoryItems.Room1Reward:
                this.Key3View.Show();
                break;
            case InventoryItems.Gun:
                this.gameObject.GetComponent<CreatureController>().AcquireGun();
                break;
            default: break;
        }
    }

    public void RemoveItem(InventoryItems contents)
    {
        switch (contents)
        {
            case InventoryItems.SpawnRoomKey:
                this.Inventory.Remove(InventoryItems.SpawnRoomKey);
                this.Key1View.Hide();
                break;
            case InventoryItems.Room1RewardKey:
                this.Inventory.Remove(InventoryItems.Room1RewardKey);
                this.Key2View.Hide();
                break;
            case InventoryItems.Room1Reward:
                this.Inventory.Remove(InventoryItems.Room1Reward);
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

        if (GameManager.Instance.CheckpointList[(int)Checkpoints.GunRoomComplete])
        {
            this.GetItem(InventoryItems.Gun);
        }
    }
}

public enum InventoryItems
{
    None,
    SpawnRoomKey,
    Room1RewardKey,
    Room1Reward,
    Remote,
    Gun,
    Room2Reward,
    Room4RewardKey,
    Room4Reward,
    Room5Reward,
    Room6DoorKey,
    Room6ExitKey
}
