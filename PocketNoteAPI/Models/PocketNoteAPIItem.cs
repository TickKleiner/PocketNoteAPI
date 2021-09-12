using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketNoteAPI.Models
{
    //Объект содержащий информацию о файле
    public class File
    {
        public File() {}

        public File(string _FileId, string _GoogleUserId)
        {
            FileId = _FileId;
            GoogleUserId = _GoogleUserId;
        }

        public void update_data(FileDTO file)
        {
            EncryptedName = file.EncryptedName;
            Type = file.Type;
            Signature = file.Signature;
            UploadDate = file.UploadDate;
        }

        //FileID same as FileId on google drive
        [Key]
        public string FileId { get; set; }

        [ForeignKey("PocketNoteAPIItem")]
        public string GoogleUserId { get; set; }
        public PocketNoteAPIItem PocketNoteAPIItem { get; set; }
        //Encrypted file name
        public string EncryptedName { get; set; }
        //File type
        public string Type { get; set; }
        //Upload date
        public string UploadDate { get; set; }
        //Signatyre to protect against unwanted spoofing
        public string Signature { get; set; }
	}

	public class Session
	{
        public Session() {}
        public Session(string _DeviceId, string _GoogleUserId)
        {
            GoogleUserId = _GoogleUserId;
            DeviceId = _DeviceId;
        }

        //Обновить сессию
        public void UpdateSession(string _AuthDate, string _Ip)
        {
            AuthDate = _AuthDate;
            Ip = _Ip;
        }
        // Keys start
        [Key]
        public string GoogleUserId { get; set; }
        [Key]
        public string DeviceId { get; set; }
        // keys end
        public string AuthDate { get; set; }
        public string Ip { get; set; }
        public Device Device { get; set; }
    }

	//Объект содержащий информацию о девайсе
	public class Device
    {
        public Device()
        {
            Sessions = new List<Session>();
        }
        public Device(string _DeviceId)
        {
            DeviceId = _DeviceId;
        }

        //Device id same as... Native device id
        [Key]
        public string DeviceId { get; set; }

        [ForeignKey("PocketNoteAPIItem")]
        public string GoogleUserId { get; set; }
        public virtual List<Session> Sessions { get; set; }
        public PocketNoteAPIItem PocketNoteAPIItem { get; set; }
    }

    //Основной объект хранилища
    public class PocketNoteAPIItem
    {
        public PocketNoteAPIItem() {}
        public PocketNoteAPIItem(PostSignUpDataDTO postSignUpDataDTO)
        {
            GoogleUserId = postSignUpDataDTO.GoogleUserId;
            PublicKey = postSignUpDataDTO.PublicKey;
            PrivateKey = postSignUpDataDTO.PrivateKey;
            Pin = postSignUpDataDTO.Pin;
        }
        //User id, same with google user id
        [Key]
        public string GoogleUserId { get; set; }
        //AuthToken for current seesion to protect against CSRF 
        public string AuthToken { get; set; }
        //Storage pin(in progress)
        public short Pin { get; set; }
        //Private key to decrypt data
        public string PrivateKey { get; set; }
        //Public key to encrypt data
        public string PublicKey { get; set; }
        //Список файлов пользователя
        public virtual List<File> Files { get; set; }
        //Список девайсов на которых производилась аутентификация
        public virtual List<Device> Devices { get; set; }
    }

    //Класс для загрузки информации о пользователе в бд
    public class PostSignUpDataDTO
    {
        public PostSignUpDataDTO() {}
        public string GoogleUserId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public short Pin { get; set; }
    }

    //Класс для аутентификации
    public class PostAuthDataDTO
    {
        public PostAuthDataDTO() {}
        public string DeviceId { get; set; }
        public string AuthDate { get; set; }
        public string Ip { get; set; }
    }

    //Получить токен для аутентификации
    public class GetAuthTokenDTO
    {
        public GetAuthTokenDTO()
        {
            AuthToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
        public string AuthToken { get; set; }
    }

    //Класс для получения постАутентификационной информации
    public class GetAppDataDTO
    {
        public GetAppDataDTO() { }
        public GetAppDataDTO(PocketNoteAPIItem pocketNoteAPIItem)
        {
            Pin = pocketNoteAPIItem.Pin;
            PublicKey = pocketNoteAPIItem.PublicKey;
        }
        public short Pin { get; set; }
        public string PublicKey { get; set; }
        public List<Session> Sessions { get; set; }
    }

    //Класс для получения информации для загрузки файлов
    public class GetDownloadDataDTO
    {
        public GetDownloadDataDTO() { }
        public GetDownloadDataDTO(PocketNoteAPIItem pocketNoteAPIItem)
        {
            PrivateKey = pocketNoteAPIItem.PrivateKey;
        }
        public void FilesToFileDTO(List<File> rawFiles){
            Files = new List<FileDTO>();    
            foreach(File rawFile in rawFiles){
                Files.Add(new FileDTO(rawFile));
			}
		}
        public string PrivateKey { get; set; }
        public List<FileDTO> Files { get; set; }
    }

    //Класс загружаемого файла
    public class FileDTO
    {
        public FileDTO() {}
        public FileDTO(File rawFile) {
            FileId = rawFile.FileId;
            EncryptedName = rawFile.EncryptedName;
            Type = rawFile.Type;
            UploadDate = rawFile.UploadDate;
            Signature = rawFile.Signature;
        }
        public string FileId { get; set; }
        //Encrypted file name
        public string EncryptedName { get; set; }
        //File type
        public string Type { get; set; }
        //Upload date
        public string UploadDate { get; set; }
        //Signatyre to protect against unwanted spoofing
        public string Signature { get; set; }
    }
    //Класс для загрузки информации о файлах в бд
    public class PostFilesDTO
    {
        public PostFilesDTO() {}
        public List<FileDTO> Files { get; set; }
    }

    //Класс для удаления файлов из бд
    public class PatchFilesDTO
    {
        public PatchFilesDTO() {}
        public List<string> FileIDs { get; set; }
    }
}