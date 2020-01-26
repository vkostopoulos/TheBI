using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb {
    public static class ReClasser {
        public static dynamic FixMeUp<T>(this T fixMe) {
            var t = fixMe.GetType();
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in t.GetProperties()) {
                var val = pr.GetValue(fixMe);
                if (val is string && string.IsNullOrWhiteSpace(val.ToString())) {
                }
                else if (val == null) {
                }
                else {
                    returnClass.Add(pr.Name, val);
                }
            }
            return returnClass;
        }
    }
}
