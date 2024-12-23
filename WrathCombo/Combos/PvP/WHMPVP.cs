﻿using WrathCombo.CustomComboNS;

namespace WrathCombo.Combos.PvP
{
    internal static class WHMPvP
    {
        public const byte JobID = 24;

        public const uint
            Glare = 29223,
            Cure2 = 29224,
            Cure3 = 29225,
            AfflatusMisery = 29226,
            Aquaveil = 29227,
            MiracleOfNature = 29228,
            SeraphStrike = 29229;

        internal class Buffs
        {
            internal const ushort
                Cure3Ready = 3083,
                SacredSight = 4326;
        }

        internal class WHMPvP_Burst : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHMPvP_Burst;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Glare)
                {
                    if (!PvPCommon.TargetImmuneToDamage())
                    {
                        // Afflatus Misery if enabled and off cooldown
                        if (IsEnabled(CustomComboPreset.WHMPvP_Afflatus_Misery) && IsOffCooldown(AfflatusMisery))
                            return AfflatusMisery;

                        // Seraph Strike if Sacred Sight is active
                        if (IsEnabled(CustomComboPreset.WHMPvP_Glare4) && HasEffect(Buffs.SacredSight))
                            return OriginalHook(SeraphStrike);

                        // Weave conditions
                        if (CanWeave())
                        {
                            // Miracle of Nature if enabled and off cooldown and inrange 
                            if (IsEnabled(CustomComboPreset.WHMPvP_Mirace_of_Nature) && IsOffCooldown(MiracleOfNature) && InActionRange(MiracleOfNature))
                                return MiracleOfNature;

                            // Seraph Strike if enabled and off cooldown
                            if (IsEnabled(CustomComboPreset.WHMPvP_Seraph_Strike) && IsOffCooldown(SeraphStrike))
                                return SeraphStrike;
                        }
                    }
                }

                return actionID;
            }
        }
        internal class WHMPvP_Aquaveil : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHMPvP_Aquaveil;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Cure2 && IsOffCooldown(Aquaveil))
                    return Aquaveil;

                return actionID;
            }
        }

        internal class WHMPvP_Cure3 : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHMPvP_Cure3;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Cure2 && HasEffect(Buffs.Cure3Ready))
                    return Cure3;

                return actionID;
            }
        }
    }
}
