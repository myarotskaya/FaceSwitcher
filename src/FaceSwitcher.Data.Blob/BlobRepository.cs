using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceSwitcher.Data.Blob
{
    public class BlobRepository : IBlobRepository
    {
        private readonly CloudBlobContainer _cloudBlobContainer;

        public BlobRepository(string connectionString, string containerName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _cloudBlobContainer = CloudStorageAccount
                .Parse(connectionString)
                .CreateCloudBlobClient()
                .GetContainerReference(containerName);
        }

        public async Task<string> GetUrlAsync(string name, CancellationToken cancellationToken)
        {
            var block = (CloudBlockBlob)_cloudBlobContainer
                .ListBlobs(name)
                .FirstOrDefault();

            var isExists = block != null && await block.ExistsAsync(cancellationToken);
            if (!isExists)
            {
                throw new KeyNotFoundException("Blob is not found.");
            }

            return block.Uri.ToString();
        }

        public async Task DownloadAsync(Stream stream, string name, CancellationToken cancellationToken)
        {
            var block = _cloudBlobContainer.GetBlockBlobReference(name);

            var isExists = await block.ExistsAsync(cancellationToken);
            if (!isExists)
            {
                throw new KeyNotFoundException("Blob is not found.");
            }

            await block.DownloadToStreamAsync(stream, cancellationToken);
        }

        public async Task UploadAsync(Stream stream, string name, string contentType, CancellationToken cancellationToken)
        {
            if (stream.Length == 0 || !stream.CanSeek)
            {
                throw new ArgumentException("Incorrect stream.");
            }

            stream.Seek(0, SeekOrigin.Begin);
            var block = _cloudBlobContainer.GetBlockBlobReference(name);

            block.Properties.ContentType = contentType;

            await block.UploadFromStreamAsync(stream, cancellationToken);
        }
    }
}
