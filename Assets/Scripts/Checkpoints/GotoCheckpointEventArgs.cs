using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GotoCheckpointEventArgs : EventArgs
{
    public string ConversationID;
    public GotoCheckpointEventArgs(string conversationID)
    {
        this.ConversationID = conversationID;
    }

    public GotoCheckpointEventArgs()
    {
        ConversationID = null;
    }
}