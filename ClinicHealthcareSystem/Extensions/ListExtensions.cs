using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        ///     To the observable collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
