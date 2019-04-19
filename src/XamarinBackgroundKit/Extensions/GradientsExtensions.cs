using System.Collections.Generic;
using System.Linq;
using XamarinBackgroundKit.Controls;

namespace XamarinBackgroundKit.Extensions
{
    public static class GradientsExtensions
    {
        public static bool AreEqual(this IList<GradientStop> source, IList<GradientStop> dest)
        {
            /* No need to update the layer if the gradients are the same */
            if (source == null && dest == null) return true;
            if (source == null || dest == null) return false;
            if (source.Count != dest.Count) return false;
            
            /* Find the different gradients */
            var differencesOneWay = source.Where(x => !dest.Any(y => y.Equals(x))).ToList();
            var differencesSecWay = dest.Where(x => !source.Any(y => y.Equals(x))).ToList();

            /* If no differences found, then return */
            return differencesOneWay.Count == 0 && differencesSecWay.Count == 0;
        }
    }
}
