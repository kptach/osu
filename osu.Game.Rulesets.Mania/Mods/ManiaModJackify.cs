// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Mania.Mods
{
    public class ManiaModJackify : Mod, IApplicableAfterBeatmapConversion
    {
        [SettingSource("1", "Division 1")]
        public Bindable<bool> OneConversion { get; } = new BindableBool(true);

        [SettingSource("1/2", "Division 2")]
        public Bindable<bool> OneSecondConversion { get; } = new BindableBool(true);

        [SettingSource("1/4", "Division 4")]
        public Bindable<bool> OneThorthConversion { get; } = new BindableBool(true);

        [SettingSource("1/8", "Division 8")]
        public Bindable<bool> OneEighthConversion { get; } = new BindableBool();

        [SettingSource("1/16", "Division 16")]
        public Bindable<bool> OneSixteenConversion { get; } = new BindableBool();

        [SettingSource("1/32", "Division 32")]
        public Bindable<bool> OneThirtyTwoConversion { get; } = new BindableBool();

        [SettingSource("1/64", "Division 64")]
        public Bindable<bool> OneSixteThorthConversion { get; } = new BindableBool();

        public override string Name => "Jackify";
        public override LocalisableString Description => "Convert Hold notes to Jacks.";
        public override double ScoreMultiplier => 1;
        public override string Acronym => "JF";
        public override ModType Type => ModType.Conversion;
        public override Type[] IncompatibleMods => new[] { typeof(ManiaModInvert), typeof(ManiaModNoRelease), typeof(ManiaModHoldOff) };

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            var maniaBeatmap = (ManiaBeatmap)beatmap;

            var newObjects = new List<ManiaHitObject>();




            foreach (var h in beatmap.HitObjects.OfType<HoldNote>())
            {

                double currentStartTime = h.StartTime;


                h.GetEndTime();


                // Add a note for the beginning of the hold note
                newObjects.Add(new Note
                {
                    Column = h.Column,
                    StartTime = h.StartTime,
                    Samples = h.GetNodeSamples(0)
                });
            }

            maniaBeatmap.HitObjects = maniaBeatmap.HitObjects.OfType<Note>().Concat(newObjects).OrderBy(h => h.StartTime).ToList();
        }
    }
}
