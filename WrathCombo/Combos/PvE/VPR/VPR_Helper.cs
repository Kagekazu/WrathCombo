using System.Collections.Generic;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using WrathCombo.CustomComboNS;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;

namespace WrathCombo.Combos.PvE;

internal partial class VPR
{
    public static VPROpenerLogic VPROpener = new();

    public static VPRGauge gauge = GetJobGauge<VPRGauge>();

    // VPR Gauge & Extensions

    public static float GCD => GetCooldown(OriginalHook(ReavingFangs)).CooldownTotal;

    public static float ireCD => GetCooldownRemainingTime(SerpentsIre);

    public static bool trueNorthReady => TargetNeedsPositionals() && ActionReady(All.TrueNorth) &&
                                         !HasEffect(All.Buffs.TrueNorth);

    public static bool VicewinderReady => gauge.DreadCombo == DreadCombo.Dreadwinder;

    public static bool HuntersCoilReady => gauge.DreadCombo == DreadCombo.HuntersCoil;

    public static bool SwiftskinsCoilReady => gauge.DreadCombo == DreadCombo.SwiftskinsCoil;

    public static bool VicepitReady => gauge.DreadCombo == DreadCombo.PitOfDread;

    public static bool SwiftskinsDenReady => gauge.DreadCombo == DreadCombo.SwiftskinsDen;

    public static bool HuntersDenReady => gauge.DreadCombo == DreadCombo.HuntersDen;

    public static bool CappedOnCoils =>
        (TraitLevelChecked(Traits.EnhancedVipersRattle) && gauge.RattlingCoilStacks > 2) ||
        (!TraitLevelChecked(Traits.EnhancedVipersRattle) && gauge.RattlingCoilStacks > 1);

    public static bool HasRattlingCoilStack(VPRGauge Gauge) => gauge.RattlingCoilStacks > 0;

    internal class VPROpenerLogic : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            ReavingFangs,
            SerpentsIre,
            SwiftskinsSting,
            Vicewinder,
            HuntersCoil,
            TwinfangBite,
            TwinbloodBite,
            SwiftskinsCoil,
            TwinbloodBite,
            TwinfangBite,
            Reawaken,
            FirstGeneration,
            FirstLegacy,
            SecondGeneration,
            SecondLegacy,
            ThirdGeneration,
            ThirdLegacy,
            FourthGeneration,
            FourthLegacy,
            Ouroboros,
            UncoiledFury,
            UncoiledTwinfang,
            UncoiledTwinblood,
            UncoiledFury,
            UncoiledTwinfang,
            UncoiledTwinblood,
            HindstingStrike,
            DeathRattle,
            Vicewinder,
            UncoiledFury,
            UncoiledTwinfang,
            UncoiledTwinblood,
            HuntersCoil,
            TwinfangBite,
            TwinbloodBite,
            SwiftskinsCoil,
            TwinbloodBite,
            TwinfangBite
        ];

        public override bool HasCooldowns()
        {
            if (GetRemainingCharges(Vicewinder) < 2)
                return false;

            if (!ActionReady(SerpentsIre))
                return false;

            return true;
        }
    }

    internal class VPRHelper
    {
        public static bool UseReawaken(VPRGauge gauge)
        {
            float ireCD = GetCooldownRemainingTime(SerpentsIre);

            if (LevelChecked(Reawaken) && !HasEffect(Buffs.Reawakened) && InActionRange(Reawaken) &&
                !HasEffect(Buffs.HuntersVenom) && !HasEffect(Buffs.SwiftskinsVenom) &&
                !HasEffect(Buffs.PoisedForTwinblood) && !HasEffect(Buffs.PoisedForTwinfang) &&
                !IsEmpowermentExpiring(6))
                if ((!JustUsed(SerpentsIre, 2.2f) && HasEffect(Buffs.ReadyToReawaken)) || //2min burst
                    (WasLastWeaponskill(Ouroboros) && gauge.SerpentOffering >= 50 && ireCD >= 50) || //2nd RA
                    (gauge.SerpentOffering is >= 50 and <= 80 && ireCD is >= 50 and <= 62) || //1min
                    gauge.SerpentOffering >= 100 || //overcap
                    (gauge.SerpentOffering >= 50 && WasLastWeaponskill(FourthGeneration) &&
                     !LevelChecked(Ouroboros))) //<100
                    return true;

            return false;
        }

        public static bool IsHoningExpiring(float Times)
        {
            float GCD = GetCooldown(SteelFangs).CooldownTotal * Times;

            return (HasEffect(Buffs.HonedSteel) && GetBuffRemainingTime(Buffs.HonedSteel) < GCD) ||
                   (HasEffect(Buffs.HonedReavers) && GetBuffRemainingTime(Buffs.HonedReavers) < GCD);
        }

        public static bool IsVenomExpiring(float Times)
        {
            float GCD = GetCooldown(SteelFangs).CooldownTotal * Times;

            return (HasEffect(Buffs.FlankstungVenom) && GetBuffRemainingTime(Buffs.FlankstungVenom) < GCD) ||
                   (HasEffect(Buffs.FlanksbaneVenom) && GetBuffRemainingTime(Buffs.FlanksbaneVenom) < GCD) ||
                   (HasEffect(Buffs.HindstungVenom) && GetBuffRemainingTime(Buffs.HindstungVenom) < GCD) ||
                   (HasEffect(Buffs.HindsbaneVenom) && GetBuffRemainingTime(Buffs.HindsbaneVenom) < GCD);
        }

        public static bool IsEmpowermentExpiring(float Times)
        {
            float GCD = GetCooldown(SteelFangs).CooldownTotal * Times;

            return GetBuffRemainingTime(Buffs.Swiftscaled) < GCD || GetBuffRemainingTime(Buffs.HuntersInstinct) < GCD;
        }

        public static unsafe bool IsComboExpiring(float Times)
        {
            float GCD = GetCooldown(SteelFangs).CooldownTotal * Times;

            return ActionManager.Instance()->Combo.Timer != 0 && ActionManager.Instance()->Combo.Timer < GCD;
        }
    }
}