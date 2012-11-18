using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ActueelNS.Services
{
  
    public enum StorageType
    {
        Roaming, Local, Temporary
    }
    public class StorageHelper<T>
    {
        private ApplicationData appData = Windows.Storage.ApplicationData.Current;
        private XmlSerializer serializer;
        private StorageType storageType;
        public StorageHelper(StorageType StorageType)
        {
            serializer = new XmlSerializer(typeof(T));
            storageType = StorageType;
        }

        public async void DeleteASync(string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                StorageFolder folder = GetFolder(storageType);

                var file = await GetFileIfExistsAsync(folder, FileName);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void SaveASync(T Obj,string FileName)
        {

            FileName = FileName + ".xml";
            try
            {
                if (Obj != null)
                {
                    StorageFile file = null;
                    StorageFolder folder = GetFolder(storageType);
                    file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                    DataContractJsonSerializer ser = new DataContractJsonSerializer(Obj.GetType());
                    MemoryStream ms = new MemoryStream();
                    ser.WriteObject(ms, Obj);
                    var bytes = ms.ToArray();
                    var contents = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    using (var writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        //Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result;
                        //serializer.Serialize(outStream, Obj);

                        using (var outStream = writeStream.GetOutputStreamAt(0))
                        {
                            using (var dataWriter = new Windows.Storage.Streams.DataWriter(outStream))
                            {
                                dataWriter.WriteString(contents);
                                await dataWriter.StoreAsync();
                                dataWriter.DetachStream();
                                await outStream.FlushAsync();
                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<T> LoadASync(string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                StorageFile file = null;
                StorageFolder folder = GetFolder(storageType);
                file = await folder.GetFileAsync(FileName);

                //using (var readStream = await file.OpenAsync(FileAccessMode.Read))
                //{
                //    Windows.Storage.Streams.DataReader reader = new Windows.Storage.Streams.DataReader(readStream);

                //    await reader.LoadAsync((uint)readStream.Size);
                //    string data = reader.ReadString((uint)readStream.Size);

                //    if (data == null)
                //    {
                //    }
                //}
               

                using (var readStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    //Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result;
                    //return (T)serializer.Deserialize(inStream);

                    var inStream = readStream.GetInputStreamAt(0);
                    Windows.Storage.Streams.DataReader reader = new Windows.Storage.Streams.DataReader(inStream);
                    await reader.LoadAsync((uint)readStream.Size);
                    string data = reader.ReadString((uint)readStream.Size);
                    reader.DetachStream();

                    MemoryStream ms = new MemoryStream(UTF8Encoding.UTF8.GetBytes(data));
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                    T result = (T)ser.ReadObject(ms);
                    return result;

                }
            }
            catch (FileNotFoundException)
            {
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //throw;
            }
            catch (Exception)
                 {
                //Unable to load contents of file
                throw;
            }
        }

        private StorageFolder GetFolder(StorageType storageType)
        {
            StorageFolder folder;
            switch (storageType)
            {
                case StorageType.Roaming:
                    folder = appData.RoamingFolder;
                    break;
                case StorageType.Local:
                    folder = appData.LocalFolder;
                    break;
                case StorageType.Temporary:
                    folder = appData.TemporaryFolder;
                    break;
                default:
                    throw new Exception(String.Format("Unknown StorageType: {0}", storageType));
            }
            return folder;
        }

        private async Task<StorageFile> GetFileIfExistsAsync(StorageFolder folder, string fileName)
        {
            try
            {
                return await folder.GetFileAsync(fileName);

            }
            catch
            {
                return null;
            }
 }

    }
}
