using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Services.Common.Interfaces.FileSystem
{
	public interface IFileWorkerService
	{
		//Task<IFileSource> PickFileAsync(enFileWorkerLocation startupLocation = enFileWorkerLocation.Default, params string[] typeFilters);
		//Task<enFileOperationResult> MoveFileAsync(IFileSource fileSource, enFileWorkerLocation locationToSave, string newName);
		//Task<enFileOperationResult> RenameFileAsync(IFileSource fileSource, string newName);
		//Task<bool> CheckFileExistsInFolderAsync(IFolderSource folderSource, string fileName);
		//Task<bool> CheckFileExistsInSameFolderAsync(IFileSource fileSource, string fileName);
		//Task<IFolderSource> GetParentFolderAsync(IFileSource fileSource);
		//Task<enFileOperationResult> SaveToTextFileAsync(string fileName, string data, enFileWorkerLocation locationToSave = enFileWorkerLocation.Default, bool isReplacingExisted = false);
		//Task<enFileOperationResult> AddToTextFileAsync(string fileName, string data, enFileWorkerLocation locationToSave = enFileWorkerLocation.Default);
		//Task<IFileSource> GetFileAsync(string filePath, bool isNew = false);
		//Task<IFileSource> TryGetFileAsync(string fileName, enFileWorkerLocation location);
		//Task<IFolderSource> ResolveLocationAsync(enFileWorkerLocation location);
		//Task<byte[]> GetFileData(IFileSource fileSource);
		//Task<IFileSource> CacheFileAsync(Stream stream, string cacheFileName, bool fileIsNew = false);
		Task AddToTextFileAsync(string logFile, string message);
	}
}
