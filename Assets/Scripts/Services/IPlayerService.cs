using System.Collections.Generic;

internal interface IPlayerService
{
    List<Player> _players { get; }

    void AddPlayer();
    void RemovePlayer(Player player);
    void RemovePlayerAt(int index);
    Player GetPlayerAt(int index);
    List<Player> GetAllPlayers();
}