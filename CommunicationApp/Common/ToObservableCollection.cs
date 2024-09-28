using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationApp.Common
{
    public static class ToObservableCollection
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> value) =>
            new ObservableCollection<T>(value);
    }
}
