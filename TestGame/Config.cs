using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Json.Net;

namespace TestGame;

public class Config
{
    public int ScreenWidth;
    public int ScreenHeight;
    public string ServerHost;
    public int ServerPort;
    public int ClientPort;
    public string ConnectionKey;
    public int MapSeed;
    public int PlayerId;
    public string PlayerName;
    public string PlayerTexture;

    public static Config FromFile(string name)
    {
        Config config;
        using (StreamReader reader = new StreamReader("config.json"))
        {
            string json = reader.ReadToEnd();
            config = JsonNet.Deserialize<Config>(json);
        }

        return config;
    }
}