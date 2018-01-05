using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;

namespace UploadAzureBlobs
{
    class Program
    {
        const string StorageAccountName = "gsazurecsharp";
        const string StorageAccountKey = "871nNIZfCqMfbq/xmX2ipA9VPW0zbfn7jr1E9MlfW39wlBSvP0VyQMbdsABMkvJa3AwsM7fQ7X5gzoJhxUh2lw==";
        const string FolderPath = @"C:\code\gsazurecsharpdocs";

        static void Main(string[] args)
        {
            var storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("gsazuerecontainer");
            container.CreateIfNotExists();

            container.SetPermissions(new Microsoft.WindowsAzure.Storage.Blob.BlobContainerPermissions()
            {
                PublicAccess = Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Blob
            });

            foreach (var filepath in Directory.GetFiles(FolderPath, "*.*", SearchOption.AllDirectories))
            {
                var blob = container.GetBlockBlobReference(filepath);
                blob.UploadFromFile(filepath);

                Console.WriteLine("Upload {0}", filepath);
            }

            Console.WriteLine("Completed");
        }
    }
}
