﻿using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace CrawlLeague.ServiceModel.Types
{
    public enum RuneType
    {
        Abyssal,
        Barnacled,
        Bone,
        Dark,
        Decaying,
        Demonic,
        Fiery,
        Glowing,
        Golden,
        Gossamer,
        Icy,
        Iron,
        Magical,
        Obsidian,
        Serpentine,
        Silver,
        Slimy
    }

    [CompositeIndex("GameId", "Type")] // Only 1 rune type per game.
    public class Rune
    {
        [IgnoreDataMember]
        [AutoIncrement]
        public int Id { get; set; }

        [Description("Id of the Game")]        
        [References(typeof(Game))]
        [IgnoreDataMember]
        public int GameId { get; set; }

        [Description("Type of the rune")]
        public RuneType Type { get; set; }
    }
}
