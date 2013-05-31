using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Resources;

namespace Orchestra.ViewModels
{
    /// <summary>
    /// Represents dynamic item of list from texts resource source.
    /// </summary>
    public class DynamicItemTextsSource : DynamicTextsSourceBase
    {
        /// <summary>
        /// Indicates that dynamic texts source is in collection context.
        /// </summary>
        private string _collectionName;

        /// <summary>
        /// Index of collection context
        /// </summary>
        private int _indexIndicator;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="collectionName">Collection name as resource key prefix.</param>
        /// <param name="indexIndicator">Index of dynamic text item in collection</param>
        public DynamicItemTextsSource(string collectionName, int indexIndicator)
        {
            this._indexIndicator = indexIndicator;
            this._collectionName = collectionName;
        }

        /// <summary>
        /// Tries to get text from resource by invoking dynamic property.
        /// </summary>
        /// <param name="binder">Information about property binder.</param>
        /// <param name="result">Value of dynamic property.</param>
        /// <returns>Always true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return GetDynamicStringProperty(
                string.Format("{0}_{1}_{2}", _collectionName, _indexIndicator, binder.Name), out result);
        }
    }

    /// <summary>
    /// Dynamic object for returning localized texts from resources.
    /// For XAML data binding.
    /// </summary>
    public class DynamicTextsSource : DynamicTextsSourceBase
    {
        /// <summary>
        /// Constant fro indication collection property.
        /// </summary>
        private const string STR_Collection = "#Collection";

        /// <summary>
        /// Tries to get text from resource by invoking dynamic property.
        /// </summary>
        /// <param name="binder">Information about property.</param>
        /// <param name="result">Value of dynamic property.</param>
        /// <returns>Always true</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name.IndexOf(STR_Collection) > 0)
            {
                return GetCollectionProperty(binder, out result);
            }
            return GetDynamicStringProperty(binder.Name, out result);
        }

        /// <summary>
        /// Get items count for dynamic text collection
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <returns>Collection count as int</returns>
        private static int GetCollectionCount(string collectionName)
        {
            object count;
            GetDynamicStringProperty(collectionName + "_Count", out count);
            var collectionCountIndicator = Convert.ToInt32(count);
            return collectionCountIndicator;
        }

        /// <summary>
        /// Gets collection property
        /// </summary>
        /// <param name="binder">Information about property</param>
        /// <param name="result">Value of dynamic collection.</param>
        /// <returns>Always true</returns>
        private static bool GetCollectionProperty(GetMemberBinder binder, out object result)
        {
            var collectionName = binder.Name.Replace(STR_Collection, "");
            int collectionCount = GetCollectionCount(collectionName);
            var resultList = new List<DynamicItemTextsSource>();
            for (int i = 0; i < collectionCount; i++)
            {
                resultList.Add(new DynamicItemTextsSource(collectionName, i));
            }
            result = resultList;
            return true;
        }
    }

    /// <summary>
    /// Base class for dynamic text source.
    /// </summary>
    public class DynamicTextsSourceBase : DynamicObject
    {
        /// <summary>
        /// Resources manager for overriden texts from application entry assembly.
        /// </summary>
        protected static readonly ResourceManager _appAssemblyTextResourceManager;

        /// <summary>
        /// Resources manager for default texts from Orchestra.Shell assembly.
        /// </summary>
        protected static readonly ResourceManager _orchestraTextResourceManager;
        /// <summary>
        /// Static constructor
        /// </summary>
        static DynamicTextsSourceBase()
        {
            var appAssembly = Assembly.GetEntryAssembly();
            var orchestraAssembly = Assembly.GetExecutingAssembly();
            _orchestraTextResourceManager = new ResourceManager("Orchestra.Resources.Texts", orchestraAssembly);
            _appAssemblyTextResourceManager = new ResourceManager(string.Format("{0}.Resources.Texts", appAssembly.GetName().Name), appAssembly);
        }

        /// <summary>
        /// Returns string value from resources for property
        /// </summary>
        /// <param name="name">Name of text as key in resources.</param>
        /// <param name="result">Value of dynamic text property.</param>
        /// <returns>Always true</returns>
        protected static bool GetDynamicStringProperty(string name, out object result)
        {
            result = _appAssemblyTextResourceManager.GetString(name);
            if (result == null)
            {
                //falback to default texts from Orchestra.Shell assembly
                result = _orchestraTextResourceManager.GetString(name);
            }
            return true;
        }
    }
}