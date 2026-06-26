using WrathCombo.API.Enum;
using WrathCombo.Services.IPC;

namespace WrathCombo.CustomComboNS.Functions;

internal abstract partial class CustomComboFunctions
{
    /// <summary>
    ///     Reports an upcoming positional requirement for external overlay plugins.
    /// </summary>
    internal static void ReportUpcomingPositional(
        PositionalDirection direction,
        uint actionId,
        int gcdsUntil,
        bool preferOverCurrent = false) =>
        UpcomingPositionalHintService.Report(direction, actionId, gcdsUntil, preferOverCurrent);
}
