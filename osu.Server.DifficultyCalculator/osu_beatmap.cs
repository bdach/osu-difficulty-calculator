// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;

namespace osu.Server.DifficultyCalculator
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Serializable]
    [Table("osu_beatmaps")]
    public class osu_beatmap
    {
        [ExplicitKey]
        public uint beatmap_id { get; set; }

        public float diff_size { get; set; }
        public byte playmode { get; set; }
        public string version { get; set; } = string.Empty;
        public float difficultyrating { get; set; }
    }
}
