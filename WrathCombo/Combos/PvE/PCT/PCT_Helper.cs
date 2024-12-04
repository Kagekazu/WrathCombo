using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons.DalamudServices;
using WrathCombo.Combos.JobHelpers.Enums;
using WrathCombo.CustomComboNS.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Statuses;
using WrathCombo.CustomComboNS;
using WrathCombo.Data;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;
using static WrathCombo.Combos.PvE.MCH;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WrathCombo.Combos.PvE;

internal partial class PCT
{
    public static PCTopener1 PCTOpener = new();

    public static PCTGauge Gauge = GetJobGauge<PCTGauge>();

    public static bool canWeave => CanSpellWeave(ActionWatching.LastSpell);

    public static bool HasMotifs()
    {

        if (!Gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Pom))
            return false;

        if (!Gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Weapon))
            return false;

        if (!Gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Landscape))
            return false;

        return true;
    }

    internal class PCTopener1 : WrathOpener
    {
        public override int OpenerLevel => 100;

        public override List<uint> OpenerActions { get; protected set; } =
        [
            RainbowDrip,
            StrikingMuse,
            HolyInWhite,
            PomMuse,
            WingMotif,
            StarryMuse,
            HammerStamp,
            SubtractivePalette,
            BlizzardinCyan,
            StoneinYellow,
            ThunderinMagenta,
            CometinBlack,
            WingedMuse,
            MogoftheAges,
            StarPrism,
            HammerBrush,
            PolishingHammer,
            RainbowDrip

        ];

        public override bool HasCooldowns()
        {
            if (!ActionReady(StarryMuse))
                return false;

            if (GetRemainingCharges(LivingMuse) < 3)
                return false;

            if (GetRemainingCharges(SteelMuse) < 2)
                return false;

            if (!HasMotifs())
                return false;

            if (HasEffect(Buffs.SubtractivePalette))
                return false;

            return true;
        }

    }
}