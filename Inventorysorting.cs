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
        
        GameAPI.Console_Write("Inventory Sorting Launched! ");
    }


    public void Game_Event(CmdId eventId, ushort seqNr, object data)
    {

        GameAPI.Console_Write($"ID:EVENT! {eventId} - {seqNr}");
        try
        {
            switch (eventId)
            {

                case CmdId.Event_Player_Connected:
                    GameAPI.Game_Request(CmdId.Request_Player_Info, (ushort)CmdId.Request_Player_Info, (Id)data);
                    break;
                case CmdId.Event_ChatMessage:
                    ChatInfo ci = (ChatInfo)data;
                    //TODO
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

    public void Game_Update()
    {
    }

    public void Game_Exit()
    {
    }
}