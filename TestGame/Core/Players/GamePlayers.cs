using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TestGame.Core.Entities.Creatures;

namespace TestGame.Core.Players;

public class GamePlayers : IEnumerable<Player>
{
    private List<Player> _players;

    public GamePlayers()
    {
        _players = new List<Player>();
    }

    public bool Exists(int id)
    {
        return _players.FirstOrDefault(p => p.Id == id) != null;
    }

    public void Add(Player player)
    {
        if (_players.FirstOrDefault(p => p.Id == player.Id) == null)
        {
            _players.Add(player);
        }
    }

    public void RemovePlayer(int id)
    {
        _players.RemoveAll(p => p.Id == id);
    }

    public Player FindById(int id)
    {
        return _players.FirstOrDefault(p => p.Id == id);
    }

    public void Clear()
    {
        _players.Clear();
    }
    
    public IEnumerator<Player> GetEnumerator() =>  _players.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _players.GetEnumerator();
}