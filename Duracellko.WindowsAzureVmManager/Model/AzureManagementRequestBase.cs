﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Duracellko.WindowsAzureVmManager.Client;

namespace Duracellko.WindowsAzureVmManager.Model
{
    public abstract class AzureManagementRequestBase<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        #region Fields

        private readonly IAzureManagementClient client;

        #endregion

        #region Constructor

        static AzureManagementRequestBase()
        {
            WindowsAzureNamespace = XNamespace.Get("http://schemas.microsoft.com/windowsazure");
        }

        protected AzureManagementRequestBase(IAzureManagementClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }

        #endregion

        #region Properties

        public static XNamespace WindowsAzureNamespace { get; private set; }

        #endregion

        #region Public methods

        public async Task<TResponse> Submit(TRequest request)
        {
            var path = this.GetPath(request);
            var requestXml = this.SerializeRequest(request);
            var responseXml = await this.client.GetResponseAsync(path, requestXml);
            return this.DeserializeResponse(responseXml);
        }

        #endregion

        #region Protected methods

        protected abstract string GetPath(TRequest request);

        protected virtual XDocument SerializeRequest(TRequest request)
        {
            return null;
        }

        protected abstract TResponse DeserializeResponse(XDocument xml);

        #endregion
    }
}
