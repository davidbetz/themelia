using System;

namespace Nalarium.Web.Processing.Sequence
{
    public interface IInitializer
    {
        /// <summary>
        /// Used to choose the initial sequence view.
        /// </summary>
        /// <returns>Name of initial view.</returns>
        String SelectInitView();
    }
}