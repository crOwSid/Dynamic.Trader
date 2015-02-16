﻿using System;
using System.IO;
using StructureMap.Configuration.DSL;
using Trader.Client.Infrastucture;
using Trader.Domain.Infrastucture;
using Trader.Domain.Services;
using ILogger = Trader.Domain.Infrastucture.ILogger;

namespace Trader.Client.Infrastucture
{
    internal class AppRegistry : Registry
    {
        public AppRegistry()
        {

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (!File.Exists(path))
                throw new FileNotFoundException("The log4net.config file was not found" + path);

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));

            For<ILogger>().Use<Log4NetLogger>().Ctor<Type>("type").Is(x => x.RootType);
            For<ISchedulerProvider>().Singleton().Use<SchedulerProvider>();
            For<IObjectProvider>().Singleton().Use<ObjectProvider>();
            For<ITradeService>().Singleton().Use<TradeService>();
            For<IStaticData>().Singleton().Use<StaticData>();
            For<IMarketDataService>().Singleton().Use<MarketDataService>();
            For<INearToMarketService>().Singleton().Use<NearToMarketService>();
           
            For<ILogEntryService>().Singleton().Use<LogEntryService>();
            
            For<UnhandledExceptionEventHandler>().Singleton();
            For<TradePriceUpdateJob>().Singleton();
            For<LogWriter>().Singleton();


            Scan(scanner => scanner.LookForRegistries());
        }
    }
}

