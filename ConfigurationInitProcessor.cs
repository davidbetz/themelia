#region Copyright

//+ Nalarium Pro 3.0 - Web Module
//+ Copyright © Jampad Technology, Inc. 2008-2010

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Nalarium.Web.Processing.Configuration;
using Nalarium.Web.Processing.Data;
//+

namespace Nalarium.Web.Processing
{
    internal class ConfigurationInitProcessor : SystemInitProcessor
    {
        internal static ReaderWriterLockSlim ConfigurationReaderWriterLockSlim = new ReaderWriterLockSlim();

        //+
        //- @Info -//

        //+
        //- @OnPreHttpHandlerExecute -//
        public override InitProcessor Execute()
        {
            ProcessingSection cs = ProcessingSection.GetConfigSection();
            if (cs == null)
            {
                return null;
            }
            try
            {
                ConfigurationReaderWriterLockSlim.EnterUpgradeableReadLock();
                //+
                if (SequenceDataList.AllSequenceData == null)
                {
                    ConfigurationReaderWriterLockSlim.EnterWriteLock();
                    try
                    {
                        if (SequenceDataList.AllSequenceData == null)
                        {
                            //+ sequence
                            SequenceDataList.AllSequenceData = new SequenceDataList();
                            ConfigurationLoader.LoadSequenceData(SequenceDataList.AllSequenceData, cs.Sequences);
                        }
                    }
                    finally
                    {
                        ConfigurationReaderWriterLockSlim.ExitWriteLock();
                    }
                }
                if (WebDomainDataList.AllWebDomainData == null)
                {
                    ConfigurationReaderWriterLockSlim.EnterWriteLock();
                    try
                    {
                        if (WebDomainDataList.AllWebDomainData == null)
                        {
                            WebDomainCollection webDomainCollection = cs.WebDomain;
                            var webDomainDataList = new WebDomainDataList();
                            //+ dupe?
                            var duplicate = webDomainCollection.GroupBy(p => p.Name).Select(p => new
                                                                                                 {
                                                                                                     Name = p.Key,
                                                                                                     Count = p.Count()
                                                                                                 }).FirstOrDefault(p => p.Count > 1);
                            if (duplicate != null)
                            {
                                throw new InvalidOperationException(String.Format(Resource.WebDomain_DuplicateName, duplicate.Name));
                            }
                            //+ abstract
                            IEnumerable<WebDomainElement> abstractWebDomainElementEnumerable = webDomainCollection.Where(p => p.IsAbstract);
                            foreach (WebDomainElement element in abstractWebDomainElementEnumerable)
                            {
                                element.ValidateAbstract();
                                ConfigurationLoader.InitWebDomain(element.Name, element, webDomainDataList);
                            }
                            //+ root
                            WebDomainElement rootWebDomainElement = webDomainCollection.FirstOrDefault(p => String.IsNullOrEmpty(p.Name) || p.Name.Equals(Info.Root, StringComparison.OrdinalIgnoreCase));
                            if (rootWebDomainElement != null)
                            {
                                String webDomainName = rootWebDomainElement.Name;
                                if (String.IsNullOrEmpty(webDomainName))
                                {
                                    webDomainName = Info.Root;
                                }
                                else
                                {
                                    webDomainName = webDomainName.ToLower(CultureInfo.CurrentCulture);
                                }
                                ConfigurationLoader.InitWebDomain(webDomainName, rootWebDomainElement, webDomainDataList);
                            }
                            else
                            {
                                ConfigurationLoader.InitWebDomain(Info.Root, new WebDomainElement
                                                                             {
                                                                                 Name = Info.Root,
                                                                                 Path = String.Empty
                                                                             }, webDomainDataList);
                            }
                            if (cs.EnableConfigViewer)
                            {
                                webDomainDataList[Info.Root].EndpointDataList.Add(new EndpointData
                                                                                  {
                                                                                      Type = "ConfigViewer",
                                                                                      Text = "/__viewconfig",
                                                                                      Selector = SelectorType.PathStartsWith
                                                                                  });
                            }
                            if (cs.EnableConfigEditor)
                            {
                                webDomainDataList[Info.Root].EndpointDataList.Add(new EndpointData
                                                                                  {
                                                                                      Type = "ConfigEditor",
                                                                                      Text = "/__editconfig",
                                                                                      Selector = SelectorType.PathStartsWith
                                                                                  });
                            }
                            //+ other
                            IEnumerable<WebDomainElement> concreteWebDomainElementEnumerable = webDomainCollection.Where(p => !p.IsAbstract);
#if DEBUG
                            List<WebDomainElement> list = concreteWebDomainElementEnumerable.ToList();
#endif
                            foreach (WebDomainElement element in concreteWebDomainElementEnumerable)
                            {
                                String webDomainName = element.Name;
                                if (String.IsNullOrEmpty(webDomainName))
                                {
                                    webDomainName = Info.Root;
                                }
                                else
                                {
                                    webDomainName = webDomainName.ToLower(CultureInfo.CurrentCulture);
                                }
                                if (webDomainName != Info.Root)
                                {
                                    ConfigurationLoader.InitWebDomain(element.Name, element, webDomainDataList);
                                }
                            }
                            //+
                            WebDomainDataList.AllWebDomainData = webDomainDataList;
                        }
                    }
                    finally
                    {
                        ConfigurationReaderWriterLockSlim.ExitWriteLock();
                    }
                }
            }
            finally
            {
                ConfigurationReaderWriterLockSlim.ExitUpgradeableReadLock();
            }
            //+
            return null;
        }

        #region Nested type: Info

        public static class Info
        {
            public const String ActiveData = "ActiveData";
            public const String Root = "root";
        }

        #endregion
    }
}