﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Examine.LuceneEngine.Config;
using Examine.LuceneEngine.Providers;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Examine.Azure
{
    public interface IAzureCatalogue
    {
        string Catalogue { get; }
    }

    public static class AzureSetupExtensions
    {

        public static void EnsureAzureConfig()
        {
            // get settings from azure settings or app.config
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                try
                {
                    configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                }
                catch (Exception)
                {
                    // for a console app, reading from App.config
                    configSetter(System.Configuration.ConfigurationManager.AppSettings[configName]);
                }
            });
        }

        public static Lucene.Net.Store.Directory GetAzureDirectory(this IAzureCatalogue searcher)
        {
            var azureDirectory = new AzureDirectory(CloudStorageAccount.FromConfigurationSetting("blobStorage"), searcher.Catalogue);
            return azureDirectory;
        }

        public static IndexWriter GetAzureIndexWriter(this LuceneIndexer indexer)
        {
            var writer = indexer.GetIndexWriter();
            writer.SetRAMBufferSizeMB(10.0);
            writer.SetUseCompoundFile(false);
            writer.SetMaxMergeDocs(10000);
            writer.SetMergeFactor(100);
            return writer;
        }

        public static void SetOptimizationThresholdOnInit(this LuceneIndexer indexer, System.Collections.Specialized.NameValueCollection config)
        {
            if (config["autoOptimizeCommitThreshold"] == null)
            {
                //by default we need a higher threshold according to lucene azure docs
                indexer.OptimizationCommitThreshold = 1000;
            }
            else
            {
                int autoCommitThreshold;
                if (int.TryParse(config["autoOptimizeCommitThreshold"], out autoCommitThreshold))
                {
                    indexer.OptimizationCommitThreshold = autoCommitThreshold;
                }
                else
                {
                    throw new FormatException("Could not parse autoCommitThreshold value into an integer");
                }
            }
        }

    }
}
