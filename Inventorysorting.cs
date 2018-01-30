using System;
using System.Collections.Generic;
using System.Linq;
using Eleon.Modding;




public partial class InventorySorting: ModInterface
{
    static ModGameAPI GameAPI;
    ushort eventSortId = 26989;
    ushort eventSetInventoryId = 26980;


    public void Game_Start(ModGameAPI dediAPI)
    {
        InventorySorting.GameAPI = dediAPI;
        LogFile("chat.txt", "Mod Loaded");
        GameAPI.Console_Write("Inventory Sorting Launched! ");
    }

    private void LogFile(String FileName, String FileData)
    {
        if (!System.IO.File.Exists("Content\\Mods\\InventorySorting\\" + FileName))
        {
            System.IO.File.Create("Content\\Mods\\InventorySorting\\" + FileName);
        }
        string FileData2 = FileData + Environment.NewLine;
        System.IO.File.AppendAllText("Content\\Mods\\InventorySorting\\" + FileName, FileData2);
    }


    public void Game_Event(CmdId eventId, ushort seqNr, object data)
    {

        GameAPI.Console_Write($"ID:EVENT! {eventId} - {seqNr}");
        LogFile("chat.txt", $"ID:EVENT! {eventId} - {seqNr}");
        try
        {
            switch (eventId)
            {

                case CmdId.Event_Player_Connected:
                    int id = (int)data;
                    GameAPI.Console_Write("SuicideCommand : player " + id + "connected! ");
                    LogFile("chat.txt", "Player " + id + "connected");
                    break;
                case CmdId.Event_ChatMessage:
                    ChatInfo ci = (ChatInfo)data;
                    if (ci.msg.StartsWith("s! "))
                    {
                        ci.msg = ci.msg.Remove(0, 3);
                    }
                    ci.msg = ci.msg.ToLower();
                    if (ci.msg.StartsWith("/sort"))
                    {
                        id = ci.playerId;
                        LogFile("chat.txt", "Player " + id + " sorted inventory");
                        GameAPI.Game_Request(CmdId.Request_Player_GetInventory, eventSortId, new Eleon.Modding.Id(id));
                        
                    }
                    break;
                case CmdId.Event_Player_Inventory:
                    Inventory inventory = (Inventory) data;
                    if(seqNr == eventSortId)
                    {
                        Sort(inventory);
                    }
                    
                    break;

                default:
                    GameAPI.Console_Write($"event: {eventId}");
                    var outmessage = "NO DATA";
                    if (data != null)
                    {
                        outmessage = "data: " + data.ToString();
                    }
                    GameAPI.Console_Write(outmessage);
                    
                    break;
            }
        }
        catch (Exception ex)
        {
            GameAPI.Console_Write(ex.Message);
            GameAPI.Console_Write(ex.ToString());
        }
    }

    public void Sort(Inventory inventory)
    {
        GameAPI.Console_Write("InventorySorting : beginning sorting ");
        LogFile("chat.txt", "InventorySorting : beginning sorting ");
        ItemStack[] bag = inventory.bag;

        for (int i = bag.Length - 1; i > 0; i--)
        {
            for (int j = 0; j <= i - 1; j++)
            {
                if (bag[j].id == bag[j + 1].id)
                {
                    int sum = bag[j].count + bag[j + 1].count;
                    if (bag[j].ammo == 0) //check if its not a weapon
                    {
                        bag[j].count = sum;
                        bag[j + 1] = new ItemStack();
                        j = 0;
                    } 
                }

                if (bag[j].id > bag[j + 1].id)
                {
                    ItemStack highValue = bag[j];
                    bag[j] = bag[j + 1];
                    bag[j + 1] = highValue;
                }

                
            }
        }

        inventory.bag = bag;
        GameAPI.Console_Write("InventorySorting : sorting ended successfully ! ");
        LogFile("chat.txt", "InventorySorting : sorting ended successfully ! ");
        GameAPI.Game_Request(CmdId.Request_Player_SetInventory, eventSetInventoryId, inventory);      
            
        
    }

    public void Game_Update()
    {
    }

    public void Game_Exit()
    {
    }

}