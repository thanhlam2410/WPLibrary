using System;
using System.Collections.Generic;

namespace WPCore.Patterns
{
    public class ServiceLocator
    {
        #region Fields
        private Dictionary<object, object> serviceMapTable;
        #endregion

        #region Methods
        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceLocator()
        {
            serviceMapTable = new Dictionary<object, object>();
        }

        /// <summary>
        /// Register class to ServiceLocator
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="obj"></param>
        public void RegisterService<TInterface, TClass>() where TClass : TInterface, new()
        {
            if (serviceMapTable.ContainsKey(typeof(TInterface)))
            {
                return;
            }

            serviceMapTable.Add(typeof(TInterface), new Lazy<TInterface>(() =>
            {
                return new TClass();
            },
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication));
        }

        /// <summary>
        /// Register class to ServiceLocator
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="obj"></param>
        public void RegisterService<TClass>() where TClass : new()
        {
            if (serviceMapTable.ContainsKey(typeof(TClass)))
            {
                return;
            }

            serviceMapTable.Add(typeof(TClass), new Lazy<TClass>(() =>
            {
                return new TClass();
            },
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication));
        }

        /// <summary>
        /// Get a service that are registered with ServiceLocator
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface GetService<TInterface>()
        {
            if (serviceMapTable.ContainsKey(typeof(TInterface)))
            {
                object obj = serviceMapTable[typeof(TInterface)];
                Lazy<TInterface> lazyObj = obj as Lazy<TInterface>;

                if (lazyObj != null)
                {
                    return lazyObj.Value;
                }
            }

            return default(TInterface);
        }

        /// <summary>
        /// Unregister a service from ServiceLocator
        /// </summary>
        public void UnregisterService<TInterface>()
        {
            if (!serviceMapTable.ContainsKey(typeof(TInterface)))
            {
                return;
            }

            serviceMapTable.Remove(typeof(TInterface));
        }
        #endregion
    }
}
