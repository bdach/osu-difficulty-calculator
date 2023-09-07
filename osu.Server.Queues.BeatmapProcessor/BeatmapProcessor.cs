// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Dapper;
using osu.Server.DifficultyCalculator;
using osu.Server.QueueProcessor;

namespace osu.Server.Queues.BeatmapProcessor
{
    internal class BeatmapProcessor : QueueProcessor<BeatmapItem>
    {
        private readonly ServerDifficultyCalculator calculator;

        public BeatmapProcessor()
            : base(new QueueConfiguration
            {
                InputQueueName = "beatmap",
                MaxInFlightItems = 4,
            })
        {
            calculator = new ServerDifficultyCalculator(new[] { 0, 1, 2, 3 });
        }

        protected override void ProcessResult(BeatmapItem item)
        {
            using (var db = GetDatabaseConnection())
            {
                var beatmaps = db.Query<long>("SELECT beatmap_id FROM osu_beatmaps WHERE beatmapset_id = @beatmapset_id AND deleted_at IS NULL", item);

                foreach (long beatmapId in beatmaps)
                {
                    var working = BeatmapLoader.GetBeatmap((int)beatmapId);

                    // ensure the correct online id is set
                    working.BeatmapInfo.OnlineID = (int)beatmapId;

                    calculator.ProcessDifficulty(working);
                }
            }
        }
    }
}
