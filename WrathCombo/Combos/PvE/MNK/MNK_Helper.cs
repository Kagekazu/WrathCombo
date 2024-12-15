#region

using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using WrathCombo.CustomComboNS;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;

#endregion

namespace WrathCombo.Combos.PvE;

internal static partial class MNK
{
    public static MNKOpenerLogicSL MNKOpenerSL = new();
    public static MNKOpenerLogicLL MNKOpenerLL = new();

    public static MNKGauge Gauge = GetJobGauge<MNKGauge>();

    // MNK Gauge & Extensions
    public static float GCD => GetCooldown(OriginalHook(Bootshine)).CooldownTotal;

    public static bool bothNadisOpen => Gauge.Nadi.ToString() == "LUNAR, SOLAR";

    public static bool solarNadi => Gauge.Nadi == Nadi.SOLAR;

    public static bool lunarNadi => Gauge.Nadi == Nadi.LUNAR;

    public static int opoOpoChakra => Gauge.BeastChakra.Count(x => x == BeastChakra.OPOOPO);

    public static int raptorChakra => Gauge.BeastChakra.Count(x => x == BeastChakra.RAPTOR);

    public static int coeurlChakra => Gauge.BeastChakra.Count(x => x == BeastChakra.COEURL);

    internal class MNKHelper
    {
        public static uint DetermineCoreAbility(uint actionId, bool useTrueNorthIfEnabled)
        {
            if (HasEffect(Buffs.OpoOpoForm) || HasEffect(Buffs.FormlessFist))
                return Gauge.OpoOpoFury == 0 && LevelChecked(DragonKick)
                    ? DragonKick
                    : OriginalHook(Bootshine);

            if (HasEffect(Buffs.RaptorForm))
                return Gauge.RaptorFury == 0 && LevelChecked(TwinSnakes)
                    ? TwinSnakes
                    : OriginalHook(TrueStrike);

            if (HasEffect(Buffs.CoeurlForm))
            {
                if (Gauge.CoeurlFury == 0 && LevelChecked(Demolish))
                {
                    if (!OnTargetsRear() &&
                        TargetNeedsPositionals() &&
                        !HasEffect(Buffs.TrueNorth) &&
                        ActionReady(TrueNorth) &&
                        useTrueNorthIfEnabled)
                        return TrueNorth;

                    return Demolish;
                }

                if (LevelChecked(SnapPunch))
                {
                    if (!OnTargetsFlank() &&
                        TargetNeedsPositionals() &&
                        !HasEffect(Buffs.TrueNorth) &&
                        ActionReady(TrueNorth) &&
                        useTrueNorthIfEnabled)
                        return TrueNorth;

                    return OriginalHook(SnapPunch);
                }
            }

            return actionId;
        }

        public static bool UsePerfectBalance()
        {
            if (ActionReady(PerfectBalance) && !HasEffect(Buffs.PerfectBalance) && !HasEffect(Buffs.FormlessFist))
            {
                // Odd window
                if ((JustUsed(OriginalHook(Bootshine)) || JustUsed(DragonKick)) &&
                    !JustUsed(PerfectBalance, 20) &&
                    HasEffect(Buffs.RiddleOfFire) && !HasEffect(Buffs.Brotherhood))
                    return true;

                // Even window
                if ((JustUsed(OriginalHook(Bootshine)) || JustUsed(DragonKick)) &&
                    (GetCooldownRemainingTime(Brotherhood) <= GCD * 3 || HasEffect(Buffs.Brotherhood)) &&
                    (GetCooldownRemainingTime(RiddleOfFire) <= GCD * 3 || HasEffect(Buffs.RiddleOfFire)))
                    return true;

                // Low level
                if ((JustUsed(OriginalHook(Bootshine)) || JustUsed(DragonKick)) &&
                    ((HasEffect(Buffs.RiddleOfFire) && !LevelChecked(Brotherhood)) ||
                     !LevelChecked(RiddleOfFire)))
                    return true;
            }

            return false;
        }
    }

    #region Openers

    internal class MNKOpenerLogicSL : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            PerfectBalance,
            TwinSnakes,
            Demolish,
            Brotherhood,
            RiddleOfFire,
            LeapingOpo,
            TheForbiddenChakra,
            RiddleOfWind,
            RisingPhoenix,
            DragonKick,
            WindsReply,
            FiresReply,
            LeapingOpo,
            PerfectBalance,
            DragonKick,
            LeapingOpo,
            DragonKick,
            ElixirBurst,
            LeapingOpo
        ];

        public override bool HasCooldowns()
        {
            if (GetRemainingCharges(PerfectBalance) < 2)
                return false;

            if (!ActionReady(Brotherhood))
                return false;

            if (!ActionReady(RiddleOfFire))
                return false;

            if (!ActionReady(RiddleOfWind))
                return false;

            if (Gauge.Nadi != Nadi.NONE)
                return false;

            if (Gauge.RaptorFury != 0)
                return false;

            if (Gauge.CoeurlFury != 0)
                return false;

            return true;
        }
    }

    internal class MNKOpenerLogicLL : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            DragonKick,
            PerfectBalance,
            LeapingOpo,
            DragonKick,
            Brotherhood,
            RiddleOfFire,
            LeapingOpo,
            TheForbiddenChakra,
            RiddleOfWind,
            ElixirBurst,
            DragonKick,
            WindsReply,
            FiresReply,
            LeapingOpo,
            PerfectBalance,
            DragonKick,
            LeapingOpo,
            DragonKick,
            ElixirBurst,
            LeapingOpo
        ];

        public override bool HasCooldowns()
        {
            if (GetRemainingCharges(PerfectBalance) < 2)
                return false;

            if (!ActionReady(Brotherhood))
                return false;

            if (!ActionReady(RiddleOfFire))
                return false;

            if (!ActionReady(RiddleOfWind))
                return false;

            if (Gauge.Nadi != Nadi.NONE)
                return false;

            if (Gauge.RaptorFury != 0)
                return false;

            if (Gauge.CoeurlFury != 0)
                return false;

            return true;
        }
    }

    #endregion
}