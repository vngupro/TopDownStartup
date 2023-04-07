using System.Collections.Generic;

internal interface IPlayerService
{
    List<Player> _players { get; }

    void AddPlayer(Player newplayer);
    void RemovePlayer(Player newplayer);
    void RemovePlayerAt(int index);
    Player GetPlayerAt(int index);
    List<Player> GetAllPlayers();
}