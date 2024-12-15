#region

using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using WrathCombo.Combos.PvE.Content;
using WrathCombo.CustomComboNS;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;

#endregion

namespace WrathCombo.Combos.PvE;

internal static partial class DRG
{
    public static DRGOpenerLogic DRGOpener = new();

    // DRG Gauge & Extensions
    public static DRGGauge Gauge => GetJobGauge<DRGGauge>();

    public static Status? ChaosDoTDebuff =>
        FindTargetEffect(LevelChecked(ChaoticSpring)
            ? Debuffs.ChaoticSpring
            : Debuffs.ChaosThrust);

    public static bool trueNorthReady =>
        TargetNeedsPositionals() && ActionReady(All.TrueNorth) &&
        !HasEffect(All.Buffs.TrueNorth);

    internal class DRGOpenerLogic : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            TrueThrust,
            SpiralBlow,
            LanceCharge,
            ChaoticSpring,
            BattleLitany,
            Geirskogul,
            WheelingThrust,
            HighJump,
            LifeSurge,
            Drakesbane,
            DragonfireDive,
            Nastrond,
            RaidenThrust,
            Stardiver,
            LanceBarrage,
            Starcross,
            LifeSurge,
            HeavensThrust,
            RiseOfTheDragon,
            MirageDive,
            FangAndClaw,
            Drakesbane,
            RaidenThrust,
            WyrmwindThrust
        ];

        public override bool HasCooldowns()
        {
            if (GetRemainingCharges(LifeSurge) < 2)
                return false;

            if (!ActionReady(BattleLitany))
                return false;

            if (!ActionReady(DragonfireDive))
                return false;

            if (!ActionReady(LanceCharge))
                return false;

            return true;
        }
    }

    internal class DRGHelper
    {
        internal static readonly List<uint> FastLocks =
        [
            BattleLitany,
            LanceCharge,
            LifeSurge,
            Geirskogul,
            Nastrond,
            MirageDive,
            WyrmwindThrust,
            RiseOfTheDragon,
            Starcross,
            Variant.VariantRampart,
            All.TrueNorth
        ];

        internal static readonly List<uint> MidLocks =
        [
            Jump,
            HighJump,
            DragonfireDive
        ];

        internal static uint SlowLock => Stardiver;

        internal static bool CanDRGWeave(uint oGCD)
        {
            float gcdTimer = GetCooldownRemainingTime(TrueThrust);

            //GCD Ready - No Weave
            if (IsOffCooldown(TrueThrust))
                return false;

            if (FastLocks.Any(x => x == oGCD) && gcdTimer >= 0.6f)
                return true;

            if (MidLocks.Any(x => x == oGCD) && gcdTimer >= 0.8f)
                return true;

            if (SlowLock == oGCD && gcdTimer >= 1.5f)
                return true;

            return false;
        }
    }
}