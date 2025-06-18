// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Mania.Mods
{
    public class ManiaModRandom : ModRandom, IApplicableToBeatmap
    {
        public override LocalisableString Description => @"Shuffle around the notes!";

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            Seed.Value ??= RNG.Next();
            var rng = new Random((int)Seed.Value);
            var maniaBeatmap = (ManiaBeatmap)beatmap;
            int availableColumns = maniaBeatmap.TotalColumns;

            double[] columnEndTimes = new double[availableColumns];
            double lastStartTime = -1;
            var availableColumnsList = new List<int>();

            const double release_buffer = 1.5; // Minimum gap to avoid conflict at end of HoldNote

            foreach (var h in beatmap.HitObjects.OfType<ManiaHitObject>())
            {
                double currentStartTime = h.StartTime;

                if (currentStartTime != lastStartTime)
                {
                    availableColumnsList = Enumerable.Range(0, availableColumns)
                                                     .Where(i => columnEndTimes[i] < currentStartTime - release_buffer)
                                                     .ToList();
                }

                if (availableColumnsList.Count == 0)
                    continue; // Skip if no free columns for aspire maps

                int randomIndex = rng.Next(availableColumnsList.Count);
                int randomColumn = availableColumnsList[randomIndex];

                h.Column = randomColumn;
                availableColumnsList.Remove(randomColumn);

                if (h is HoldNote hold)
                    columnEndTimes[randomColumn] = hold.GetEndTime();

                lastStartTime = currentStartTime;
            }

            maniaBeatmap.HitObjects = maniaBeatmap.HitObjects.OrderBy(h => h.StartTime).ToList();
        }
    }
}
