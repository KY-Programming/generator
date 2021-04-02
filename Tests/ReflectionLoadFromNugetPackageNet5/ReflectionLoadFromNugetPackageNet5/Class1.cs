using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using KY.Generator;

namespace ReflectionLoadFromNugetPackageNet5
{
    [Generate(OutputLanguage.TypeScript, "Output")]
    public class Class1
    {
        public string Test { get; set; }

        public IObservable<int> Count()
        {
            return Observable.Create<int>(o => Task.Run(() =>
            {
                o.OnNext(1);
                o.OnCompleted();
            }));
        }
    }
}
