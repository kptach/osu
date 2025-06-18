// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Mania.Mods
{
    public class ManiaModRearrange : Mod, IApplicableToBeatmap, IHasSeed
    {
        public override string Name => "Rearrange";
        public override string Acronym => "RG";
        public override ModType Type => ModType.Conversion;

        public override IconUsage? Icon => OsuIcon.Dice;
        public override LocalisableString Description => @"Rearrange the keys randomly or manually!";
        public override double ScoreMultiplier => 1;

        [SettingSource("Seed", "Use a custom seed instead of a random one", SettingControlType = typeof(SettingsNumberBox))]
        public Bindable<int?> Seed { get; } = new Bindable<int?>();

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            Seed.Value ??= RNG.Next();
            var rng = new Random((int)Seed.Value);
            var maniaBeatmap = (ManiaBeatmap)beatmap;
            int availableColumns = maniaBeatmap.TotalColumns;
            var shuffledColumns = Enumerable.Range(0, availableColumns).OrderBy(_ => rng.Next()).ToList();

            beatmap.HitObjects.OfType<ManiaHitObject>().ForEach(h => h.Column = shuffledColumns[h.Column]);
        }
    }
}
