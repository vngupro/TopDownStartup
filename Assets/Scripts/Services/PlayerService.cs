using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerService : MonoBehaviour, IPlayerService
{
    public List<Player> _players { get; private set; } = new List<Player>();

    private void Awake()
    {
        var startPlayer = Instantiate(Resources.Load<GameObject>("Player/Player")).GetComponent<Player>();
        _players.Add(startPlayer);
    }

    void IPlayerService.AddPlayer()
    {
        var newplayer = Instantiate(new Player());
        _players.Add(newplayer);
    }

    void IPlayerService.RemovePlayer(Player player) => _players.Remove(player);

    void IPlayerService.RemovePlayerAt(int index) => _players.RemoveAt(index);
    
    public Player GetPlayerAt(int index) => _players[index];

    public List<Player> GetAllPlayers() => _players;
}
