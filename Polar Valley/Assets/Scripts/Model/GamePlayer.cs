using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;

public partial class GamePlayer : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId Id { get; set; }

    [MapTo("exp")]
    public long? Exp { get; set; }

    [MapTo("isAdmin")]
    public bool? IsAdmin { get; set; }

    [MapTo("isBanned")]
    public bool? IsBanned { get; set; }

    [MapTo("level")]
    public int? Level { get; set; }

    [MapTo("player_name")]
    public string? PlayerName { get; set; }

    [MapTo("user_id")]
    public string? UserId { get; set; }
}