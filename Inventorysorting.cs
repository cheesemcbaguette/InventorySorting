using System;
using System.Collections.Generic;
using System.Linq;
using Eleon.Modding;




public partial class InventorySorting: ModInterface
{
    static ModGameAPI GameAPI;
    
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
        try
        {
            switch (eventId)
            {

                case CmdId.Event_Player_Connected:
                    GameAPI.Console_Write("InventorySorting : player connected! ");
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
                        int id = ci.playerId;
                        GameAPI.Game_Request(CmdId.Request_Player_GetInventory, (ushort)1, id);
                        
                    }
                    break;
                case CmdId.Event_Player_Inventory:
                    Inventory inventory = (Inventory) data;
                    Sort(inventory);
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
        ItemStack[] bag = inventory.bag;
        int size = bag.Length;
        for(int i = 1; i < size; ++i)
        {
            ItemStack itemStack = bag[i];
            for(int j = 0; j >= 0; --j)
            {
                if (j == 0 || bag[j - 1].id < itemStack.id)
                {
                    bag[j] = itemStack;
                    break;
                }

                else if (bag[j - 1].id > itemStack.id) bag[j] = bag[j - 1];
            }
        }
        inventory.bag = bag;

        GameAPI.Game_Request(CmdId.Request_Player_SetInventory, (ushort)1, inventory); 
        
            
        
    }

    public void Game_Update()
    {
    }

    public void Game_Exit()
    {
    }
}