using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Stream> GetStreamAsync(string name, CancellationToken cancellationToken)
        {
            var block = _cloudBlobContainer.GetBlockBlobReference(name);
            var isExists = await block.ExistsAsync(cancellationToken);
            if (!isExists)
            {
                throw new KeyNotFoundException();
            }

            return await block.OpenReadAsync(cancellationToken);
        }

        public async Task<string> UploadAsync(Stream stream, string name, CancellationToken cancellationToken)
        {
            stream.Seek(0, SeekOrigin.Begin);
            if (stream.Length == 0)
            {
                throw new ArgumentException(nameof(stream));
            }

            var block = _cloudBlobContainer.GetBlockBlobReference(name);

            await block.UploadFromStreamAsync(stream, cancellationToken);

            return block.Uri.ToString();
        }
    }
}
