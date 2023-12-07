
using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions {

    public static T RandomFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
        return source.Where(predicate).OrderBy(i => Guid.NewGuid()).FirstOrDefault();
    }

    public static int FirstIndexMatch<T>(this IEnumerable<T> items, Func<T, bool> matchCondition) {
        var index = 0;
        foreach (var item in items) {
            if (matchCondition.Invoke(item)) {
                return index;
            }

            index++;
        }
        
        return -1;
    }
}
