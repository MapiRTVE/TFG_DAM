using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;

public partial class GameData : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId Id { get; set; }

    [MapTo("game_date")]
    public DateTimeOffset? GameDate { get; set; }

    [MapTo("game_duration")]
    public long? GameDuration { get; set; }

    [MapTo("level_id")]
    public string? LevelId { get; set; }

    [MapTo("max_wave")]
    public long? MaxWave { get; set; }

    [MapTo("spend_money")]
    public long? SpendMoney { get; set; }

    [MapTo("user_id")]
    public string? UserId { get; set; }
}