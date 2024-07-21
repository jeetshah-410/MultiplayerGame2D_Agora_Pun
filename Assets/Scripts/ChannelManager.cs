using System;
using System.Collections.Generic;


public static class ChannelManager
{
    
    readonly private static Dictionary<string, HashSet<int>> channels = new();

    public static string GenerateRandomChannelNames()
    {
        return Guid.NewGuid().ToString();
    }

    public static void AddPlayerToChannel(string channelName, int playerID)
    {
        if (!channels.ContainsKey(channelName))
        {
            channels[channelName] = new HashSet<int>();
        }

        channels[channelName].Add(playerID);
    }

    public static void RemovePlayerFromChannel(string channelName, int playerID)
    {
        if (channels.ContainsKey(channelName))
        {
            channels[channelName].Remove(playerID);
        }

        if (channels[channelName].Count == 0)
        {
            channels.Remove(channelName);
        }
    }

    public static string GetPlayerChannel(int playerID)
    {
        foreach (var channel in channels)
        {
            if (channel.Value.Contains(playerID)) return channel.Key;
        }
        return null;
    }

    public static bool IsPlayerInChannel(int playerID)
    {
        foreach (var channel in channels)
        {
            if (channel.Value.Contains(playerID)) return true;
        }
        return false;
    }
    
}
