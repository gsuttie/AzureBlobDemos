using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadAzureBlobs
{
    class Program
    {
        const string StorageAccountName = "gsazurecsharp";
        const string StorageAccountKey = "871nNIZfCqMfbq/xmX2ipA9VPW0zbfn7jr1E9MlfW39wlBSvP0VyQMbdsABMkvJa3AwsM7fQ7X5gzoJhxUh2lw==";

        static void Main(string[] args)
        {
            var storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("gsazuerecontainer");

            var blobs = container.ListBlobs();

            DownloadAzureBlobs(blobs);

            Console.WriteLine("Completed");
        }

        private static void DownloadAzureBlobs(IEnumerable<IListBlobItem> blobs)
        {
            foreach (var blob in blobs)
            {
                if (blob is CloudBlockBlob blockblob)
                {
                    blockblob.DownloadToFile(blockblob.Name, FileMode.Create);
                    Console.WriteLine("blob name is {0}", blockblob.Name);
                }
                else if (blob is CloudBlobDirectory blobDirectory)
                {
                    Directory.CreateDirectory(blobDirectory.Prefix);
                    Console.WriteLine("Create Directory {0}", blobDirectory.Prefix);

                    DownloadAzureBlobs(blobDirectory.ListBlobs());
                }
            }
        }
    }
}
