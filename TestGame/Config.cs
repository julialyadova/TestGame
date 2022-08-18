using System;
using System.Configuration;

namespace TestGame;

public static class Config
{
    public static readonly string ServerHost;
    public static readonly int ServerPort;
    public static readonly int ClientPort;
    public static readonly string ConnectionKey;
    public static readonly int MapSeed;

    static Config()
    {
        ServerHost = ConfigurationManager.AppSettings.Get("ServerHost");
        ServerPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
        ClientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
        ConnectionKey = ConfigurationManager.AppSettings.Get("ConnectionKey");
        MapSeed = int.Parse(ConfigurationManager.AppSettings.Get("MapSeed"));
    }
}