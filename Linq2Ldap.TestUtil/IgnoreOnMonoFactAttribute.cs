using System;
using Xunit;

namespace Linq2Ldap.TestUtil
{
    // https://stackoverflow.com/questions/4421328/how-do-i-skip-specific-tests-in-xunit-based-on-current-platform
    public class IgnoreOnMonoFactAttribute : FactAttribute {

        public IgnoreOnMonoFactAttribute() {
            if(IsRunningOnMono()) {
                Skip = "Ignored on Mono";
            }
        }
        /// <summary>
        /// Determine if runtime is Mono.
        /// Taken from http://stackoverflow.com/questions/721161
        /// </summary>
        /// <returns>True if being executed in Mono, false otherwise.</returns>
        public static bool IsRunningOnMono() {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
