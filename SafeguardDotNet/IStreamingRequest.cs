﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OneIdentity.SafeguardDotNet
{

    /// <summary>
    /// HTTP streaming request methods
    /// </summary>
    public interface IStreamingRequest : IDisposable
    {
        /// <summary>
        /// Call a Safeguard POST API providing a stream as request content. If there is a 
        /// failure a SafeguardDotNetException will be thrown.
        /// </summary>
        /// <param name="service">Safeguard service to call.</param>
        /// <param name="relativeUrl">Relative URL of the service to use.</param>
        /// <param name="stream">Stream to upload as request content.</param>
        /// <param name="progress">Optionally report upload progress.</param>
        /// <param name="parameters">Additional parameters to add to the URL.</param>
        /// <param name="additionalHeaders">Additional headers to add to the request.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Response body as a string.</returns>
        Task<string> UploadAsync(Service service, string relativeUrl, Stream stream, IProgress<TransferProgress> progress = null, IDictionary<string, string> parameters = null, IDictionary<string, string> additionalHeaders = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Call a Safeguard GET API providing an output file path to which streaming download data will
        /// be written. If there is a failure a SafeguardDotNetException will be thrown.
        /// </summary>
        /// <param name="service">Safeguard service to call.</param>
        /// <param name="relativeUrl">Relative URL of the service to use.</param>
        /// <param name="outputFilePath">Full path to the file where download will be written.</param>
        /// <param name="body">Optional request body</param>
        /// <param name="progress">Optionally report upload progress.</param>
        /// <param name="parameters">Additional parameters to add to the URL.</param>
        /// <param name="additionalHeaders">Additional headers to add to the request.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the async operation</returns>
        Task DownloadAsync(Service service, string relativeUrl, string outputFilePath, string body = null, IProgress<TransferProgress> progress = null, IDictionary<string, string> parameters = null, IDictionary<string, string> additionalHeaders = null, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Call a Safeguard GET API returning output as a stream. The caller takes ownership of the
        /// StreamResponse and should dispose it when finished. Disposing the StreamResponse will dispose
        /// the stream and related resources.
        /// If there is a failure a SafeguardDotNetException will be thrown.
        /// </summary>
        /// <param name="service">Safeguard service to call.</param>
        /// <param name="relativeUrl">Relative URL of the service to use.</param>
        /// <param name="body">Optional request body</param>
        /// <param name="progress">Optionally report upload progress.</param>
        /// <param name="parameters">Additional parameters to add to the URL.</param>
        /// <param name="additionalHeaders">Additional headers to add to the request.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A StreamResponse. Call GetStream() to get the stream object.</returns>
        Task<StreamResponse> DownloadStreamAsync(Service service, string relativeUrl, string body = null, IProgress<TransferProgress> progress = null, IDictionary<string, string> parameters = null, IDictionary<string, string> additionalHeaders = null, CancellationToken? cancellationToken = null);
    }
}