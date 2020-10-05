using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class InventoryManager : MonoBehaviour
{
    private UIView SpawnKeyView;
    private UIView Key1View;
    private UIView Key4View;
    private UIView Key6View;
    private UIView KeyExitView;
    private UIView Reward1View;
    private UIView Reward2View;
    private UIView Reward4View;
    private UIView Reward5View;

    public HashSet<InventoryItems> Inventory { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        this.Inventory = new HashSet<InventoryItems>();

        UIView inventoryContainer = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<UIView>();

        UIView[] uiViews = inventoryContainer.GetComponentsInChildren<UIView>(true);

        this.SpawnKeyView = uiViews[1];
        this.Key1View = uiViews[2];
        this.Key4View = uiViews[3];
        this.Key6View = uiViews[4];
        this.KeyExitView = uiViews[5];
        this.Reward1View = uiViews[6];
        this.Reward2View = uiViews[7];
        this.Reward4View = uiViews[8];
        this.Reward5View = uiViews[9];

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
                this.SpawnKeyView.Show();
                break;
            case InventoryItems.Room1RewardKey:
                this.Key1View.Show();
                break;
            case InventoryItems.Room4RewardKey:
                this.Key4View.Show();
                break;
            case InventoryItems.Room6DoorKey:
                this.Key6View.Show();
                break;
            case InventoryItems.Room6ExitKey:
                this.KeyExitView.Show();
                break;
            case InventoryItems.Room1Reward:
                this.Reward1View.Show();
                break;
            case InventoryItems.Room2Reward:
                this.Reward2View.Show();
                break;
            case InventoryItems.Room4Reward:
                this.Reward4View.Show();
                break;
            case InventoryItems.Room5Reward:
                this.Reward5View.Show();
                break;
            case InventoryItems.Gun:
                this.gameObject.GetComponent<CreatureController>().AcquireGun();
                break;
            default: break;
        }
    }

    public void RemoveItem(InventoryItems contents)
    {
        if (contents == InventoryItems.None || contents == InventoryItems.Remote)
            return;
        
        Inventory.Remove(contents);
        
        switch (contents)
        {
            case InventoryItems.SpawnRoomKey:
                this.SpawnKeyView.Hide();
                break;
            case InventoryItems.Room1RewardKey:
                this.Key1View.Hide();
                break;
            case InventoryItems.Room4RewardKey:
                this.Key4View.Hide();
                break;
            case InventoryItems.Room6DoorKey:
                this.Key6View.Hide();
                break;
            case InventoryItems.Room6ExitKey:
                this.KeyExitView.Hide();
                break;
            case InventoryItems.Room1Reward:
                this.Reward1View.Hide();
                break;
            case InventoryItems.Room2Reward:
                this.Reward2View.Hide();
                break;
            case InventoryItems.Room4Reward:
                this.Reward4View.Hide();
                break;
            case InventoryItems.Room5Reward:
                this.Reward5View.Hide();
                break;
            default: break;
        }
    }

    private void ResetState()
    {
        this.Inventory = new HashSet<InventoryItems>();
        this.SpawnKeyView.Hide();
        this.Key1View.Hide();
        this.Key4View.Hide();
        this.Key6View.Hide();
        this.KeyExitView.Hide();
        this.Reward1View.Hide();
        this.Reward2View.Hide();
        this.Reward4View.Hide();
        this.Reward5View.Hide();

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
