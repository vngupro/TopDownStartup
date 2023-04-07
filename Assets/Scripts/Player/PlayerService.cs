using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerService : MonoBehaviour, IPlayerService
{
    public List<Player> _players { get; private set; } = new List<Player>();

    void IPlayerService.AddPlayer(Player newplayer) => _players.Add(newplayer);

    void IPlayerService.RemovePlayer(Player newplayer) => _players.Remove(newplayer);

    void IPlayerService.RemovePlayerAt(int index) => _players.RemoveAt(index);

    public Player GetPlayerAt(int index) => _players[index];

    public List<Player> GetAllPlayers() => _players;
}
