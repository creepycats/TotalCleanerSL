using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCleaner.Types
{
    public class BroadcastConfig
    {
        /// <summary>
        ///  How should Cleanup Information be shared? (Broadcast, Hint, None)
        /// </summary>
        [Description("How should Cleanup Information be shared? (Broadcast, Hint, None)")]
        public CleanupMsgChannel CleanupInfoChannel { get; set; } = CleanupMsgChannel.Broadcast;
        /// <summary>
        ///  What will the cleanup broadcast's prefix be? Defaults to Plugin Name. Leave blank for No Prefix
        /// </summary>
        [Description("What will the cleanup broadcast's prefix be? Defaults to Plugin Name. Leave blank for No Prefix")]
        public string CleanupInfoPrefix { get; set; } = "[<color=#4BE2D5>T</color><color=#55D8D6>o</color><color=#5FCED7>t</color><color=#69C4D8>a</color><color=#73BAD9>l</color><color=#7DB0DA>C</color><color=#87A6DB>l</color><color=#919CDC>e</color><color=#9B92DD>a</color><color=#A588DE>n</color><color=#AF7EDF>e</color><color=#B974E0>r</color>]";
        /// <summary>
        ///  What will the cleaner message show when cleaning large amounts of items? Variables: {items} {ragdolls}
        /// </summary>
        [Description("What will the cleaner message show when cleaning large amounts of items? Variables: {items} {ragdolls}")]
        public string CleanupMessage { get; set; } = "<size=30>Cleanup   |   {items} Items Removed   |   {ragdolls} Ragdolls Removed</size>";
    }
}
