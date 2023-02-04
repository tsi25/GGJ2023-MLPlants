using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GGJRuntime
{
    public class NeighborStrategyFactory
    {
        private Dictionary<string, Type> strategies;

        public NeighborStrategyFactory()
        {
            LoadTypesIFindNeighborStrategy();
        }


        public IFindNeighborStrategy CreateInstance(string strategyName)
        {
            Type type = GetTypeToCreate(strategyName);

            if(type == null)
            {
                type = GetTypeToCreate("more");
            }

            return Activator.CreateInstance(type) as IFindNeighborStrategy;
        }

        private Type GetTypeToCreate(string strategyName)
        {
            foreach(var strategy in strategies)
            {
                if(strategy.Key.Contains(strategyName, StringComparison.OrdinalIgnoreCase))
                {
                    return strategy.Value;
                }
            }

            return null;
        }


        private void LoadTypesIFindNeighborStrategy()
        {
            strategies = new Dictionary<string, Type>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach(Type type in types)
            {
                if(type.GetInterface(typeof(IFindNeighborStrategy).ToString()) != null)
                {
                    strategies.Add(type.Name, type);
                }
            }
        }
    }
}