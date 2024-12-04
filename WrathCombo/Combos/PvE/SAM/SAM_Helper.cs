using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using WrathCombo.CustomComboNS;
using WrathCombo.Data;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;

namespace WrathCombo.Combos.PvE;

internal partial class SAM
{
    internal static SAMGauge gauge = GetJobGauge<SAMGauge>();
    internal static SAMOpenerLogic SAMOpener = new();

    internal static int MeikyoUsed => ActionWatching.CombatActions.Count(x => x == MeikyoShisui);

    internal static bool trueNorthReady => TargetNeedsPositionals() && ActionReady(All.TrueNorth) &&
                                           !HasEffect(All.Buffs.TrueNorth);

    internal static float GCD => GetCooldown(Hakaze).CooldownTotal;

    internal class SAMHelper
    {
        internal static int SenCount => GetSenCount();

        internal static bool ComboStarted => GetComboStarted();

        private static int GetSenCount()
        {
            int senCount = 0;
            if (gauge.HasGetsu) senCount++;
            if (gauge.HasSetsu) senCount++;
            if (gauge.HasKa) senCount++;

            return senCount;
        }

        private static unsafe bool GetComboStarted()
        {
            uint comboAction = ActionManager.Instance()->Combo.Action;

            return comboAction == OriginalHook(Hakaze) ||
                   comboAction == OriginalHook(Jinpu) ||
                   comboAction == OriginalHook(Shifu);
        }

        internal static bool UseMeikyo()
        {
            int usedMeikyo = MeikyoUsed % 15;

            if (ActionReady(MeikyoShisui) && !ComboStarted)
            {
                //if no opener/before lvl 100
                if ((IsNotEnabled(CustomComboPreset.SAM_ST_Opener) || !LevelChecked(TendoSetsugekka)) &&
                    MeikyoUsed < 2 && !HasEffect(Buffs.MeikyoShisui) && !HasEffect(Buffs.TsubameReady))
                    return true;

                if (MeikyoUsed >= 2)
                {
                    if (GetCooldownRemainingTime(Ikishoten) is > 45 and < 71) //1min windows
                    {
                        if (usedMeikyo is 1 or 8 && SenCount is 3)
                            return true;

                        if (usedMeikyo is 3 or 10 && SenCount is 2)
                            return true;

                        if (usedMeikyo is 5 or 12 && SenCount is 1)
                            return true;
                    }

                    if (GetCooldownRemainingTime(Ikishoten) > 80) //2min windows
                    {
                        if (usedMeikyo is 2 or 9 && SenCount is 3)
                            return true;

                        if (usedMeikyo is 4 or 11 && SenCount is 2)
                            return true;

                        if (usedMeikyo is 6 or 13 && SenCount is 1)
                            return true;
                    }

                    if (usedMeikyo is 7 or 14 && !HasEffect(Buffs.MeikyoShisui))
                        return true;
                }
            }

            return false;
        }
    }

    internal class SAMOpenerLogic : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            MeikyoShisui,
            All.TrueNorth,
            Gekko,
            Kasha,
            Ikishoten,
            Yukikaze,
            TendoSetsugekka,
            Senei,
            TendoKaeshiSetsugekka,
            MeikyoShisui,
            Gekko,
            Zanshin,
            Higanbana,
            OgiNamikiri,
            Shoha,
            KaeshiNamikiri,
            Kasha,
            Shinten,
            Gekko,
            Gyoten,
            Gyofu,
            Yukikaze,
            Shinten,
            TendoSetsugekka,
            TendoKaeshiSetsugekka
        ];

        public override bool HasCooldowns()
        {
            if (GetRemainingCharges(MeikyoShisui) < 2)
                return false;

            if (GetRemainingCharges(All.TrueNorth) < 2)
                return false;

            if (!ActionReady(Senei))
                return false;

            if (!ActionReady(Ikishoten))
                return false;

            return true;
        }
    }
}